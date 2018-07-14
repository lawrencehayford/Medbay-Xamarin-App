using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Medbay.usedclasses;

using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace Medbay
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DealsPage : ContentPage
    {
        SessionStorage SessionObj = new SessionStorage();
        JObject obj;
        ObservableCollection<DealCart> SearchList = new ObservableCollection<DealCart>();




        public DealsPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("Data :" + SessionObj.GetItem("deal"));
            obj = JObject.Parse(SessionObj.GetItem("deal"));

            //clear all list items
            SessionStorage.GProductId.Clear();
            SessionStorage.GProductname.Clear();
            SessionStorage.GProductCategory.Clear();
            SessionStorage.GProductUnitPrice.Clear();
            SessionStorage.GProductDescription.Clear();
            SessionStorage.GProductFilename.Clear();
            SessionStorage.GProductManufacturer.Clear();
            SessionStorage.GProductPurchasedQuantity.Clear();

            ProductList.ItemsSource = SearchList;
            foreach (var product in obj)
            {

                System.Diagnostics.Debug.WriteLine("----- :" + (string)product.Value["productname"]);
                var ProdImg = "noimage.png";
                var file = (string)product.Value["filename"];
                if (file.Length > 2)
                {
                    ProdImg = "http://www.mymedbay.com/larahome/public/products/" + file;
                }

                SearchList.Add(new DealCart()
                {
                    ProdName = (string)product.Value["productname"],
                    Manufacturer = (string)product.Value["manufacturer"],
                    Amount = "GHS " + (string)product.Value["price"],
                    Instock = (string)product.Value["quantity"] + " in stock",
                    Image = ProdImg,
                    Id = (string)product.Value["id"],

                });


                //searchlist.Add((string)product["productname"]);
            }







        }

        private async Task GoToHome(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Warning", "Are you sure you want to close this? \n You will lose all your changes made if you proceed", "Yes", "No");
            if (answer)
            {
                await Navigation.PopModalAsync();
            }
        }

        private async Task Confirm(object sender, EventArgs e)
        {
            if (int.Parse(cartcount.Text) > 0) { await this.Navigation.PushModalAsync(new ShopDealPage()); }
           


        }

        protected override bool OnBackButtonPressed()
        {
            bool _canClose = true;
            ShowExitDialog();

            return _canClose;
        }
        private async void ShowExitDialog()
        {

            var answer = await DisplayAlert("Warning", "Are you sure you want to close this? \n You will lose all your changes made if you proceed", "Yes", "No");
            if (answer)
            {
                await Navigation.PopModalAsync();
            }

        }




        protected override void OnAppearing()
        {
            System.Diagnostics.Debug.WriteLine("Main Popup= " + SessionStorage.MAINPOPUP);

            if (SessionStorage.FINALCLOSE == 1) {
                this.Navigation.PopModalAsync();
            }

            //when quantity data has returned
            if (SessionStorage.MAINPOPUP == 1)
            {
                System.Diagnostics.Debug.WriteLine("popping up main= ");

                SessionStorage.MAINPOPUP = 0;
               // DisplayAlert("Returned Data is: ", SessionObj.GetItem("returnQuantity"), "OK");

                foreach (var product in obj)
                {
                    //add to ProductListToSend to be sent to next page
                    if ((string)product.Value["id"] == SessionObj.GetItem("id")) {
                        if ((int)product.Value["quantity"] < Int32.Parse(SessionObj.GetItem("returnQuantity"))) {

                            DisplayAlert("Invalid", "Your Quantity entered is greater than the stock quantity", "OK");
                            return;

                        }

                        //storring details in a list
                        SessionStorage.GProductId.Add((string)product.Value["id"]);
                        SessionStorage.GProductname.Add((string)product.Value["productname"]);
                        SessionStorage.GProductCategory.Add((string)product.Value["category"]);
                        SessionStorage.GProductUnitPrice.Add((decimal)product.Value["price"]);
                        SessionStorage.GProductDescription.Add((string)product.Value["description"]);
                        SessionStorage.GProductFilename.Add((string)product.Value["filename"]);
                        SessionStorage.GProductManufacturer.Add((string)product.Value["manufacturer"]);
                        SessionStorage.GProductPurchasedQuantity.Add(SessionObj.GetItem("returnQuantity"));
                       
                    }
                   

                }

                cartcount.Text = SessionStorage.GProductId.Count.ToString();



            }

        }


        private async Task Add(object sender, EventArgs e)
        {

            var button = (Button)sender;
            var ProductId = button.ClassId;
            if (button.Text != "Remove")
            {
                // await DisplayAlert("Alert", "Id is "+ProductId, "OK");
                SessionObj.PostItem("id", ProductId.ToString());
                await this.Navigation.PushModalAsync(new DealQuantityPage());
                button.Text = "Remove";
            }
            else {

                //deleting item from the list
                int index = SessionStorage.GProductId.FindIndex(s => s.Equals(ProductId));
                SessionStorage.GProductId.RemoveAt(index);
                SessionStorage.GProductname.RemoveAt(index);
                SessionStorage.GProductCategory.RemoveAt(index);
                SessionStorage.GProductUnitPrice.RemoveAt(index);
                SessionStorage.GProductDescription.RemoveAt(index);
                SessionStorage.GProductFilename.RemoveAt(index);
                SessionStorage.GProductManufacturer.RemoveAt(index);
                SessionStorage.GProductPurchasedQuantity.RemoveAt(index);
                button.Text = "Add";
                cartcount.Text= SessionStorage.GProductId.Count.ToString();
                
            }
        }



    }
}