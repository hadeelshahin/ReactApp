using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
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
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                var UserId = Preferences.Get("UserID", "");
                if (!string.IsNullOrEmpty(UserId))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetAsync($"{App.UrlAPI}/Users/{UserId}");
                        var User = await response.Content.ReadAsAsync<Users>();
                        if (User != null)
                        {
                            if (User.UserType == UserType.Teacher)
                            {
                                App.Current.MainPage = new NavigationPage(new ClassesPage());
                            }
                            else
                            {
                                App.Current.MainPage = new NavigationPage(new StudentDashboardPage());
                            }
                        }
                        else
                        {
                            App.Current.MainPage = new NavigationPage(new MainPage());
                        }
                    }
                }
                else
                {
                    App.Current.MainPage = new NavigationPage(new MainPage());
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "SplashScreen.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
            await Navigation.PopToRootAsync();
        }
    }
}