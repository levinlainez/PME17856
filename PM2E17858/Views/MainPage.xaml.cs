using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E17858.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            iniciarGps();


        }

        Plugin.Media.Abstractions.MediaFile FileFoto = null;
        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();
                    stream.CopyTo(memory);
                    return memory.ToArray();
                }
            }
            return null;
        }



        protected async  void iniciarGps()
        {
         
            if (!CrossGeolocator.Current.IsGeolocationEnabled)
            {
                await DisplayAlert("Advertencia", "Revise la configuracion de su GPS", "OK");
                return;
            }

            CrossGeolocator.Current.PositionChanged += current_PositionChanged;
            await CrossGeolocator.Current.StartListeningAsync(new TimeSpan(0, 0, 5), 5);
           




        }

        private void current_PositionChanged(object sender, PositionEventArgs e)
        {
            var position = CrossGeolocator.Current.GetPositionAsync();


            txtLatitud.Text = position.Result.Latitude.ToString();
            txtLongitud.Text =position.Result.Longitude.ToString();
        }

       

        private async void btnAgregar_Clicked(object sender, EventArgs e)
        {

            
           
           

                try
            {
                if (txtLatitud.Text == null)
                {
                    await DisplayAlert("Error", "Es necesario tener una Latitud", "OK");
                    return;
                }
                if (txtLongitud.Text == null)
                {
                    await DisplayAlert("Error", "Es necesario tener una Longitud", "OK");
                    return;
                }
                if (txtDescripcion.Text == null)
                {
                    await DisplayAlert("Error", "Es decribir la ubicacion", "OK");
                    return;
                }

                if (FileFoto == null)
                {
                    await DisplayAlert("Error", "NO ha tomado una fotografia", "OK");
                }

                var ubicacion = new Models.Examen()
                {
                    codigo = 0,
                    foto = ConvertImageToByteArray(),
                    latitude =(txtLatitud.Text),
                    longitude = (txtLongitud.Text),
                    descripcion = txtDescripcion.Text
                  



                };

                var resultado = await App.BaseDatos.GuardarUbicacion(ubicacion);

                if (resultado == 1)
                {
                    await DisplayAlert("Agregado", "Ingreso Exitoso", "OK");
                    ClearScreen();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo guardar la ubicacion", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Agregado", ex.Message.ToString(), "OK");
            }

        }

        private void ClearScreen()
        {
            this.txtLatitud.Text = String.Empty;
            this.txtLongitud.Text = String.Empty;
            this.txtDescripcion.Text = String.Empty;
        }
        private async void btnListaSitios_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.listaPage());
        }

        private void btnSalirApp_Clicked(object sender, EventArgs e)
        {
            System.Environment.Exit(0);

        }


        private async void Foto_Clicked(object sender, EventArgs e)
        {

            FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "MisFotos",
                Name = "test.jpg",
                SaveToAlbum = true

            });
            //await DisplayAlert("path directorio", FileFoto.Path, "OK");

            if (FileFoto != null)
            {
                Foto.Source = ImageSource.FromStream(() =>
                {
                    return FileFoto.GetStream();
                });
            }
        }
    }
}
