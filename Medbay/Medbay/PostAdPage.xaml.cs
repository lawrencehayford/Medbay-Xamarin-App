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
	public partial class PostAdPage : ContentPage
	{
        JObject obj;
        public PostAdPage ()
		{
			InitializeComponent ();
		}

        private async Task BtnConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
               
                if (String.IsNullOrEmpty(itemname.Text))
                {

                    await DisplayAlert("Alert", "Item Name input required!", "OK");
                    return;
                }
                else if (String.IsNullOrEmpty(itemquantity.Text))
                {
                   
                    await DisplayAlert("Alert", "Quantity input required!", "OK");
                    return;
                }
                else if (String.IsNullOrEmpty(unitprice.Text))
                {

                    await DisplayAlert("Alert", "Price input required!", "OK");
                    return;
                }
                else if (category.Items[category.SelectedIndex].Length < 1)
                {

                    await DisplayAlert("Alert", "Item Category input required!", "OK");
                    return;
                }
               
                BtnConfirm.Text = "Please wait...";
                BtnConfirm.IsEnabled = false;
                await Task.Delay(1000);

                //fullname,telephone,email,country,password,repassword
                var postData = "fullname=";
                postData += "&telephone=";
                postData += "&email=";
                postData += "&country=" ;
                postData += "&location=";
                postData += "&password=";

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
                    Application.Current.Properties["username"] = "";
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

        private async Task GoToHome(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Warning", "Are you sure you want to close this? \n You will lose all your changes made if you proceed", "Yes", "No");
            if (answer)
            {
                await Navigation.PopModalAsync();
            }
        }

    }
}