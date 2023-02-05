using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using ModelsShared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateClassesPage : ContentPage
    {
        public string ClassName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public CreateClassesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void GenrateQRCode(object sender, EventArgs e)
        {
            try
            {
                var UserId = Preferences.Get("UserID", "");

                if (!string.IsNullOrEmpty(ClassName))
                {
                    var classesVM = new ClassesVM
                    {
                        ClassName = ClassName,
                        StartTime = new DateTime() + StartTime,
                        EndTime = new DateTime() + EndTime,
                        TeacherId = Guid.Parse(UserId)
                    };

                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.PostAsJsonAsync($"{App.UrlAPI}/Classes", classesVM);
                        var result = await response.Content.ReadAsAsync<Classes>();
                        if (response.IsSuccessStatusCode)
                        {
                            await Navigation.PushAsync(new GenerateQRCodePage(result.Id, result.IdCode, GenerateQRCodePage.TypeSacnEnum.JoinClass));
                        }
                    }
                }
                else
                    await DisplayAlert("React App", "Class name is empty", "Ok");
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "GenrateQRCode" },
                    { "Class", "CreateClassesPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }
    }
}