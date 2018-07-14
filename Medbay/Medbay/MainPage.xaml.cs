using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Medbay.usedclasses;
using System.Threading;

namespace Medbay
{
	public partial class MainPage : ContentPage
	{
        SessionStorage SessionObj = new SessionStorage();
        public MainPage()
		{
			InitializeComponent();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await MainProgressBar.ProgressTo(1, 3000, Easing.Linear);


            var postData = "";

            try
            {

                System.Diagnostics.Debug.WriteLine(" Bio PostData " + postData);
                GetOnlineRequest sendrequest = new GetOnlineRequest("http://mymedbay.com/link.php", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                System.Diagnostics.Debug.WriteLine("Returned Data " + LoginDetails);

                if (LoginDetails[0] == 'h')
                {

                    var requestPulled = LoginDetails.Split('~');
                    SessionStorage.IPPORT = requestPulled[0];
                    SessionStorage.updateLink = requestPulled[1];

                }
                else
                {
                    await DisplayAlert("Server Error", "Sorry our onlinebanking platform is down at the moment. please try again later", "OK");

                    await MainProgressBar.ProgressTo(1, 1000, Easing.Linear);
                    await DisplayAlert("Alert", "System logging out", "ok");
                    Thread.CurrentThread.Abort();
                }



            }
            catch (Exception n)
            {
                System.Diagnostics.Debug.WriteLine(" Exception Caught" + n.ToString());

                await DisplayAlert("Network Error", " Please Check Your internet connection \n" + n.Message, "OK");

                await MainProgressBar.ProgressTo(1, 1000, Easing.Linear);
                await DisplayAlert("Alert", "System logging out", "ok");
                Thread.CurrentThread.Abort();
            }

            await Navigation.PushModalAsync(new LoginPage());




            // await Navigation.PushModalAsync(new LoginPage());

        }


    }
}
