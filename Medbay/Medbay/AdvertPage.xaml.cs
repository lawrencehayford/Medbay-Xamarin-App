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
	public partial class AdvertPage : ContentPage
	{
		public AdvertPage()
		{
			InitializeComponent ();
		}

        private async Task GoToHome(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
           
        }

        private async Task OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            IsBusy = false;
            await Task.Delay(100);
            stackhide.IsVisible = false;
        }
    }
}