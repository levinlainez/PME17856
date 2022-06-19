using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E17858.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class listaPage : ContentPage
    {
        Models.Examen item;


        public listaPage()
        {
            InitializeComponent();
        }

        private void ListUbicacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var listaubicacion = await App.BaseDatos.ObtenerListaUbicaciones();
            lsubicacion.ItemsSource = listaubicacion;


        }

        private async void informacion_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Acciones", "Para eliminar tarjeta solo deslicela hacia la derecha, Para ir al mapa solo deslicela hacia la izquierda", "Entendido");
        }


        private void lsubicacion_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            item = (Models.Examen)e.Item;
        }

        private async void mapa_Invoked(object sender, EventArgs e)
        {
            if (item == null)
            {
                await DisplayAlert("Error", "Debe seleccionar una fila", "OK");
                return;
            }

            bool answer = await DisplayAlert("Accion", "Desea ir a la ubicacion indicada?", "Si", "No");
            Debug.WriteLine("Answer: " + answer);
            if (answer == true)
            {
                if (!Double.TryParse(item.latitude, out double lat))
                {
                    return;

                }
                if (!Double.TryParse(item.longitude, out double lng))
                {
                    return;

                }

                await Map.OpenAsync(lat, lng, new MapLaunchOptions
                {
                    Name =item.descripcion,
                   NavigationMode = NavigationMode.None

                });
            }

        

        }

        private async void eliminar_Invoked(object sender, EventArgs e)
        {
            try
            {
                var ubicacion = new Models.Examen()
                {
                    codigo = Convert.ToInt32(item.codigo),
                    longitude = item.longitude,
                    latitude = item.latitude,
                    descripcion = item.descripcion,
                    foto=item.foto

                };

                var resultado = await App.BaseDatos.EliminarUbicacion(ubicacion);

                if (resultado > 0)
                {
                    await DisplayAlert("Eliminar", "Datos eliminados correctamente", "OK");
                    await Navigation.PushAsync(new Views.listaPage());
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar la ubicacion", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Eliminar", ex.Message.ToString(), "OK");
            }
        }

        private async void compartirr_Clicked(object sender, EventArgs e)
        {
            if (item == null)
            {
                await DisplayAlert("Error", "Por favor seleccione una fila", "OK");
                return;
            }

            var image = await MediaPicker.PickPhotoAsync();

            if (image == null)
            {
                return;
            }

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Gerald's YouTube",
                File = new ShareFile(image)
            });
        }
    }
}