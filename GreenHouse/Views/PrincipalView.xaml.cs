using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GreenHouse.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalView : ContentPage
    {
        public PrincipalView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var b=(Button)sender;
            if(await DisplayAlert("Confirmación de eliminación",$"¿Quiere eliminar esta planta?","Sí", "No")==true)
            {
                pvm.EliminarCommand.Execute(b.CommandParameter);
            }
        }
    }
}