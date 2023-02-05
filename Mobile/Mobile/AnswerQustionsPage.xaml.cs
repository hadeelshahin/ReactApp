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
    public partial class AnswerQustionsPage : ContentPage
    {
        public List<QuizVM> Qustions { get; set; }
        public StudentAnswersQustions StudentAnswerQustion { get; set; }
        public List<StudentQustion> StudentQustions { get; set; }
        public List<StudentAnswer> StudentAnswers { get; set; }
        public QuizListVM QuizList { get; set; }
        public AnswerQustionsPage(QuizListVM quizList)
        {
			try
			{
                QuizList = quizList;
                InitializeComponent();
                TitleQuiz.Text = quizList.Title;
                var UserId = Preferences.Get("UserID", "");

                StudentAnswerQustion = new StudentAnswersQustions
                {
                    QuizListId = QuizList.Id,
                    UserId = Guid.Parse(UserId),
                };
                StudentQustions = new List<StudentQustion>();
                StudentAnswers = new List<StudentAnswer>();
                if (quizList.QuizTimeType == QuizTimeType.Interval)
                {
                    var timespan = quizList.FromTime.TimeOfDay;
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        timespan = timespan.Subtract(TimeSpan.FromSeconds(1));

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Timer.Text = timespan.ToString(@"mm\:ss");
                        });
                        return Timer.Text != TimeSpan.Zero.ToString(@"mm\:ss");
                    });
                }
                else if (quizList.QuizTimeType == QuizTimeType.FromTo)
                {
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        var FromTime = quizList.FromTime.ToString(@"mm\:ss tt");
                        var ToTime = quizList.ToTime;
                        TimeSpan duration = DateTime.Parse(ToTime.ToString(@"mm\:ss tt")).Subtract(DateTime.Parse(FromTime));
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Timer.Text = duration.ToString(@"mm\:ss");
                        });
                        return Timer.Text != TimeSpan.Zero.ToString(@"mm\:ss");
                    });
                }
                else
                {
                    Timer.IsVisible = false;
                }

            }
            catch (Exception ex)
			{
                var properties = new Dictionary<string, string> {
                    { "Method", "AnswerQustionsPage" },
                    { "Class", "AnswerQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }
        protected override async void OnAppearing()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync($"{App.UrlAPI}/Quizs/{QuizList.Id}");
                    var result = await response.Content.ReadAsAsync<List<QuizVM>>();
                    if (response.IsSuccessStatusCode)
                    {
                        Qustions = result;
                        Qustions.Reverse();
                    }
                }
                ChangeQustions();
                if (Qustions.Count > 1)
                {
                    NextBtn.IsVisible = true;
                }
                else
                {
                    FinishBtn.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "AnswerQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }

		public int QustionIndex { get; set; }
        public void ChangeQustions()
        {
            try
            {
                var qustion = Qustions[QustionIndex];
                var StartColor = (Color)Application.Current.Resources["ButtonStartColor"];
                var EndColor = (Color)Application.Current.Resources["ButtonEndColor"];
                QustionText.Text = qustion.Question;
                if (!StudentQustions.Any(c => c.StudentAnswersQustionsId == qustion.Id))
                {
                    StudentQustions.Add(new StudentQustion { QustionId = qustion.Id });
                }
                List<View> OptionsList = new List<View>();
                AnswerOption.Children.Clear();
                foreach (var item in qustion.Answers)
                {
                    var OptionBtn = new Button();
                    OptionBtn = new Button
                    {
                        Text = item.Answer,
                        TextColor = Color.White,
                        WidthRequest = 200,
                        CornerRadius = 30,
                        BorderWidth = 1,
                        BorderColor = StudentAnswers.Any(c => c.AnswerId == item.Id) ? Color.White : Color.Default,
                        Background = new LinearGradientBrush
                        {
                            EndPoint = new Point(0, 1),
                            GradientStops =
                            {
                                new GradientStop(StartColor, 0.1f),
                                new GradientStop(EndColor, 1.0f ),
                            }
                        },
                        Command = new Command(() =>
                        {
                            if (qustion.Answers.Where(c => c.IsAnswerTrue).Count() > 1)
                            {
                                if (OptionBtn.BorderColor != Color.White)
                                {
                                    OptionBtn.BorderColor = Color.White;
                                    StudentAnswers.Add(new StudentAnswer { AnswerId = item.Id, StudentQustionId = qustion.Id });
                                }
                                else
                                {
                                    OptionBtn.BorderColor = Color.Default;
                                    StudentAnswers.RemoveAll(c => c.AnswerId == item.Id);
                                }
                            }
                            else
                            {
                                foreach (Button item1 in OptionsList)
                                {
                                    item1.BorderColor = Color.Default;
                                }
                                OptionBtn.BorderColor = Color.White;
                                StudentAnswers.RemoveAll(c => c.StudentQustionId == qustion.Id);
                                StudentAnswers.Add(new StudentAnswer { AnswerId = item.Id, StudentQustionId = qustion.Id });
                            }
                        })

                    };
                    OptionsList.Add(OptionBtn);
                    AnswerOption.Children.Add(OptionBtn);
                }
            }
            catch (Exception ex)
            { 
                var properties = new Dictionary<string, string> 
                {
                    { "Method", "ChangeQustions" },
                    { "Class", "AnswerQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
		}

		private void BackButton(object sender, EventArgs e)
		{
            QustionIndex--;
            ChangeQustions();
            if (QustionIndex > 0)
            {
				NextBtn.IsVisible = true;
				FinishBtn.IsVisible = false;
			}
			else
            {
                BackBtn.IsVisible = false;
				FinishBtn.IsVisible = false;
                NextBtn.IsVisible = true;
            }
        }

		private void NextButton(object sender, EventArgs e)
		{
			BackBtn.IsVisible = false;
			FinishBtn.IsVisible = false;
			QustionIndex++;
            ChangeQustions();

            if (QustionIndex + 1 < Qustions.Count)
			{
				BackBtn.IsVisible = true;
				NextBtn.IsVisible = true;
				FinishBtn.IsVisible = false;
			}
			else if (QustionIndex + 1 == Qustions.Count)
			{
				BackBtn.IsVisible = true;
                NextBtn.IsVisible = false;
				FinishBtn.IsVisible = true;
			}
		}
		private void FinishButton(object sender, EventArgs e)
		{
			PostQuizAnswer();
		}
        public async void PostQuizAnswer()
        {
            try
            {
                if (await DisplayAlert("React App", "Are you sure about this step?", "Yes", "No"))
                {
                    var answerSudentList = new List<AnswersVM>();
                    foreach (var item in StudentQustions)
                    {
                        item.StudentAnswer = new List<StudentAnswer>();

                        foreach (var answer in StudentAnswers.Where(c => c.StudentQustionId == item.QustionId).ToList())
                        {
							answerSudentList.Add(Qustions.SelectMany(c => c.Answers).Where(c => c.Id == answer.AnswerId).SingleOrDefault());

							item.StudentAnswer.Add(answer);
                        }
                    }
                    if (QuizList.IsMarker)
                    {
                        var answers = answerSudentList.ToList();
                        var quizAnswers = Qustions.SelectMany(c => c.Answers).ToList();
						if (answers != null && quizAnswers != null)
                        {
                            var QuizAnswersCount = quizAnswers.Where(c => c.IsAnswerTrue).Count();
                            StudentAnswerQustion.Result = QuizAnswersCount > 0 ? (answers.Where(c => c.IsAnswerTrue).Count() / QuizAnswersCount) * 100 : 0;
                        }
					}
					StudentAnswerQustion.StudentQustion = StudentQustions;
                    StudentAnswerQustion.QuizList = new QuizList { ClassesId = QuizList.ClassesId };
                    StudentAnswerQustion.QuizListId = QuizList.Id;
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.PostAsJsonAsync($"{App.UrlAPI}/StudentAnswersQustions", StudentAnswerQustion);
                        var result = await response.Content.ReadAsAsync<StudentAnswersQustions>();
                        if (response.IsSuccessStatusCode)
                        {
                            await Navigation.PushAsync(new FinishQustionPage(QuizList));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "PostQuizAnswer" },
                    { "Class", "AnswerQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }

        }
		private void CheckEndTime(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
            try
            {
                var timer = (Label)sender;
			    if (QuizList.QuizTimeType == QuizTimeType.Interval)
			    {
                    if (timer.Text == TimeSpan.Zero.ToString(@"mm\:ss"))
                    {
						PostQuizAnswer();
						Navigation.PushAsync(new FinishQustionPage(QuizList));
                        Navigation.RemovePage(this);
                    }
			    }
			    else if (QuizList.QuizTimeType == QuizTimeType.FromTo)
			    {
				    if (timer.Text == TimeSpan.Zero.ToString(@"mm\:ss"))
				    {
						PostQuizAnswer();
						Navigation.PushAsync(new FinishQustionPage(QuizList));
					    Navigation.RemovePage(this);
				    }
			    }
			    else
			    {
			    }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "PostQuizAnswer" },
                    { "Class", "AnswerQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }

	}
}