using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using ModelsShared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile
{
    public partial class MainPage : ContentPage
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void GoToRegisterPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
        }

        private async void Login(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
                var loginVM = new LoginVM { Email = Email, Password = Password };

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync($"{App.UrlAPI}/Users/Login", loginVM);
                    if (response.StatusCode != System.Net.HttpStatusCode.NotAcceptable)
                    {
                        var result = await response.Content.ReadAsAsync<Users>();
                        Preferences.Set("UserID", result.Id.ToString());
                        if (result.UserType == UserType.Teacher)
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
                        await DisplayAlert("Login", await response.Content.ReadAsStringAsync() , "Ok");
                    }
                }
                IsBusy = false;

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "Login" },
                    { "Class", "MainPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);

            }
        }

        private void HideOrShowPasswrod(object sender, EventArgs e)
        {
            var imageButton = ((ImageButton)sender);
            if (PasswordEntry.IsPassword)
            {
                PasswordEntry.IsPassword = false;
                imageButton.Source = "hidePassword";
            }
            else
            {
                PasswordEntry.IsPassword = true;
                imageButton.Source = "showPassword";
            }
        }
    }
}
