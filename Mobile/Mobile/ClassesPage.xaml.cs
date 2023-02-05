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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;

namespace Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClassesPage : ContentPage
    {
        private ObservableCollection<ClassesVM> classes;
        public ObservableCollection<ClassesVM> Classes
        {
            get { return classes; }
            set { classes = value; OnPropertyChanged(); }
        }

        public ClassesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        private bool isClassesNotExist = true;

        public bool IsClassesNotExist
        {
            get { return isClassesNotExist; }
            set { isClassesNotExist = value; OnPropertyChanged(); }
        }

        protected override async void OnAppearing()
        {
            try
            {
                IsBusy = true;
                var UserId = Preferences.Get("UserID", "");
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync($"{App.UrlAPI}/Classes/GetClassesById/{UserId}");
                    var result = await response.Content.ReadAsAsync<List<ClassesVM>>();
                    if (response.IsSuccessStatusCode)
                    {
                        Classes = new ObservableCollection<ClassesVM>(result);
                    }
                }
                Device.BeginInvokeOnMainThread(() => 
                {
                    if (Classes.Count > 0)
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
                    { "Class", "ClassesPage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
        }

        private void CreateClassesPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateClassesPage());
        }

        private void ShowStudentClicked(object sender, EventArgs e)
        {
            var stringInThisCell = (ClassesVM)((Button)sender).CommandParameter;
			Navigation.PushAsync(new ShowStudentPage(stringInThisCell.Id));
		}

		private void GenerateQRCode(object sender, EventArgs e)
        {
            var item = (ClassesVM)((Xamarin.Forms.ImageButton)sender).CommandParameter;
            Navigation.PushAsync(new GenerateQRCodePage(item.Id,item.IdCode, GenerateQRCodePage.TypeSacnEnum.JoinClass));
        }

        private void ClassSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ClassId = ((ClassesVM)e.SelectedItem).Id;
            Navigation.PushAsync(new QuizListPage(ClassId));
        }
    }
}