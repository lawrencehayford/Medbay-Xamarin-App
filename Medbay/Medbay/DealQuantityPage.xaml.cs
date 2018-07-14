using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Medbay.usedclasses;

using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Medbay
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class DealQuantityPage : ContentPage
    {

        SessionStorage SessionObj = new SessionStorage();
        public DealQuantityPage()
        {
            InitializeComponent();
           
        }

        private async Task GoToHome(object sender, EventArgs e)
        {
           
                await Navigation.PopModalAsync();
           
        }

        private async Task Confirm(object sender, EventArgs e)
        {

            SessionObj.PostItem("returnQuantity", quantity.Text.Trim());
            //tell the parent page data has returned
            SessionStorage.MAINPOPUP = 1; 
            await this.Navigation.PopModalAsync();


        }

        protected override bool OnBackButtonPressed()
        {
            bool _canClose = true;
            ShowExitDialog();

            return _canClose;
        }
        private async void ShowExitDialog()
        {

            var answer = await DisplayAlert("Warning", "Are you sure you want to close this? \n You will lose all your changes made if you proceed", "Yes", "No");
            if (answer)
            {
                await Navigation.PopModalAsync();
            }

        }




        protected override void OnAppearing()
        {
            System.Diagnostics.Debug.WriteLine("Main Popup= " + SessionStorage.MAINPOPUP);

            if (SessionStorage.MAINPOPUP == 1)
            {
                System.Diagnostics.Debug.WriteLine("popping up main= ");

                SessionStorage.MAINPOPUP = 0;
                this.Navigation.PopModalAsync();

            }

        }


     

    }
}