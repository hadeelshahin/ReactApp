using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    public partial class App : Application
    {
        public static string UrlAPI = "http://reactappproject.azurewebsites.net/api";
        //public static string UrlAPI = "http://192.168.1.101:5000/api";
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new SplashScreen());
		}

		protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
