using ModelsShared.Models;
using ModelsShared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FinishQustionPage : ContentPage
	{
		public QuizListVM QuizList { get; set; }
		public FinishQustionPage(QuizListVM quizList)
		{
			QuizList = quizList;
			InitializeComponent();
			BindingContext = this;
		}

		private void GoToHome(object sender, EventArgs e)
		{
			Navigation.PopToRootAsync();
		}
	}
}