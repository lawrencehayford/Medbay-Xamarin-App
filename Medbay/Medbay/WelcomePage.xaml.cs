using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Medbay
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			InitializeComponent ();
		}

        private async Task Login(object sender, EventArgs e)
        {
           
            login1.Text = "Please wait..";
            login1.TextColor = Color.White;
            login1.IsEnabled = false;
            await Task.Delay(100);
            await Navigation.PushModalAsync(new LoginPage());
        }
        protected override bool OnBackButtonPressed()
        {
            bool _canClose = true;
            ShowExitDialog();

            return _canClose;
        }
        private async void ShowExitDialog()
        {

            var answer = await DisplayAlert("Exit", "Do you wan't to exit the App?", "Yes", "No");
            if (answer)
            {
              
                Thread.CurrentThread.Abort();
            }

        }
    }
}