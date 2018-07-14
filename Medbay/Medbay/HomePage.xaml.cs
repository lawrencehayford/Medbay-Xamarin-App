using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Medbay.usedclasses;
using System.Threading;

namespace Medbay
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        string Day;
        SessionStorage SessionObj = new SessionStorage();
        public HomePage()
        {
            InitializeComponent();
            if (DateTime.Now.Hour < 12)
            {
                Day = "\nGood Morning";
            }
            else if (DateTime.Now.Hour < 17)
            {
                Day = "\nGood Afternoon";
            }
            else
            {
                Day = "\nGood Evening";
            }

            string Details = Day + "\n" +
              SessionObj.GetItem("name") + "\n" +
               "MEDBAY PREFERRED CLIENT " + "\n" +
              "Last login :" + DateTime.Now + "\n";

            Myname.Text = Details;
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
                SessionObj.ClearAllItem();
                Thread.CurrentThread.Abort();
            }

        }

        async void ShopDrug(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ShopDrugPage());
        }

        async void PostAd(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PostAdPage());
        }

        async void PostNeed(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PostNeedPage());
        }

        async void Logout(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Exit", "Do you wan't to exit the App?", "Yes", "No");
            if (answer)
            {
                SessionObj.ClearAllItem();
                Thread.CurrentThread.Abort();
            }
        }

        async void ShopHerbal(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ShopHerbalPage());
        }

        async void Settings(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingsPage());
        }

        async void ShopSundries(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ShopSundriesPage());
        }
		
		async void ShopDevices(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ShopDevicesPage());
        }

        async void ShowAdvert(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AdvertPage());
        }

        async void ShowDeal(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DealsPage());
        }


    }
}