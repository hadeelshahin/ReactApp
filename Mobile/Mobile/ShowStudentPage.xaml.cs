using Microsoft.AppCenter.Crashes;
using Mobile.Interfaces;
using ModelsShared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowStudentPage : ContentPage
    {
        public Guid IdClass { get; set; }
        private List<Users> users;

        public List<Users> Users
		{
            get { return users; }
            set { users = value; OnPropertyChanged(); }
        }

        public ShowStudentPage(Guid classId)
        {
			IdClass = classId;

			InitializeComponent();
        }
		protected override async void OnAppearing()
        {
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var response = await client.GetAsync($"{App.UrlAPI}/Classes/GetStudents/{IdClass}");
					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadAsAsync<List<Users>>();
						if (result != null)
						{
							Users = result;
							webView.Source = GenerateHTML();
						}
					}
				}
			}
			catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "ShowStudentPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }
		private void Print(object sender, EventArgs e)
        {
            var printService = DependencyService.Get<IPrintService>();
            printService.Print(webView, $"ReactApp-ShowStudent-{DateTime.Now.Ticks}");
        }

        public HtmlWebViewSource GenerateHTML()
        {
			// New up the Razor template
			var printTemplate = new Mobile.PrintTemplates.ListPrintTemplate();

			// Set the model property (ViewModel is a custom property within containing view - FYI)
			printTemplate.Model = Users;

			// Generate the HTML
			var htmlString = printTemplate.GenerateString();

			// Create a source for the webview
			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = htmlString;
			return htmlSource;
		}
	}
}