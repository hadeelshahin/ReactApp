using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class StudentDashboardPage : ContentPage
    {
		private ObservableCollection<QuizList> quizList;

		public ObservableCollection<QuizList> QuizList
		{
			get { return quizList; }
			set { quizList = value; OnPropertyChanged(); }
		}
        private bool isClassesNotExist = true;

        public bool IsClassesNotExist
        {
            get { return isClassesNotExist; }
            set { isClassesNotExist = value; OnPropertyChanged(); }
        }


        public StudentDashboardPage()
        {
            InitializeComponent();
			BindingContext = this;
        }

		protected override async void OnAppearing()
		{
			try
			{
                IsBusy = true;
                using (HttpClient client = new HttpClient())
                {
                    var UserId = Preferences.Get("UserID", "");
                    var response = await client.GetAsync($"{App.UrlAPI}/StudentAnswersQustions/GetQuizForStudent/{UserId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<List<QuizList>>();
                        if (result != null)
                        {
                            QuizList = new ObservableCollection<QuizList>(result);
                        }
                    }
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (QuizList.Count > 0)
                    {
                        IsClassesNotExist = false;
                    }
                    IsBusy = false;
                });
            }
            catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "StudentDashboardPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }
		private void GoToScanPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ScanQRCodePage());
        }

		private void GoToProfile(object sender, EventArgs e)
		{
			Navigation.PushAsync(new StudentProfilePage());
		}
	}
}