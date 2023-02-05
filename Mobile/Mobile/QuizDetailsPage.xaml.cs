using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using ModelsShared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuizDetailsPage : ContentPage
    {
		private QuizListVM quizList;

		public QuizListVM QuizList
		{
			get { return quizList; }
			set { quizList = value; OnPropertyChanged(); }
		}

		private int numberOfStudent;
		public int NumberOfStudent
		{
			get { return numberOfStudent; }
			set { numberOfStudent = value; OnPropertyChanged(); }
		}
		private int netOfNumberOfStudent;
		public int NetOfNumberOfStudent
		{
			get { return netOfNumberOfStudent; }
			set { netOfNumberOfStudent = value; OnPropertyChanged(); }
		}
		private string quizDuration;
		public string QuizDuration
		{
			get { return quizDuration; }
			set { quizDuration = value; OnPropertyChanged(); }
		}
		public Guid QuizListId { get; set; }
		public QuizDetailsPage(Guid quizListId)
        {
			QuizListId = quizListId;
            InitializeComponent();
			BindingContext = this;
		}

		protected override async void OnAppearing()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var responseQuizList = await client.GetAsync($"{App.UrlAPI}/QuizLists/{QuizListId}");
					var resultQuizList = await responseQuizList.Content.ReadAsAsync<QuizListVM>();
					if (responseQuizList.IsSuccessStatusCode)
					{
						QuizList  = resultQuizList;
					}
					var response = await client.GetAsync($"{App.UrlAPI}/Classes/GetCountOfStudents/{QuizList.ClassesId}");
					var result = await response.Content.ReadAsAsync<int>();
					if (response.IsSuccessStatusCode)
					{
						NumberOfStudent = result;
					}
					var responseStudentAnswers = await client.GetAsync($"{App.UrlAPI}/StudentAnswersQustions/{QuizList.Id}");
					var resultStudentAnswers = await responseStudentAnswers.Content.ReadAsAsync<List<StudentAnswersQustions>>();
					if (responseStudentAnswers.IsSuccessStatusCode)
					{
						NetOfNumberOfStudent = resultStudentAnswers.Count;
					}
				}
				switch (QuizList.QuizTimeType)
				{
					case QuizTimeType.Interval:
						QuizDuration = QuizList.FromTime.TimeOfDay.ToString(@"mm\:ss");
						break;
					case QuizTimeType.FromTo:
						var FromTime = quizList.FromTime.ToString(@"mm\:ss tt");
						var ToTime = quizList.ToTime;
						TimeSpan duration = DateTime.Parse(ToTime.ToString(@"mm\:ss tt")).Subtract(DateTime.Parse(FromTime));
						QuizDuration = duration.ToString(@"hh\:mm\:ss");
						break;
					case QuizTimeType.Unlimited:
						QuizDuration = "Unlimited";
						break;
				}
			}
			catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "QuizDetailsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }
	}
}