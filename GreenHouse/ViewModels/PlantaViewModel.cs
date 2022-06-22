using GreenHouse.Models;
using GreenHouse.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GreenHouse.ViewModels
{
    public class PlantaViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
      
        public ObservableCollection<Planta> Invernadero { get; set; } = new ObservableCollection<Planta>();
        public string Mensaje { get; set; } = "";
        public Planta Planta { get; set; }
  

        public ICommand AgregarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand VerDetallesCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        AgregarView AgregarP;
        EditarView EditarP;
        DetallesView Detalles;

        public PlantaViewModel()
        {
            Abrir();
            AgregarCommand = new Command(Agregar);
            CambiarVistaCommand = new Command<string>(CambiarVista);
            EliminarCommand = new Command<Planta>(Eliminar);
            VerDetallesCommand = new Command<Planta>(VerDetalles);
            EditarCommand = new Command<Planta>(Editar);
            GuardarCommand = new Command(Guardar);
        }

        int p;
        private void Editar(Planta planta)
        {
            p = Invernadero.IndexOf(Planta);
            if (Planta != null)
                {
                this.Planta = new Planta
            {
                NombreComun = Planta.NombreComun,
                NombreCientifico = Planta.NombreCientifico,
                Tipo = Planta.Tipo,
                Altura = Planta.Altura,
                Observaciones = Planta.Observaciones,
                Fotografia = Planta.Fotografia
            };
                
            }
            EditarP = new EditarView()
            {
                BindingContext = this
            };
            Application.Current.MainPage.Navigation.PushAsync(EditarP);
        }

        private void Guardar()
        {   
            Invernadero[p] = Planta;  
            Cargar();
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        private void VerDetalles(Planta planta)
        {
            Detalles = new DetallesView()
            {
                BindingContext = planta
            };
            Application.Current.MainPage.Navigation.PushAsync(Detalles);
            PropertyChanged(this, new PropertyChangedEventArgs(null));
        }

        private void Eliminar(Planta planta)
        {
            Invernadero.Remove(planta);
            Cargar();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private void Agregar()
        {
            if (Planta!=null)
            {
                if(string.IsNullOrWhiteSpace(Planta.NombreComun))
                {
                    Mensaje = "El campo de Nombre Común esta vacío, asegúrese de escribirlo correctamente.";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                    return;
                }

                if (string.IsNullOrWhiteSpace(Planta.NombreCientifico))
                {
                    Mensaje = "El campo de Nombre Científico esta vacío, asegúrese de escribirlo correctamente.";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                    return;
                }

                if (string.IsNullOrWhiteSpace(Planta.Tipo))
                {
                    Mensaje = "Seleccione el tipo de planta.";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                    return;
                }
                if (string.IsNullOrWhiteSpace(Planta.Altura))
                {
                    Mensaje = "El campo de Altura esta vacío.";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                    return;
                }
                if (string.IsNullOrWhiteSpace(Planta.Observaciones))
                {
                    Mensaje = "El campo de Observaciones esta vacío.";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                    return;
                }
                if (string.IsNullOrWhiteSpace(Planta.Fotografia))
                {
                    Mensaje = "Proporcione una imagen mediante una URL.";
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                        return;
                    }
                if (!Uri.TryCreate(Planta.Fotografia, UriKind.Absolute, out var uri))
                {
                    Mensaje = "URL no válida, vuelva a intentarlo.";
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                        return;
                        }
            Invernadero.Add(Planta);
            }
            Cargar();
            CambiarVista("ver");
        }

        private void CambiarVista(string Vista)
        {
            if (Vista == "agregar")
            {
                Mensaje = "";
                Planta = new Planta();
                AgregarP = new AgregarView() { BindingContext = this };
                Application.Current.MainPage.Navigation.PushAsync(AgregarP);
                PropertyChanged(this, new PropertyChangedEventArgs(null));
            }
            if (Vista == "ver")
            {
                Application.Current.MainPage.Navigation.PopToRootAsync();
            } 
        }

        void Cargar()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "InvernaderoP.json";
            File.WriteAllText(file, JsonConvert.SerializeObject(Invernadero));
        }

        void Abrir()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "InvernaderoP.json";
            if (File.Exists(file))
            {
                Invernadero = JsonConvert.DeserializeObject<ObservableCollection<Planta>>(File.ReadAllText(file));
            }
        }
    }
}
