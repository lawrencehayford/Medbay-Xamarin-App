using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Medbay.usedclasses;
using Newtonsoft.Json.Linq;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Medbay
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PostNeedFinalPage : ContentPage
	{
        JObject obj;
        SessionStorage SessionObj = new SessionStorage();
        public PostNeedFinalPage ()
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
                BtnConfirm.Text = "Please wait...";
                BtnConfirm.IsEnabled = false;
                await Task.Delay(1000);

                //fullname,telephone,email,country,password,repassword
                var postData = "productname="+itemname.Text;
                postData += "&quantity="+itemquantity.Text;
                postData += "&Username=" + SessionObj.GetItem("name");
                postData += "&email=" + SessionObj.GetItem("email");
                postData += "&tel=" + SessionObj.GetItem("tel");
                postData += "&isneedmet=0"; 



                System.Diagnostics.Debug.WriteLine("PostData" + postData );

                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "postneed", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                Console.WriteLine("Registration response" + LoginDetails);
                System.Diagnostics.Debug.WriteLine("Registration response" + LoginDetails);
                obj = JObject.Parse(LoginDetails);

                //ad ok
                if (obj["success"].ToString() == "0")
                {
                    //success

                    await DisplayAlert("Success", obj["message"].ToString(), "OK");
                    await Navigation.PopModalAsync();
                }
                // failed
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
            BtnConfirm.Text = "   Confirm   ";
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