using Microsoft.AppCenter.Crashes;
using Mobile.Interfaces;
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
	public partial class ResultQuizPage : ContentPage
	{
		public Guid QuizlistId { get; set; }
		public QuizListVM QuizListVM { get; set; }
		public Classes Classes { get; set; }
		private List<StudentAnswersQustions> studentAnswersQustions;

		public List<StudentAnswersQustions> StudentAnswersQustions
		{
			get { return studentAnswersQustions; }
			set { studentAnswersQustions = value; OnPropertyChanged(); }
		}
		private bool isOpen = false;

		public bool IsOpen
		{
			get { return isOpen; }
			set { isOpen = value; OnPropertyChanged(); }
		}

		public ResultQuizPage(QuizListVM quizListVM)
		{
			QuizListVM = quizListVM;
			QuizlistId = quizListVM.Id;

			InitializeComponent();

			BindingContext = this;
		}
		protected override async void OnAppearing()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var response = await client.GetAsync($"{App.UrlAPI}/StudentAnswersQustions/{QuizlistId}");
					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadAsAsync<List<StudentAnswersQustions>>();
						if (result != null)
						{
							StudentAnswersQustions = result;
							webView.Source = GenerateHTML();
						}
					}
				}
			}
			catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "ResultQuizPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }
		private void Print(object sender, EventArgs e)
		{
			var printService = DependencyService.Get<IPrintService>();
			printService.Print(webView, $"ReactApp - Quiz Result - {DateTime.Now.Ticks}");
		}

		public HtmlWebViewSource GenerateHTML()
		{
			// New up the Razor template
			var printTemplate = new Mobile.PrintTemplates.ShowResultPrintTemplate();

			// Set the model property (ViewModel is a custom property within containing view - FYI)
			printTemplate.Model = StudentAnswersQustions;

			// Generate the HTML
			var htmlString = printTemplate.GenerateString();

			// Create a source for the webview
			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = htmlString;
			return htmlSource;
		}
		private void OpenOptionsResultQuiz(object sender, EventArgs e)
		{
			IsOpen = true;
		}

		private void CloseOptionsResultQuiz(object sender, EventArgs e)
		{
			IsOpen = false;
		}

		private void ShowResult(object sender, EventArgs e)
		{
			Navigation.PushAsync(new QuizDetailsPage(QuizlistId));
		}

		private void ShowStatistics(object sender, EventArgs e)
		{
			Navigation.PushAsync(new ResultByChartPage(QuizlistId, 
				QuizListVM.QuizType == QuizType.Normal ? ResultByChartPage.ChartTypeEnum.Bar : ResultByChartPage.ChartTypeEnum.Pie));

		}
	}
}