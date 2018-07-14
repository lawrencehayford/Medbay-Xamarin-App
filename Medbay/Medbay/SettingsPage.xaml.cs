using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Medbay
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        string FastbalanceActivated;
        public SettingsPage()
        {
            InitializeComponent();

                if (Application.Current.Properties.ContainsKey("biologin"))
            {
                FastbalanceActivated = Application.Current.Properties["biologin"] as string;
               
            }
            else
            {
                myswitch.IsToggled = false;
            }

            if(FastbalanceActivated=="1"){
                myswitch.IsToggled = true;
            }else{
                myswitch.IsToggled = false; 
            }



            myswitch.Toggled += HandleSwitchToggledByUser;

        }

        private void GoToHome(object sender, EventArgs e)
        {
            //var FastBalance = new NavigationPage(new FastBalance());



            this.Navigation.PopModalAsync();

            // Navigation.PushModalAsync(new FastBalance());
        }

        async void HandleSwitchToggledByUser(object sender, ToggledEventArgs e)
        {
            try
            {
                if (myswitch.IsToggled == true)
                {
                   
                    Application.Current.Properties["biologin"] = "1";
                    await Application.Current.SavePropertiesAsync();


                    await DisplayAlert(
                    "Success",
                    "Finger Print activated successfully ",
                    "OK");

                    return;



                }
                else
                {
                    Application.Current.Properties["biologin"] = "0";
                    await Application.Current.SavePropertiesAsync();

                    await DisplayAlert(
                   "Success",
                   "Finger Print deactivated successfully ",
                   "OK");
                }

            }
            catch (Exception f) {
                await DisplayAlert(
                   "Error",
                   "An Error has occured"+f.ToString(),
                   "OK");

            }


        }
    }

    }

