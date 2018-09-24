using System;
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
	public partial class AdListPage : ContentPage
	{
        SessionStorage SessionObj = new SessionStorage();
        JArray obj;
        ObservableCollection<AddList> List = new ObservableCollection<AddList>();

        public AdListPage ()
		{
			InitializeComponent ();

            obj = JArray.Parse(SessionObj.GetItem("ad_list"));
            lst.ItemsSource = List;
            lst.SelectedItem = null; // de-select the row

            var products = obj;
            foreach (var product in products)
            {
                var ProdImg = "noimage.png";
                var FileName = (string)product["filename"];
               
                if (FileName.Length > 2)
                {
                    ProdImg = "http://www.mymedbay.com/larahome/public/products/" + FileName;
                }
                var ProdDescription = (string)product["description"];
                List.Add(new AddList()
                    {
                        ProdName = (string)product["productname"],
                        Manufacturer = " "+ ProdDescription+"\n Posted By: "+(string)product["manufacturer"]+ "\n Price: GHS" + (string)product["price"]+"\n Quantity: " +(string)product["quantity"] + "\n Expiring: -" + (string)product["expiring"],
                        Image = ProdImg,
                        Contact = (string)product["tel"]


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
            

           await this.Navigation.PushModalAsync(new PostAdPage());
           

        }

         void MakeCall(object sender, EventArgs e)
        {
            
            
            var ImageTel = (Image)sender;
            var Contact = ImageTel.ClassId;
            System.Diagnostics.Debug.WriteLine("Number To call :" + Contact);
            Device.OpenUri(new Uri("tel://"+Contact));
            return;
        }



        protected override void OnAppearing()
        {


            System.Diagnostics.Debug.WriteLine("Main Popup= " + SessionStorage.MAINPOPUP);

            if (SessionStorage.MAINPOPUP == 1)
            {
                System.Diagnostics.Debug.WriteLine("popping up main= ");
                SessionStorage.FINALCLOSE = 1;
                this.Navigation.PopModalAsync();

            }


        }
    }
}