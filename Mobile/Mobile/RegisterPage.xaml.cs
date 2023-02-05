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
	public partial class RegisterPage : ContentPage
	{
		private string email;
		public string Email
		{
			get { return email; }
			set { email = value; }
		}

		private string fullName;

		public string FullName
		{
			get { return fullName; }
			set { fullName = value; }
		}

		private string password;

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		private string passwordConfirmation;

		public string PasswordConfirmation
		{
			get { return passwordConfirmation; }
			set { passwordConfirmation = value; }
		}

		private Gender gender;

		public Gender Gender
		{
			get { return gender; }
			set { gender = value; }
		}

		private UserType userType;

		public UserType UserType
		{
			get { return userType; }
			set { userType = value; }
		}


		public RegisterPage ()
		{
			InitializeComponent ();
			BindingContext = this;
		}

        private async void SignUp(object sender, EventArgs e)
        {
			try
			{
                IsBusy = true;
                string Errors = string.Empty;
                if (string.IsNullOrWhiteSpace(FullName)) { Errors += "No Full Name Enter\n"; }
                if (string.IsNullOrWhiteSpace(Email)) { Errors += "No Eamil Enter\n"; }
                if (string.IsNullOrWhiteSpace(Password)) { Errors += "No Password Enter\n"; }
                if (string.IsNullOrWhiteSpace(PasswordConfirmation)) { Errors += "No Password confirmation Enter\n"; }

                bool VaildPassword = false;
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    if (!string.IsNullOrWhiteSpace(PasswordConfirmation))
                    {
                        VaildPassword = Password.Equals(PasswordConfirmation);
                    }
                }


                if (string.IsNullOrEmpty(Errors))
                {
                    if (VaildPassword)
                    {
                        var register = new RegisterVM
                        {
                            FullName = FullName,
                            Email = Email,
                            Password = Password,
                            Gender = Gender,
                            UserType = UserType,
                        };

                        using (HttpClient client = new HttpClient())
                        {
                            var response = await client.PostAsJsonAsync($"{App.UrlAPI}/Users/Register", register);
                            var result = await response.Content.ReadAsStringAsync();
                            if (bool.TryParse(result, out bool parsedValue))
                            {
                                if (parsedValue)
                                {
                                    await DisplayAlert("Sign up", "The account was successfully created", "Ok");
                                    await Navigation.PopAsync();
                                }
                                else
                                {
                                    await DisplayAlert("Sign up", "The account not created", "Ok");
                                }
                            }
                            else
                            {
                                await DisplayAlert("Sign up", result, "Ok");
                            }

                        }
                    }
                    else
                    {
                        await DisplayAlert("Sign up", $"password confirmation does not match password", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Sign up", $"Please fill the data:\n{Errors}", "Ok");
                }

            }
            catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "SignUp" },
                    { "Class", "RegisterPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
            IsBusy = false;
        }

        private void GenderChanged(object sender, CheckedChangedEventArgs e)
        {
			var value = (sender as RadioButton).Value.ToString();
            Gender = (Gender)Enum.Parse(typeof(Gender), value);
        }

        private void AccountTypeChanged(object sender, CheckedChangedEventArgs e)
        {
            var value = (sender as RadioButton).Value.ToString();
            UserType = (UserType)Enum.Parse(typeof(UserType), value);
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

        private void HideOrShowPasswordConfirmation(object sender, EventArgs e)
        {
            var imageButton = ((ImageButton)sender);
            if (PasswordConfirmationEntry.IsPassword)
            {
                PasswordConfirmationEntry.IsPassword = false;
                imageButton.Source = "hidePassword";
            }
            else
            {
                PasswordConfirmationEntry.IsPassword = true;
                imageButton.Source = "showPassword";
            }

        }
    }
}