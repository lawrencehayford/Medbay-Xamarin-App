using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Medbay.usedclasses;
using Newtonsoft.Json.Linq;
using System.Threading;
using Plugin.Fingerprint;

namespace Medbay
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        JObject obj;
        SessionStorage SessionObj = new SessionStorage();

        public LoginPage ()
		{
			InitializeComponent ();

            //sign up tap recognizer
            var signup_tap = new TapGestureRecognizer();
            signup_tap.Tapped += (s, e) =>
            {
                Navigation.PushModalAsync(new RegisterPage());
            };
            signup.GestureRecognizers.Add(signup_tap);

            //password reset tap recognizer
            var passreset_tap = new TapGestureRecognizer();
            passreset_tap.Tapped += (s, e) =>
            {
                //
                //  Do your work here.
                //
                Navigation.PushModalAsync(new ForgotPasswordPage());
            };
            passwordreset.GestureRecognizers.Add(passreset_tap);



            if (Application.Current.Properties.ContainsKey("username"))
            {
                username.Text = Application.Current.Properties["username"] as string;


            }
        }


        private async Task Login(object sender, EventArgs e)
        {
            this.IsBusy = true;
            this.IsVisible = true;
            LoginBtn.Text = "Logging in...";
            LoginBtn.IsEnabled = false;

            var Uname = username.Text;
            var Pass = password.Text;

            await Task.Delay(1000);
            LoginBtn.Text = "Please wait..";
            LoginBtn.TextColor = Color.White;
            //password.Text = "";
            try
            {
                if (String.IsNullOrEmpty(Uname) || String.IsNullOrEmpty(Pass))
                {

                    Whenfail();
                    await DisplayAlert("Alert", "Missing input required!", "OK");
                    return;
                }
                else if (Uname.Length < 5 || Pass.Length < 4)
                {
                    Whenfail();
                    await DisplayAlert("Alert", "Invalid Username/Password!", "OK");
                    return;
                }


                var postData = "email=" + Uname;
                    postData += "&password=" + Pass;
                System.Diagnostics.Debug.WriteLine("PostData" + postData);

                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "getuserdetails", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                Console.WriteLine("Login Details" + LoginDetails);
                System.Diagnostics.Debug.WriteLine("Login Details" + LoginDetails);
                obj = JObject.Parse(LoginDetails);

                //login failed
                if (obj["success"].ToString() == "1")
                {
                    await DisplayAlert("Alert", " Login Failed. Please try again ", "OK");
                   
                    Whenfail();
                    await Task.Delay(100);

                    return;
                }
                //login successful
                else {

                    Application.Current.Properties["username"] = obj["email"].ToString();
                    await Application.Current.SavePropertiesAsync();

                    if (!String.IsNullOrEmpty(obj["company_name"].ToString()))
                    {
                        SessionObj.PostItem("name", obj["company_name"].ToString());
                    }
                    else
                    {
                        SessionObj.PostItem("name", obj["name"].ToString());
                    }

                    SessionObj.PostItem("email", obj["email"].ToString());
                    SessionObj.PostItem("tel", obj["tel"].ToString());
                    SessionObj.PostItem("category", obj["category"].ToString());
                    SessionObj.PostItem("location", obj["location"].ToString());
                    SessionObj.PostItem("country", obj["country"].ToString());
                    SessionObj.PostItem("status", obj["status"].ToString());
                    SessionObj.PostItem("product", obj["product"].ToString());
                    SessionObj.PostItem("need", obj["need"].ToString());
                    SessionObj.PostItem("usertoken", obj["id"].ToString());
                    SessionObj.PostItem("deal", obj["deal"].ToString());
                    SessionObj.PostItem("ad_list", obj["ad_list"].ToString());

                    if (obj["status"].ToString().Equals("Active"))
                    {
                        await Navigation.PushModalAsync(new HomePage());
                    }
                    else {
                        await DisplayAlert("Alert", " Your Account has been blocked. Please contact our helpline ", "OK");
                        return;
                    }
                   

                }



            }
            catch (Exception f)
            {
                await DisplayAlert("Alert", " Exception caught " + f.Message, "OK");
                Console.WriteLine(" Exception caught " + f.ToString());
                Whenfail();
                return;
            }
        }




        private void Whenfail()
        {
            
            LoginBtn.IsEnabled = true;
            this.IsBusy = false;
            LoginBtn.Text = "Login";
            LoginBtn.TextColor = Color.White;
            LoginBtn.Text = "Login";


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




        protected override void OnAppearing()
        {
            base.OnAppearing();
            //put email into the email entry 
            if (Application.Current.Properties.ContainsKey("username"))
            {
                username.Text = Application.Current.Properties["username"] as string;


            }

        }


        private async Task Authenticate(object sender, EventArgs e)
        {
            if (!Application.Current.Properties.ContainsKey("biologin"))
            {
                await DisplayAlert("Alert", " Finger print not enabled on your device.  Login to the app > Settings to enable it", "OK");
                return;
            }
            else
            {
                var bio = Application.Current.Properties["biologin"] as string;
                if (bio == "0")
                {
                    await DisplayAlert("Alert", " Finger print is deactivated.  Login to the app > Settings to enable it ", "OK");
                    return;
                }

            }

            var result = await CrossFingerprint.Current.AuthenticateAsync("Authenticate fingerprint to login!");
            if (result.Authenticated)
            {
                await BioLogin();
                
            }
            else
            {
                await DisplayAlert("Alert", " Failed to login ", "OK");
            }
        }


        private async Task BioLogin()
        {
            this.IsBusy = true;
            this.IsVisible = true;
            LoginBtn.Text = "Logging in...";
            LoginBtn.IsEnabled = false;

            var Uname = username.Text;

            await Task.Delay(1000);
            LoginBtn.Text = "Please wait..";
            LoginBtn.TextColor = Color.White;
            //password.Text = "";
            try
            {
               if (Uname.Length < 5)
                {
                    Whenfail();
                    await DisplayAlert("Alert", "Invalid Username/Password!", "OK");
                    return;
                }


                var postData = "email=" + Uname;
                System.Diagnostics.Debug.WriteLine("PostData" + postData);

                GetOnlineRequest sendrequest = new GetOnlineRequest(SessionStorage.IPPORT + "getuserbiodetails", "POST", postData);
                string LoginDetails = sendrequest.GetResponse();
                Console.WriteLine("Login Details" + LoginDetails);
                System.Diagnostics.Debug.WriteLine("Login Details" + LoginDetails);
                obj = JObject.Parse(LoginDetails);

                //login failed
                if (obj["success"].ToString() == "1")
                {
                    await DisplayAlert("Alert", " Login Failed. Please try again ", "OK");

                    Whenfail();
                    await Task.Delay(100);

                    return;
                }
                //login successful
                else
                {

                    Application.Current.Properties["username"] = obj["email"].ToString();
                    await Application.Current.SavePropertiesAsync();
                    
                    if (!String.IsNullOrEmpty(obj["company_name"].ToString()))
                    {
                        SessionObj.PostItem("name", obj["company_name"].ToString());
                    }
                    else
                    {
                        SessionObj.PostItem("name", obj["name"].ToString());
                    }
                    
                   

                    SessionObj.PostItem("email", obj["email"].ToString());
                    SessionObj.PostItem("tel", obj["tel"].ToString());
                    SessionObj.PostItem("category", obj["category"].ToString());
                    SessionObj.PostItem("location", obj["location"].ToString());
                    SessionObj.PostItem("country", obj["country"].ToString());
                    SessionObj.PostItem("status", obj["status"].ToString());
                    SessionObj.PostItem("product", obj["product"].ToString());
                    SessionObj.PostItem("need", obj["need"].ToString());
                    SessionObj.PostItem("usertoken", obj["id"].ToString());
                    SessionObj.PostItem("deal", obj["deal"].ToString());
                    SessionObj.PostItem("ad_list", obj["ad_list"].ToString());

                    if (obj["status"].ToString().Equals("Active"))
                    {
                        await Navigation.PushModalAsync(new HomePage());
                    }
                    else
                    {
                        await DisplayAlert("Alert", " Your Account has been blocked. Please contact our helpline ", "OK");
                        return;
                    }


                }



            }
            catch (Exception f)
            {
                await DisplayAlert("Alert", " Exception caught " + f.Message, "OK");
                Console.WriteLine(" Exception caught " + f.ToString());
                Whenfail();
                return;
            }
        }


    }
}