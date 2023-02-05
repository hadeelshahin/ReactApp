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
    public partial class CreateQustionsPage : ContentPage
    {
        public List<Option> QustionAnswers { get; set; }
        public List<Qustion> Qustions { get; set; }

        public class Qustion
        {
            public bool IsMarker { get; set; }
            public string Text { get; set; }
            public List<Option> Options { get; set; }
        }
        public class Option
        {
            public Entry Answer { get; set; }
            public bool Selected { get; set; }

        }
        private bool isMarker;

        public bool IsMarker
        {
            get { return isMarker; }
            set { isMarker = value; OnPropertyChanged(); }
        }
        public Guid QuizListId { get; set; }
        public CreateQustionsPage()
        {
            InitializeComponent();
            BindingContext = this;
            QustionAnswers = new List<Option>();
            Qustions = new List<Qustion>();
            AddOptionLayout(false);
            AddOptionLayout(false);
        }
        public CreateQustionsPage(Guid quizListId, bool isMarker)
        {
            InitializeComponent();
            QustionAnswers = new List<Option>();
            Qustions = new List<Qustion>();
            QuizListId = quizListId;
            IsMarker = isMarker;
            BindingContext = this;
            AddOptionLayout(false);
            AddOptionLayout(false);
        }

        private void SelectTrueOption(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var stackOption = (StackLayout)button.Parent.Parent ?? (StackLayout)button.Parent;
            var index = OptionsLayout.Children.IndexOf(stackOption);
            var item = QustionAnswers[index];
            if (!item.Selected)
            {
                button.ImageSource = "select.png";
                item.Selected = true;
            }
            else
            {
                button.ImageSource = "unselect.png";
                item.Selected = false;
            }
        }

        private void RemoveOption(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var stackOption = (StackLayout)button.Parent.Parent;
            var index = OptionsLayout.Children.IndexOf(stackOption);
            QustionAnswers.Remove(QustionAnswers[index]);
            OptionsLayout.Children.Remove(stackOption);
        }

        private void AddLayout(object sender, FocusEventArgs e)
        {
            addEntryOption.Unfocus();
            AddOptionLayout(true).Focus();
        }

        public Entry AddOptionLayout(bool withRemoveBtn)
        {
            var OptionEntry = new Entry
            {
                Placeholder = $"Option {OptionsLayout.Children.Count + 1}",
                PlaceholderColor = Color.White,
                TextColor = Color.White,
                WidthRequest = 230,
            };
            try
            {
                var selectedOption = new Option();
                var RemoveButton = new Button
                {
                    ImageSource = "remove.png",
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Color.Transparent,
                    CornerRadius = 25,
                    WidthRequest = 43,
                    HeightRequest = 43,
                    ClassId = "remove"
                };
                RemoveButton.Clicked += RemoveOption;
                var SelectButton = new Button
                {
                    ImageSource = "unselect.png",
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    BackgroundColor = Color.Transparent,
                    CornerRadius = 25,
                    WidthRequest = 43,
                    HeightRequest = 43,
                    IsVisible = false,
                    ClassId = "select"
                };
                SelectButton.Clicked += SelectTrueOption;
                selectedOption.Answer = OptionEntry;

                var Control = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                };

                if (withRemoveBtn)
                {
                    Control.Children.Add(RemoveButton);
                }
                if (IsMarker)
                {
                    SelectButton.IsVisible = true;
                }
                Control.Children.Add(SelectButton);
                QustionAnswers.Add(selectedOption);

                OptionsLayout.Children.Add(new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        OptionEntry,
                        Control
                    }
                });
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "AddOptionLayout" },
                    { "Class", "CreateQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
            return OptionEntry;
        }

        private async void CreateQustion(object sender, EventArgs e)
        {
            try
            {
                if (await DisplayAlert("React App", "Are you sure you're done writing questions?", "Yes", "No"))
                {
                    var qustion = new Qustion { Text = QustionEditor.Text };
                    qustion.Options = new List<Option>();
                    qustion.Options.AddRange(QustionAnswers);
                    Qustions.Add(qustion);
                    List<QuizVM> quizVM = new List<QuizVM>();

                    foreach (var item in Qustions)
                    {
                        var quizId = Guid.NewGuid();
                        List<AnswersVM> answersVM = new List<AnswersVM>();
                        foreach (var option in item.Options)
                        {
                            answersVM.Add(new AnswersVM { Answer = option.Answer.Text, IsAnswerTrue = option.Selected, QuizId = quizId });
                        }
                        quizVM.Add(new QuizVM { Id = quizId, Question = item.Text, QuizListId = QuizListId, Answers = answersVM });
                    }
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.PostAsJsonAsync($"{App.UrlAPI}/Quizs", quizVM);
                        var model = await response.Content.ReadAsAsync<QuizListVM>();
                        if (response.IsSuccessStatusCode)
                        {
                            await Navigation.PushAsync(new GenerateQRCodePage(model.Id, model.IdCode, GenerateQRCodePage.TypeSacnEnum.JoinQuiz));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "CreateQustion" },
                    { "Class", "CreateQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }

        private async void NextQustion(object sender, EventArgs e)
        {
            try
            {
                if (await DisplayAlert("React App", "Are you sure about the input of the current question to move to create the next question?", "Yes", "No"))
                {
                    var qustion = new Qustion { Text = QustionEditor.Text };
                    qustion.Options = new List<Option>();
                    qustion.Options.AddRange(QustionAnswers);
                    Qustions.Add(qustion);
                    SwitchMarker.IsEnabled = false;
                    QustionAnswers.Clear();
                    QustionEditor.Text = string.Empty;
                    OptionsLayout.Children.Clear();
                    AddOptionLayout(false);
                    AddOptionLayout(false);
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "NextQustion" },
                    { "Class", "CreateQustionsPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }

        }

        private void ChangeMarker(object sender, ToggledEventArgs e)
        {
            foreach (StackLayout item in OptionsLayout.Children)
            {
                foreach (var layout in item.Children)
                {
                    if (layout.GetType() == typeof(StackLayout))
                    {
                        foreach (Button btn in ((StackLayout)layout).Children)
                        {
                            if (btn.ClassId == "select")
                            {
                                if (!e.Value)
                                {
                                    btn.IsVisible = false;
                                }
                                else
                                {
                                    btn.IsVisible = true;
                                }
                            }
                        }
                    }

                }
            }
            foreach (var item in Qustions)
            {
                item.IsMarker = e.Value;
            }
        }
    }
}