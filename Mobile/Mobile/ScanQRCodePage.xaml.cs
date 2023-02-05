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
    public partial class ScanQRCodePage : ContentPage
    {
        public enum TypeSacnEnum
        {
            JoinClass,
            JoinQuiz
        }
        public TypeSacnEnum TypeSacn { get; set; }
        public ScanQRCodePage()
        {
            InitializeComponent();
            BindingContext = this;
        }
		protected override void OnAppearing()
		{
			scanView.Options = new ZXing.Mobile.MobileBarcodeScanningOptions
			{
				DelayBetweenAnalyzingFrames = 5000,
				DelayBetweenContinuousScans = 10000,
				TryHarder = false,
			};
		}
		public async void Handle_OnScanResult(ZXing.Result result)
        {
            try
            {
                var splitResult = result.Text.Split(',');
                var guid = splitResult[0];
                TypeSacn = (TypeSacnEnum)Enum.Parse(typeof(TypeSacnEnum), splitResult[1], true);
                var IsGuid = Guid.TryParse(guid, out Guid ID);
                if (IsGuid)
                {
                    if (ID != Guid.Empty)
                    {
                        Device.BeginInvokeOnMainThread(() => IsBusy = true);
                        var UserId = Preferences.Get("UserID", "");
                        if (TypeSacn == TypeSacnEnum.JoinQuiz)
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                var responseQuizLists = await client.GetAsync($"{App.UrlAPI}/QuizLists/{ID}");
                                if (responseQuizLists.StatusCode != System.Net.HttpStatusCode.NotFound)
                                {
                                    var resultQuizLists = await responseQuizLists.Content.ReadAsAsync<QuizListVM>();
                                    if (responseQuizLists.IsSuccessStatusCode)
                                    {
                                        var responseCheck = await client.GetAsync($"{App.UrlAPI}/Classes/CheckUsersClass/{UserId}/{resultQuizLists.ClassesId}");
                                        if (responseCheck.StatusCode != System.Net.HttpStatusCode.NotFound)
                                        {
                                            if (responseCheck.IsSuccessStatusCode)
                                            {
                                                var resultCheck = await responseCheck.Content.ReadAsAsync<bool>();
                                                if (resultCheck)
                                                {
                                                    var responseCheckTakeQuiz = await client.GetAsync($"{App.UrlAPI}/StudentAnswersQustions/CheckStudentTakeQuiz/{ID}/{UserId}");
                                                    if (responseCheckTakeQuiz.StatusCode != System.Net.HttpStatusCode.NotFound)
                                                    {
                                                        if (responseCheckTakeQuiz.IsSuccessStatusCode)
                                                        {
                                                            var resultCheckTakeQuiz = await responseCheckTakeQuiz.Content.ReadAsAsync<bool>();
                                                            if (!resultCheckTakeQuiz)
                                                            {
                                                                Device.BeginInvokeOnMainThread(() =>
                                                                {
                                                                    Navigation.PushAsync(new AnswerQustionsPage(resultQuizLists));
                                                                });
                                                            }
                                                            else
                                                            {
                                                                Device.BeginInvokeOnMainThread(() =>
                                                                {
                                                                    DisplayAlert("React App", "You've already taken the quiz. You can't take the quiz again.", "Ok");
                                                                });
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Device.BeginInvokeOnMainThread(async () =>
                                                    {
                                                        var resultRegister = await DisplayAlert("React App", "You can't get this quiz because you're not joind in the class, Join in the class and rescan quiz after joined.", "Join in the Class", "Cancel");
                                                        if (resultRegister)
                                                        {
                                                            Navigation.RemovePage(this);
                                                            await Navigation.PushAsync(new ScanQRCodePage());
                                                        }
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    DisplayAlert("React App", "Sorry, can't get this quiz because something error.", "Ok");
                                                });
                                            }
                                        }
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                DisplayAlert("React App", "Sorry, something went wrong.", "Ok");
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        DisplayAlert("React App", "Sorry, this quiz not found.", "Ok");
                                    });
                                }
                            }

                        }
                        else
                        {
                            using (HttpClient client = new HttpClient())
                            {

                                var responseAdd = await client.GetAsync($"{App.UrlAPI}/Classes/AddUsersClass/{UserId}/{ID}");
                                if (responseAdd.StatusCode != System.Net.HttpStatusCode.NotFound)
                                {
                                    if (responseAdd.IsSuccessStatusCode)
                                    {
                                        var resultAdd = await responseAdd.Content.ReadAsAsync<bool>();
                                        if (resultAdd)
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                DisplayAlert("React App", "You have been added to the class successfully", "Ok");
                                            });
                                        }
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            await DisplayAlert("React App", await responseAdd.Content.ReadAsStringAsync(), "Ok");
                                        });
                                    }

                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        DisplayAlert("React App", "Sorry, this class not found.", "Ok");
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => DisplayAlert("React App", $"invalid id please scan correct {(TypeSacn == TypeSacnEnum.JoinClass ? "class" : "quiz")} id", "OK"));
                }

            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "Handle_OnScanResult" },
                    { "Class", "ScanQRCodePage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
            Device.BeginInvokeOnMainThread(() => IsBusy = false);
        }

        private async void SubmitCode(object sender, EventArgs e)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() => IsBusy = true);
                var value = EntryCode.Text;
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                var UserId = Preferences.Get("UserID", "");
                using (HttpClient client = new HttpClient())
                {

                    var responseQuizLists = await client.GetAsync($"{App.UrlAPI}/QuizLists/GetQuizListByCode/{value}");
                    if (responseQuizLists.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        var resultQuizLists = await responseQuizLists.Content.ReadAsAsync<QuizListVM>();
                        if (responseQuizLists.IsSuccessStatusCode)
                        {
                            var responseCheck = await client.GetAsync($"{App.UrlAPI}/Classes/CheckUsersClass/{UserId}/{resultQuizLists.ClassesId}");
                            if (responseCheck.StatusCode != System.Net.HttpStatusCode.NotFound)
                            {
                                if (responseCheck.IsSuccessStatusCode)
                                {
                                    var resultCheck = await responseCheck.Content.ReadAsAsync<bool>();
                                    if (resultCheck)
                                    {
                                        var responseCheckTakeQuiz = await client.GetAsync($"{App.UrlAPI}/StudentAnswersQustions/CheckStudentTakeQuiz/{resultQuizLists.Id}/{UserId}");
                                        if (responseCheckTakeQuiz.StatusCode != System.Net.HttpStatusCode.NotFound)
                                        {
                                            if (responseCheckTakeQuiz.IsSuccessStatusCode)
                                            {
                                                var resultCheckTakeQuiz = await responseCheckTakeQuiz.Content.ReadAsAsync<bool>();
                                                if (!resultCheckTakeQuiz)
                                                {
                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        Navigation.PushAsync(new AnswerQustionsPage(resultQuizLists));
                                                    });
                                                }
                                                else
                                                {
                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        DisplayAlert("React App", "You've already taken the quiz. You can't take the quiz again.", "Ok");
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            var resultRegister = await DisplayAlert("React App", "You can't get this quiz because you're not joind in the class, Join in the class and rescan quiz after joined.", "Join in the Class", "Cancel");
                                            if (resultRegister)
                                            {
                                                Navigation.RemovePage(this);
                                                await Navigation.PushAsync(new ScanQRCodePage());
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        DisplayAlert("React App", "Sorry, can't get this quiz because something error.", "Ok");
                                    });
                                }
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("React App", "Sorry, something went wrong.", "Ok");
                                });
                            }
                        }
                    }
                    else
                    {
                        var responseAdd = await client.GetAsync($"{App.UrlAPI}/Classes/AddUsersClassByCode/{UserId}/{value}");
                        if (responseAdd.StatusCode != System.Net.HttpStatusCode.NotFound)
                        {
                            if (responseAdd.IsSuccessStatusCode)
                            {
                                var resultAdd = await responseAdd.Content.ReadAsAsync<bool>();
                                if (resultAdd)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        DisplayAlert("React App", "You have been added to the class successfully", "Ok");
                                        EntryCode.Text = string.Empty;
                                    });
                                }
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await DisplayAlert("React App", await responseAdd.Content.ReadAsStringAsync(), "Ok");
                                });
                            }

                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DisplayAlert("React App", "Sorry, Not found class or quiz.", "Ok");
                            });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "SubmitCode" },
                    { "Class", "ScanQRCodePage.xaml.cs" }
                };
                Crashes.TrackError(ex, properties);
            }
            Device.BeginInvokeOnMainThread(() => IsBusy = false);
        }
	}
}