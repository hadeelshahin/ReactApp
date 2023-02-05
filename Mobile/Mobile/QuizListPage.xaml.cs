using Microsoft.AppCenter.Crashes;
using ModelsShared.Models;
using ModelsShared.ViewModels;
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
	public partial class QuizListPage : ContentPage
	{
        private ObservableCollection<QuizListVM> quizList;
        public ObservableCollection<QuizListVM> QuizList
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

        public Guid ClassId { get; set; }
        public QuizListPage (Guid classId)
		{
			InitializeComponent ();
            ClassId = classId;
            BindingContext = this;
		}

        protected override async void OnAppearing()
        {
            try
            {
                IsBusy = true;
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync($"{App.UrlAPI}/QuizLists/GetQuizListsByClassId/{ClassId}");
                    var result = await response.Content.ReadAsAsync<List<QuizListVM>>();
                    if (response.IsSuccessStatusCode)
                    {
                        QuizList = new ObservableCollection<QuizListVM>(result);
                    }
                }
                if (quizList.Count > 0)
                {
                    IsClassesNotExist = false;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "OnAppearing" },
                    { "Class", "QuizListPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }

        private bool isCreateQuiz = false;
		public bool IsCreateQuiz
        {
			get { return isCreateQuiz; }
			set { isCreateQuiz = value; OnPropertyChanged(); }
		}

		private void OpenOptionsCreateQuiz(object sender, EventArgs e)
        {
            IsCreateQuiz = true;
        }

        private void CloseOptionsCreateQuiz(object sender, EventArgs e)
        {
            IsCreateQuiz = false;
        }

        private void CreateQuiz(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateQuizPage(ClassId, QuizType.Normal));
            IsCreateQuiz = false;
        }

        private void CreatePresenceAndAbsence(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateQuizPage(ClassId, QuizType.PresenceAndAbsence));
            IsCreateQuiz = false;
        }

        private void QuizSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var QuizList = ((QuizListVM)e.SelectedItem);
            Navigation.PushAsync(new ResultQuizPage(QuizList));
        }

        private void GenerateQRCode(object sender, EventArgs e)
        {
            var model = (QuizListVM)((ImageButton)sender).CommandParameter;
            Navigation.PushAsync(new GenerateQRCodePage(model.Id, model.IdCode, GenerateQRCodePage.TypeSacnEnum.JoinQuiz));
		}
	}
}