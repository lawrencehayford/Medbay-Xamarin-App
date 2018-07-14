using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medbay.usedclasses;
using Newtonsoft.Json.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Medbay
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPasswordPage : ContentPage
	{
        JObject obj;
        Boolean IsSent = false;
        string EmailVal;
        public ForgotPasswordPage ()
		{
			InitializeComponent ();
		}


        private async Task BtnConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (IsSent == true)
                {
                    await ConfirmReset();
                    return;
                }

                if (String.IsNullOrEmpty(email.Text))
                {

                    await DisplayAlert("Alert", "Missing email input required!", "OK");
                    return;
                }
               
                BtnConfirm.Text = "Please wait...";
                BtnConfirm.IsEnabled = false;
                await Task.Delay(1000);

                //fullname,telephone,email,country,password,repassword
                var postData = "email=" + email.Text;

                System.Diagnostics.Debug.WriteLine("PostData" + postData);

                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "getauthcode", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                Console.WriteLine("Reset response" + LoginDetails);
                System.Diagnostics.Debug.WriteLine("Reset response" + LoginDetails);
                obj = JObject.Parse(LoginDetails);

                //registration ok
                if (obj["success"].ToString() == "0")
                {
                    //saving email
                    await Task.Delay(100);
                    //hide first stacks
                    firstentrystack.IsVisible = false;

                    //show second stacks
                    secondentrycodestack.IsVisible = true;
                    secondentrypasswordstack.IsVisible = true;
                    secondentryconfirmpasswordstack.IsVisible = true;
                    BtnConfirm.Text = "   Reset   ";
                    BtnConfirm.IsEnabled = true;
                    IsSent = true;
                    EmailVal = email.Text;

                    await DisplayAlert("Successful", obj["message"].ToString(), "OK");
                }
                //registration failed
                else
                {
                    Whenfail();
                    await Task.Delay(100);

                    await DisplayAlert("Error", obj["message"].ToString(), "OK");
                }
            }
            catch (Exception f)
            {
                Whenfail();
                await Task.Delay(100);
                await DisplayAlert("Error", f.Message, "OK");

            }
        }

        protected async Task ConfirmReset()
        {
            try
            {

                if (String.IsNullOrEmpty(pincode.Text))
                {

                    await DisplayAlert("Alert", "Missing pin input required!", "OK");
                    return;
                }

                if (String.IsNullOrEmpty(newpass.Text))
                {

                    await DisplayAlert("Alert", "Missing password input required!", "OK");
                    return;
                }

                if (confirmnewpass.Text!= newpass.Text)
                {

                    await DisplayAlert("Alert", "Password does not match", "OK");
                    return;
                }

                BtnConfirm.Text = "Please wait...";
                BtnConfirm.IsEnabled = false;
                await Task.Delay(1000);

                var postData = "pincode=" + pincode.Text;
                postData += "&password=" + newpass.Text;
                postData += "&email=" +EmailVal;

                System.Diagnostics.Debug.WriteLine("PostData" + postData);

                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "changeuserpassword", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                Console.WriteLine("Reset response" + LoginDetails);
                System.Diagnostics.Debug.WriteLine("Reset response" + LoginDetails);
                obj = JObject.Parse(LoginDetails);

                //registration ok
                if (obj["success"].ToString() == "0")
                {
                   
                    await DisplayAlert("Successful", obj["message"].ToString(), "OK");
                    await Navigation.PopModalAsync();
                }
                //registration failed
                else
                {
                    Whenfail();
                    await Task.Delay(100);

                    await DisplayAlert("Error", obj["message"].ToString(), "OK");
                }
            }
            catch (Exception f)
            {
                Whenfail();
                await Task.Delay(100);
                await DisplayAlert("Error", f.Message, "OK");

            }
        }

        private void Whenfail()
        {
            BtnConfirm.Text = "   Reset   ";
            BtnConfirm.IsEnabled = true;


        }
    }
}