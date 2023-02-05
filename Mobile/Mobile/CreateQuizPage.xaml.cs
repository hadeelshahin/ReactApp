using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using ModelsShared.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateQuizPage : ContentPage
    {

        private TimeSpan fromTime;
        public TimeSpan FromTime
        {
            get { return fromTime; }
            set { fromTime = value; OnPropertyChanged(); }
        }

        private TimeSpan toTime;
        public TimeSpan ToTime
        {
            get { return toTime; }
            set { toTime = value; OnPropertyChanged(); }
        }

        private string titleQuiz;
        public string TitleQuiz
        {
            get { return titleQuiz; }
            set { titleQuiz = value; OnPropertyChanged(); }
        }

        private string subjectQuiz;
        public string SubjectQuiz
        {
            get { return subjectQuiz; }
            set { subjectQuiz = value; OnPropertyChanged(); }
        }

        private bool isMarker = true;
        public bool IsMarker
        {
            get { return isMarker; }
            set { isMarker = value; OnPropertyChanged(); }
        }

        private int numberOfStudent;
        public int NumberOfStudent
        {
            get { return numberOfStudent; }
            set { numberOfStudent = value; OnPropertyChanged(); }
        }


        public QuizType QuizType { get; set; }
        public QuizTimeType TimeType { get; set; }
        public Guid ClassId { get; set; }

        private List<int> minutes;
        public List<int> Minutes
        {
            get { return minutes; }
            set { minutes = value; OnPropertyChanged(); }
        }


        public CreateQuizPage (Guid classId, QuizType createType)
		{
            try
            {
                InitializeComponent();
                ClassId = classId;
                QuizType = createType;

                switch (createType)
                {
                    case QuizType.Normal:
                        break;
                    case QuizType.PresenceAndAbsence:
                        TitleQuiz = "Presence & Absence";
                        IsMarker = false;
                        MarkerLayout.IsVisible = false;
                        break;
                }
                TimeType = QuizTimeType.Interval;
                Minutes = new List<int>();
                for (int i = 1; i <= 30; i++)
                {
                    Minutes.Add(i);
                }
                BindingContext = this;
                intervalPicker.ItemsSource = Minutes;
                intervalPicker.SelectedIndexChanged += Interval_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "CreateQuizPage" },
                    { "Class", "CreateQuizPage.xaml.cs" }
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
                    var response = await client.GetAsync($"{App.UrlAPI}/Classes/GetCountOfStudents/{ClassId}");
                    var result = await response.Content.ReadAsAsync<int>();
                    if (response.IsSuccessStatusCode)
                    {
                        NumberOfStudent = result;
                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "CreateQuizPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }


        private void SelectInterval(object sender, EventArgs e)
        {
            try
            {
                IntervalButton.TextColor = Color.White;
			    FromToButton.TextColor = Color.FromHex("#A2A2A2");
			    UnlimitedButton.TextColor = Color.FromHex("#A2A2A2");
                TimeType = QuizTimeType.Interval;
                QuizTimeLayout.Children.Clear();
                var StartColor = (Color)Application.Current.Resources["ButtonStartColor"];
                var EndColor = (Color)Application.Current.Resources["ButtonEndColor"];
                var frame = new Frame
                {
                    Padding = 0,
                    Margin = 0,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BorderColor = Color.Transparent,
                    WidthRequest = 90,
                    Background = new LinearGradientBrush
                    {
                        EndPoint= new Point(0, 1),
                        GradientStops =
                        {
                            new GradientStop(StartColor, 0.1f),
                            new GradientStop(EndColor, 1.0f ),
                        }
                    }
                };

                var interval = new Picker
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = Color.White,
                    TitleColor = Color.White,
                    ItemsSource = Minutes,
                    ItemDisplayBinding = new Binding("."),
                    Title = "Minutes"
                };
                interval.SelectedIndexChanged += Interval_SelectedIndexChanged;
                frame.Content = interval;
                QuizTimeLayout.Children.Add(frame);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "SelectInterval" },
                    { "Class", "CreateQuizPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }

        private void Interval_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((Picker)sender).SelectedItem.ToString();
            TimeSpan duration = TimeSpan.Parse($"00:{value}:00", CultureInfo.InvariantCulture);
            FromTime = duration;
        }

        private void SelectFromTo(object sender, EventArgs e)
        {
            try
            {
                IntervalButton.TextColor = Color.FromHex("#A2A2A2");
                FromToButton.TextColor = Color.White;
                UnlimitedButton.TextColor = Color.FromHex("#A2A2A2");
                TimeType = QuizTimeType.FromTo;
                QuizTimeLayout.Children.Clear();
                var StartColor = (Color)Application.Current.Resources["ButtonStartColor"];
                var EndColor = (Color)Application.Current.Resources["ButtonEndColor"];
                var frameFrom = new Frame
                {
                    Padding = 0,
                    Margin = 0,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BorderColor = Color.Transparent,
                    WidthRequest = 90,
                    Background = new LinearGradientBrush
                    {
                        EndPoint= new Point(0, 1),
                        GradientStops =
                        {
                            new GradientStop(StartColor, 0.1f),
                            new GradientStop(EndColor, 1.0f ),
                        }
                    }
                };
                var frameTo = new Frame
                {
                    Padding = 0,
                    Margin = 0,
                    CornerRadius = 10,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BorderColor = Color.Transparent,
                    WidthRequest = 90,
                    Background = new LinearGradientBrush
                    {
                        EndPoint = new Point(0, 1),
                        GradientStops =
                        {
                            new GradientStop(StartColor, 0.1f),
                            new GradientStop(EndColor, 1.0f ),
                        }
                    }
                };

                var FromTime = new TimePicker
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = Color.White,

                };
                var ToTime = new TimePicker
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = Color.White,

                };
                FromTime.SetBinding(TimePicker.TimeProperty, "FromTime");
                ToTime.SetBinding(TimePicker.TimeProperty, "ToTime");

                frameFrom.Content = FromTime;
                frameTo.Content = ToTime;
                QuizTimeLayout.Children.Add(frameFrom);
                QuizTimeLayout.Children.Add(frameTo);
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "SelectFromTo" },
                    { "Class", "CreateQuizPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }

        private void SelectUnlimited(object sender, EventArgs e)
        {
            IntervalButton.TextColor = Color.FromHex("#A2A2A2");
            FromToButton.TextColor = Color.FromHex("#A2A2A2");
            UnlimitedButton.TextColor = Color.White;
            TimeType = QuizTimeType.Unlimited;
            QuizTimeLayout.Children.Clear();
        }

        private async void CreateQuiz(object sender, EventArgs e)
        {
            try
            {
                var model = new QuizListVM
                {
                    ClassesId = ClassId,
                    IsMarker = IsMarker,
                    QuizTimeType = TimeType,
                    Subject = SubjectQuiz,
                    Title = TitleQuiz,
                    FromTime = new DateTime() + FromTime,
                    ToTime = new DateTime() + ToTime,
                    QuizType = QuizType,
                };
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync($"{App.UrlAPI}/QuizLists", model);
                    var result = await response.Content.ReadAsAsync<QuizListVM>();
                    if (response.IsSuccessStatusCode)
                    {
                        await Navigation.PushAsync(new CreateQustionsPage(result.Id, result.IsMarker));
                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "CreateQuiz" },
                    { "Class", "CreateQuizPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }

        }
    }
}