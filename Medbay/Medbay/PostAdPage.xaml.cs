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
	public partial class PostAdPage : ContentPage
	{
        JObject obj;
        string base64;
        SessionStorage SessionObj = new SessionStorage();
        public PostAdPage ()
		{
			InitializeComponent ();
            category.Items.Add("Drug");
            category.Items.Add("Herbal");
            category.Items.Add("Sundry");
            category.Items.Add("Device");
        }

        private async Task BtnConfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                byte[] b = System.IO.File.ReadAllBytes(base64);
                String basestr = Convert.ToBase64String(b);
                //System.Diagnostics.Debug.WriteLine("base64 " + basestr);

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

                if (String.IsNullOrEmpty(expDate.Date.ToString("dd-MMM-yyyy").Trim()))
                {

                    await DisplayAlert("Alert", "Expiring Date input required!", "OK");
                    return;
                }
                BtnConfirm.Text = "Please wait...";
                BtnConfirm.IsEnabled = false;
                await Task.Delay(1000);

                //fullname,telephone,email,country,password,repassword 
                var postData = "productname="+itemname.Text;
                postData += "&category="+ category.Items[category.SelectedIndex].ToString();
                postData += "&price="+unitprice.Text;
                postData += "&quantity="+itemquantity.Text;
                postData += "&description="+itemname.Text;
                postData += "&manufacturer="+ SessionObj.GetItem("name");
                postData += "&expDate=" + expDate.Date.ToString("dd-MMM-yyyy").Trim();
                postData += "&photo=" + basestr;
                postData += "&isFromApi=true";

                

                System.Diagnostics.Debug.WriteLine("PostData" + postData );
                System.Diagnostics.Debug.WriteLine(SessionStorage.IPPORT + "addproductfromuser");

                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "addproductfromuser", "POST", postData);
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
                await DisplayAlert("Error", "This error might be that u haven't filled all required field or you haven't uploaded a valid photo or a server error has occured. Please find below the error details \n ("+f.Message+")", "OK");
                return;

            }
        }
        private void Whenfail()
        {
            BtnConfirm.Text = "   Post AD   ";
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

        async void getImage(object sender, EventArgs e)
        {
            try
            {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                });


                if (file == null)
                    return;
                base64 = file.Path;

                photo.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

                photo.WidthRequest = 300;

            }
            catch (Exception f)
            {
                await DisplayAlert("Please Enable camera and storage permission on Settings ", f.Message, "OK");
            }

           



        }


    }
}