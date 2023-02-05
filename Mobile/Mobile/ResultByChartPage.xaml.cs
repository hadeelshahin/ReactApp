using Microcharts;
using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using ModelsShared.ViewModels;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Mobile.CreateQustionsPage;

namespace Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultByChartPage : ContentPage
	{
		public Guid QuizListID { get; set; }
		public ChartTypeEnum ChartType { get; set; }
		public List<QuizVM> Quizzes { get; set; }
		public List<StudentQustion> StudentQustion { get; set; }

		private string titleQustion;
		public string TitleQustion
		{
			get { return titleQustion; }
			set { titleQustion = value; OnPropertyChanged(); }
		}

		private string correctAnswer;
		public string CorrectAnswer
		{
			get { return correctAnswer; }
			set { correctAnswer = value; OnPropertyChanged(); }
		}

		public ResultByChartPage(Guid quizListID, ChartTypeEnum chartType)
		{
			QuizListID = quizListID;
			InitializeComponent();
			BindingContext = this;
		}

		protected override async void OnAppearing()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var response = await client.GetAsync($"{App.UrlAPI}/Quizs/{QuizListID}");
					if (response.IsSuccessStatusCode)
					{
						var result = await response.Content.ReadAsAsync<List<QuizVM>>();
						if (result != null)
						{
							Quizzes = result;
							Quizzes.Reverse();
						}
					}
					var responseStudentAnswersQustions = await client.GetAsync($"{App.UrlAPI}/StudentAnswersQustions/{QuizListID}");
					if (responseStudentAnswersQustions.IsSuccessStatusCode)
					{
						var result = await responseStudentAnswersQustions.Content.ReadAsAsync<List<StudentAnswersQustions>>();
						if (result != null)
						{
							StudentQustion = result.SelectMany(c=>c.StudentQustion).ToList();
						}
					}
				}
				GenerateChart(ChartType);
			}
			catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "ResultByChartPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
		}
		public enum ChartTypeEnum
		{
			Bar,
			Pie
		}
		public void GenerateChart (ChartTypeEnum chartType)
		{
			try
			{
				List<ChartEntry> ChartEntry = new List<ChartEntry>();
				var Quiz = Quizzes[QustionIndex];
				TitleQustion = Quiz.Question;
				var studentsQustion = StudentQustion.Where(c => c.QustionId == Quiz.Id).ToList();
				var selectStudentAnswer = studentsQustion.SelectMany(c => c.StudentAnswer);
				var StudentAnswers = selectStudentAnswer.GroupBy(n => n.Answer.Answer).Select(c => new { model = c,  answer = c.Key, total = c.Count() });
				var random = new Random();
				if (StudentAnswers != null)
				{
					CorrectAnswer = string.Join(", ", Quiz.Answers.Where(c => c.IsAnswerTrue).Select(c => c.Answer).ToList());
					foreach (var item in StudentAnswers)
					{
						var color = string.Format("#{0:X6}", random.Next(0x1000000));
						var percentage = (float)Math.Round((item.total * 100d) / selectStudentAnswer.Count());
						ChartEntry.Add(new ChartEntry(percentage)
						{
							Color = SKColor.Parse(color),
							TextColor = SKColor.Parse(color),
							Label = item.answer,
							ValueLabel = $"{percentage}%",
							
						});
					}
				}

				if (chartType == ChartTypeEnum.Bar)
				{
					var ResultChart = new BarChart()
					{
						LabelTextSize = 45,
						LabelOrientation = Orientation.Horizontal,
						Entries = ChartEntry,
						BarAreaAlpha = 0,
						Typeface = SKTypeface.FromFamilyName("Arial")
					};
					microChart.Chart = ResultChart;
				}
				else
				{
					var ResultChart = new DonutChart()
					{
						LabelTextSize = 45,
						HoleRadius = 0.20f,
						Entries = ChartEntry,
						Typeface = SKTypeface.FromFamilyName("Arial"),
					};
					microChart.Chart = ResultChart;
				}

			}
			catch (Exception ex)
			{
				var properties = new Dictionary<string, string> {
					{ "Method", "GenerateChart" },
					{ "Class", "ResultByChartPage.xaml.cs" }
				};
				Crashes.TrackError(ex, properties);
			}
        }
		public int QustionIndex { get; set; }

		private void BackButton(object sender, EventArgs e)
		{
			QustionIndex--;
			GenerateChart(ChartType);
			if (QustionIndex > 0)
			{
				NextBtn.IsVisible = true;
			}
			else
			{
				BackBtn.IsVisible = false;
			}

		}

		private void NextButton(object sender, EventArgs e)
		{
			BackBtn.IsVisible = false;
			QustionIndex++;
			GenerateChart(ChartType);

			if (QustionIndex + 1 < Quizzes.Count)
			{
				BackBtn.IsVisible = true;
				NextBtn.IsVisible = true;
			}
			else if (QustionIndex + 1 == Quizzes.Count)
			{
				BackBtn.IsVisible = true;
				NextBtn.IsVisible = false;
			}

		}
	}
}