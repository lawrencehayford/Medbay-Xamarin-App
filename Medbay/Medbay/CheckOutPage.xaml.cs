using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Medbay.usedclasses;
using Newtonsoft.Json.Linq;
using System.Globalization;


namespace Medbay
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckOutPage : ContentPage
	{
        SessionStorage SessionObj = new SessionStorage();
        JObject obj;
        string PayType;
        public CheckOutPage ()
		{
			InitializeComponent ();
		}
        private async Task GoToHome(object sender, EventArgs e)
        {
            await this.Navigation.PopModalAsync();
        }

        private void Highlight(object sender, EventArgs e)
        {
            h.BackgroundColor = Color.AntiqueWhite;
            h1.BackgroundColor = Color.White;
            hu.Source = "checkbox.png";
            hu1.Source = "uncheck.png";
            PayType = "COD";
        }

        private void Highlight1(object sender, EventArgs e)
        {
            h.BackgroundColor = Color.White;
            h1.BackgroundColor = Color.AntiqueWhite;
            hu.Source = "uncheck.png";
            hu1.Source = "checkbox.png";
            PayType = "CHQ";
        }

        async void Confirm(object sender, EventArgs e)
        {
            try
            {
                if (location.Text.Length < 5)
                {

                    await DisplayAlert("Alert", "Enter a valid delivery address", "OK");
                    return;
                }

                if (PayType.Length < 1)
                {

                    await DisplayAlert("Alert", "Select a payment method", "OK");
                    return;
                }
                BtnConfirm.IsEnabled = false;
                await Task.Delay(100);
                var postData = "email=" + SessionObj.GetItem("email").Trim();
                postData += "&usertoken=" + SessionObj.GetItem("usertoken").Trim();
                postData += "&name=" + SessionObj.GetItem("name");
                postData += "&cart=" + SessionObj.GetItem("ProductList");
                postData += "&total=" + SessionObj.GetItem("Total").Trim();
                postData += "&paytype=" + PayType.ToString();
                postData += "&address=" + location.Text;


                System.Diagnostics.Debug.WriteLine("PostData :" + postData);
                BtnConfirm.IsEnabled = false;


                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "orderpayment", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                obj = JObject.Parse(LoginDetails);

                if (obj["success"].ToString() == "0")
                {
                    //order successful

                    await DisplayAlert("Alert", obj["message"].ToString(), "OK");
                    SessionStorage.MAINPOPUP = 1;
                    await this.Navigation.PopModalAsync();
                    return;


                }
                else
                {
                    BtnConfirm.IsEnabled = true;
                    await Task.Delay(100);
                    await DisplayAlert("Alert", "An error has occurred \n" + LoginDetails.ToString(), "OK");

                    return;
                }


            }
            catch (Exception n)
            {
                BtnConfirm.IsEnabled = true;
                System.Diagnostics.Debug.WriteLine("Exception Caught :" + n.ToString());
                await DisplayAlert("Alert", "An error has occured \n Please try again ", "OK");

                //return;
            }



        }
    }
}