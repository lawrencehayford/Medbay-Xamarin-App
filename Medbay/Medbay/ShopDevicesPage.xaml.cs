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
	public partial class ShopDevicesPage : ContentPage
	{
        SessionStorage SessionObj = new SessionStorage();
        JArray obj;
        string[] IndividalAccount;
        string itemList;

        List<string>searchlist = new List<string>();
        List<string> Product = new List<string>();
        List<string> ProductId = new List<string>();
        List<decimal> ProductUnitPrice = new List<decimal>();
        List<int> ProductQuantity = new List<int>();
        ObservableCollection<ItemCart> List = new ObservableCollection<ItemCart>();
        ObservableCollection<SearchCart> SearchList = new ObservableCollection<SearchCart>();

        int quant;
        int count=0;
        int quantity = 0;
        decimal REALTOTAL = 0;


        public ShopDevicesPage ()
		{
			InitializeComponent ();
            ProductList.IsVisible = false;
            searchcartheader.IsVisible = false;
            cartheader.IsVisible = false;
            itemList ="";

            obj = JArray.Parse(SessionObj.GetItem("product"));
            var products = obj;
            foreach (var product in products)
            {
                if ((string)product["category"] == "Device")
                {
                    SearchList.Add(new SearchCart()
                    {
                        ProdName = (string)product["productname"],
                        Manufacturer = (string)product["manufacturer"],
                        Instock = (string)product["quantity"] + " in stock",

                    });

                }
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
            var jsonList = "[";
            //process check out here
            if (count == 0) {
                await DisplayAlert("Alert", "Please add an item", "OK");
                return;
            }

            for (var i = 0; i < Product.Count; i++)
            {
                decimal price = ProductUnitPrice[i];
                int quantity = ProductQuantity[i];
                var all = price * quantity;

                if (i + 1 < Product.Count)
                {
                    jsonList += "{'productname':'" + Product[i] 
                             + "','id':'" + ProductId[i] 
                             + "','unitprice':'" + ProductUnitPrice[i] 
                             + "','quantity':'" + ProductQuantity[i] 
                             + "'"+",'total':'" + all + "'},";

                }
                else {

                    jsonList += "{'productname':'" + Product[i] 
                             + "','id':'" + ProductId[i] 
                             + "','unitprice':'" + ProductUnitPrice[i] 
                             + "','quantity':'" + ProductQuantity[i] + "'" 
                             + ",'total':'" + all + "'}";

                }

            }
            jsonList += "]";
            //putting data into session for next page

            SessionObj.PostItem("Total",REALTOTAL.ToString());
            SessionObj.PostItem("ProductList", jsonList.ToString());
            //await DisplayAlert("Alert", SessionObj.GetItem("ProductList"), "OK");
            await this.Navigation.PushModalAsync(new CheckOutPage());
        }

        private async Task Add(object sender, EventArgs e)
        {

            if (Quantity.Text.Length<1)
            {
                await Task.Delay(10);
                await DisplayAlert("Alert", "Please enter quantity ", "OK");
                return;
            }
            ProductList.IsVisible = false;
            searchcartheader.IsVisible = false;
            stackhide.IsVisible = true;
            await Task.Delay(1000);
           

            itemList = SearchContent.Text + "~" + Quantity.Text + "~" + Amount.Text + "~" + img.Text + "~" + token.Text + "`";
            quant = 0;
            Int32.TryParse(Quantity.Text, out quant);

            //adding to list
            Product.Add(SearchContent.Text);
            ProductId.Add(token.Text);
            ProductUnitPrice.Add(Decimal.Parse(Amount.Text));
            ProductQuantity.Add(quant);


            try
            {
              
                    lst.ItemsSource = List;

                    string str = itemList;
                    string[] Account_list = str.Split('`');
                
                    count +=1;
                    IndividalAccount = Account_list[0].Split('~');
                    var overall = Decimal.Parse(IndividalAccount[2]) * Decimal.Parse(IndividalAccount[1]);
                    var ProdImg = "noimage.png";
                    if (IndividalAccount[3].Length > 2) {
                        ProdImg = "http://www.mymedbay.com/larahome/public/products/" + IndividalAccount[3];
                    }

                    string[] ItemName = IndividalAccount[0].Split(' ');
                    if (IndividalAccount[0].Length < 12)
                    {
                        ItemName[0] = IndividalAccount[0];
                    }

                    List.Add(new ItemCart()
                        {
                            No = count.ToString() + ".  ",
                            Img = ProdImg,
                            ItemName = ItemName[0],
                            ItemQuantity = IndividalAccount[1],
                            ItemTotal = "GHS" + overall,
                            Id = IndividalAccount[4],

                        });




                SearchContent.Text = "";
                Quantity.Text = "";
                Amount.Text = "";
                Amount1.Text = "";
                instock.Text = "";

                decimal total = 0;
                for (var i = 0; i < ProductUnitPrice.Count; i++)
                {
                    decimal price = ProductUnitPrice[i];
                    int quantity =  ProductQuantity[i];
                    total += price * quantity;
                }

                totalcost.Text = "Total : GHS "+ total;
                REALTOTAL = total;
                await Task.Delay(100);
                stackhide.IsVisible = false;
                lst.IsVisible = true;
                cartheader.IsVisible = true;
                ProductList.IsVisible = false;
                searchcartheader.IsVisible = false;
                cartcount.Text = (count).ToString();
            }
            catch (Exception n)
            {
                System.Diagnostics.Debug.WriteLine(" Exception caught " + n.ToString());
               
                await Task.Delay(100);
                stackhide.IsVisible = false;

            }

        }

        private void DeleteItem(object sender, EventArgs e)
        {

            
           
            var item = (Xamarin.Forms.Button)sender;
            string ef = item.CommandParameter.ToString();


            ItemCart listitem = (from itm in List
                                 where itm.Id == item.CommandParameter.ToString()
                             select itm)
                            .FirstOrDefault<ItemCart>();

            int index = ProductId.FindIndex(s => s.Equals(item.CommandParameter.ToString()));
            
            List.Remove(listitem);
            
            count -= 1;
            cartcount.Text = (count).ToString();
            
            //removing item from list
            
            Product.RemoveAt(index);
            ProductId.RemoveAt(index);
            ProductUnitPrice.RemoveAt(index);
            ProductQuantity.RemoveAt(index);

            //totalcost.Text = "Total : GHS " + total;
            decimal total = 0;
            for (var i = 0; i < ProductUnitPrice.Count; i++)
            {
                decimal sumt = Decimal.Parse(ProductUnitPrice[i].ToString());
                quantity=0;
                Int32.TryParse(ProductQuantity[i].ToString(), out quantity);
                total += sumt*quantity;
            }
           

            totalcost.Text = "Total : GHS " + total.ToString();
            REALTOTAL = total;

        }

            private void SearchContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            lst.IsVisible = false;
            cartheader.IsVisible = false;
            var keyword = SearchContent.Text;
            if (keyword.Length >0)
            {
                var suggestion = SearchList.Where(c => c.ProdName.ToLower().Contains(keyword.ToLower()));
                ProductList.ItemsSource = suggestion;
                ProductList.IsVisible = true;
                searchcartheader.IsVisible = true;
            }
            else
            {
                ProductList.IsVisible = false;
                searchcartheader.IsVisible = false;

                //if item in cart
                if (count > 0) {
                    lst.IsVisible = true;
                    cartheader.IsVisible = true;
                }
               

            }
        }

        private void ProductList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lst.IsVisible = false;
            cartheader.IsVisible = false;
            var dataItem = e.Item as SearchCart;
            if (dataItem.ProdName as string == null)
            {
                return;
            }
            else
            {
                ProductList.ItemsSource = SearchList.Where(c => c.ProdName.Equals(dataItem.ProdName as string));
                ProductList.IsVisible = false;
                searchcartheader.IsVisible = false;
                SearchContent.Text = dataItem.ProdName as string;

                obj = JArray.Parse(SessionObj.GetItem("product"));
                var products = obj;
                foreach (var product in products)
                {
                    if (SearchContent.Text.Equals((string)product["productname"]))
                    {
                        
                        Amount.Text = (string)product["price"];
                        Amount1.Text ="GHS " +(string)product["price"];
                        instock.Text = (string)product["quantity"] ;
                        img.Text = (string)product["filename"];
                        token.Text = (string)"T"+product["id"];
                        prodid.Text= (string) product["id"];
                        break;
                    }

                   
                }

            }

        }

        protected override void OnAppearing()
        {
            System.Diagnostics.Debug.WriteLine("Main Popup= " + SessionStorage.MAINPOPUP);

            if (SessionStorage.MAINPOPUP == 1)
            {
                System.Diagnostics.Debug.WriteLine("popping up main= ");

                SessionStorage.MAINPOPUP = 0;
                this.Navigation.PopModalAsync();

            }
            
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
    }
}