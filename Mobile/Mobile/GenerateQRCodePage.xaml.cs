using Microsoft.AppCenter.Crashes;
using Mobile.Interfaces;
using ModelsShared.Models;
using ModelsShared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenerateQRCodePage : ContentPage
    {
        public string ImageQRCodeURL { get; set; }
        private string qrCodeUrl;
        public string QRCodeURL 
        {
            get { return qrCodeUrl; }
            set { qrCodeUrl = value; OnPropertyChanged(); }
        }
        private string idCode;
        public string IDCode
        {
            get { return idCode; }
            set { idCode = value; OnPropertyChanged(); }
        }
        public Guid ClassId { get; set; }
        public enum TypeSacnEnum
        {
            JoinClass,
            JoinQuiz
        }
        public TypeSacnEnum TypeSacn { get; set; }

        public GenerateQRCodePage(Guid id, string IdCode, TypeSacnEnum typeSacn)
        {
            InitializeComponent();
            ClassId = id;
            IDCode = IdCode;
            BindingContext = this;
            IsBusy = true;
            TypeSacn = typeSacn;
            ImageQRCodeURL = $"https://qrcode.kaywa.com/img.php?s=20&d={id},{typeSacn}";
            QRCodeImageUrl.Source = ImageSource.FromUri(new Uri(ImageQRCodeURL));
        }

        protected override async void OnAppearing()
        {
            try
            {

                var url = $"https://tinyurl.com/api-create.php?url={ImageQRCodeURL}";
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync(url);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(response))
                        {
                            Device.BeginInvokeOnMainThread(() => QRCodeURL = response);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "GenerateQRCodePage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
            Device.BeginInvokeOnMainThread(() => IsBusy = false);
        }

        private void GoToClasses(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }

        private void ShareLink(object sender, EventArgs e)
        {
            Share.RequestAsync(new ShareTextRequest
            {
                Uri = QRCodeURL,
                Title = "React App",
                Subject = "Share QR code",
                Text = $"Share this QR code to enable student join the class:"
            });
        }

        private async void SaveInGallery(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = await client.GetAsync(ImageQRCodeURL);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsByteArrayAsync();
                        if (response != null)
                        {
                            DependencyService.Get<ISaveInGallery>().SaveImage(response, $"ReactApp-{DateTime.Now.Ticks}");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "SaveInGallery" },
                    { "Class", "GenerateQRCodePage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }

        }
    }
}