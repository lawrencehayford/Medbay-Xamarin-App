﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Medbay.usedclasses;

using System.ComponentModel;
using System.Windows.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Medbay
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PostNeedPage : ContentPage
	{
        SessionStorage SessionObj = new SessionStorage();
        JArray obj;
        ObservableCollection<NeedList> List = new ObservableCollection<NeedList>();

        public PostNeedPage ()
		{
			InitializeComponent ();

            obj = JArray.Parse(SessionObj.GetItem("need"));
            lst.ItemsSource = List;
            lst.SelectedItem = null; // de-select the row
            var products = obj;
            foreach (var product in products)
            {

                List.Add(new NeedList()
                    {
                        ProdName = (string)product["ItemName"]+ " :" + (string)product["Quantity"] + "pcs",
                        Tel = (string)product["RequestorTel"],
                        User = (string)product["Username"],
                    

                });

               
                //searchlist.Add((string)product["productname"]);
            }


           

        }

       

        private async Task GoToHome(object sender, EventArgs e)
        {
            
                await Navigation.PopModalAsync();
           
        }

        private async Task Confirm(object sender, EventArgs e)
        {
            

           await this.Navigation.PushModalAsync(new PostNeedFinalPage());
           

        }

       
        protected  override void OnAppearing()
        {


            System.Diagnostics.Debug.WriteLine("Main Popup= " + SessionStorage.MAINPOPUP);

            if (SessionStorage.MAINPOPUP == 1)
            {
                System.Diagnostics.Debug.WriteLine("popping up main= ");
                SessionStorage.FINALCLOSE = 1;
                this.Navigation.PopModalAsync();

            }


        }

        void MakeCall(object sender, EventArgs e)
        {


            var ImageTel = (Image)sender;
            var Contact = ImageTel.ClassId;
            System.Diagnostics.Debug.WriteLine("Number To call :" + Contact);
            Device.OpenUri(new Uri("tel://" + Contact));
            return;
        }

    }
}