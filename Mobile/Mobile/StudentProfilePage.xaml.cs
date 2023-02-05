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
using ZXing;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfilePage : ContentPage
    {
		private Users user;

		public Users User
		{
			get { return user; }
			set { user = value; OnPropertyChanged(); }
		}

		public StudentProfilePage()
        {
            InitializeComponent();
			BindingContext = this;
        }
		protected override async void OnAppearing()
        {
			try
			{
				var UserId = Preferences.Get("UserID", Guid.Empty.ToString());
				using (HttpClient client = new HttpClient())
				{
					var response = await client.GetAsync($"{App.UrlAPI}/Users/{UserId}");
					User = await response.Content.ReadAsAsync<Users>();
				}
			}
			catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "StudentProfilePage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }

        }
		private async void SaveProfile(object sender, EventArgs e)
		{
			try
			{
				var UserId = Preferences.Get("UserID", Guid.Empty.ToString());
				using (HttpClient client = new HttpClient())
				{
					var response = await client.PostAsJsonAsync($"{App.UrlAPI}/Users/UpdateProfile", User);
					var IsUpdate = await response.Content.ReadAsAsync<bool>();
					if (IsUpdate) 
					{
						await DisplayAlert("React App", "Your Profile is updated successfully", "OK");
					}
					else
					{
						await DisplayAlert("React App", "Your Profile is Not updated, Try again later", "OK");
					}
				}
			}
			catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "SaveProfile" },
                    { "Class", "StudentProfilePage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }

        }

        private void HideOrShowPassword(object sender, EventArgs e)
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