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
	public partial class RegisterPage : ContentPage
	{
        JObject obj;
        public RegisterPage ()
		{
			InitializeComponent ();
		}

        private async Task BtnConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
               
                if (String.IsNullOrEmpty(fullname.Text))
                {

                    await DisplayAlert("Alert", "Missing full name input required!", "OK");
                    return;
                }
                else if (String.IsNullOrEmpty(companyname.Text))
                {

                    await DisplayAlert("Alert", "Missing Company Name input required!", "OK");
                    return;
                }

                else if (String.IsNullOrEmpty(telephone.Text))
                {
                   
                    await DisplayAlert("Alert", "Missing telephone input required!", "OK");
                    return;
                }
                else if (String.IsNullOrEmpty(email.Text))
                {

                    await DisplayAlert("Alert", "Missing email input required!", "OK");
                    return;
                }
                else if (String.IsNullOrEmpty(country.Text))
                {

                    await DisplayAlert("Alert", "Missing country input required!", "OK");
                    return;
                }
                else if (String.IsNullOrEmpty(location.Text))
                {

                    await DisplayAlert("Alert", "Missing location input required!", "OK");
                    return;
                }
                else if (String.IsNullOrEmpty(password.Text))
                {

                    await DisplayAlert("Alert", "Missing password input required!", "OK");
                    return;
                }
                else if (password.Text != repassword.Text)
                {

                    await DisplayAlert("Alert", "Password does not match", "OK");
                    return;
                }

                BtnConfirm.Text = "Please wait...";
                BtnConfirm.IsEnabled = false;
                await Task.Delay(1000);

                //fullname,telephone,email,country,password,repassword
                var postData = "fullname=" + fullname.Text;
                postData += "&companyname=" + companyname.Text;
                postData += "&telephone=" + telephone.Text;
                postData += "&email=" + email.Text;
                postData += "&country=" + country.Text;
                postData += "&location=" + location.Text;
                postData += "&password=" + password.Text;

                System.Diagnostics.Debug.WriteLine("PostData" + postData);

                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "registermobileuser", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                Console.WriteLine("Registration response" + LoginDetails);
                System.Diagnostics.Debug.WriteLine("Registration response" + LoginDetails);
                obj = JObject.Parse(LoginDetails);

                //registration ok
                if (obj["success"].ToString() == "0")
                {
                    //saving email
                    Application.Current.Properties["username"] = email.Text;
                    await Application.Current.SavePropertiesAsync();

                    await DisplayAlert("Registration successful", obj["message"].ToString(), "OK");
                    await Navigation.PopModalAsync();
                }
                //registration failed
                else
                {
                    Whenfail();
                    await Task.Delay(100);

                    await DisplayAlert("Registration Failed", obj["message"].ToString(), "OK");
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
            BtnConfirm.Text = "   Register   ";
            BtnConfirm.IsEnabled = true;


        }
    }
}