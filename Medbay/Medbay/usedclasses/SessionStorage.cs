using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Medbay.usedclasses
{
    class SessionStorage 
    {
        //http://mymedbay.com/larahome/public/api/
        public static string IPPORT = "";
        public static string updateLink = "";
        public static int MAINPOPUP = 0;
        public static int FINALCLOSE = 0;
        public static Dictionary<string, string> SESSION = new Dictionary<string, string>();

        
        public static List<string> GProductname = new List<string>();
        public static List<string> GProductId = new List<string>();
        public static List<decimal> GProductUnitPrice = new List<decimal>();
        public static List<int> GProductStockQuantity = new List<int>();
        public static List<string> GProductCategory = new List<string>();
        public static List<string> GProductDescription = new List<string>();
        public static List<string> GProductFilename = new List<string>();
        public static List<string> GProductManufacturer = new List<string>();
        public static List<string> GProductPurchasedQuantity = new List<string>();

        public SessionStorage() {
            
        }
        public void PostItem(string key, string keyvalue) {

            if (SESSION.ContainsKey(key) == false)
            {
                SESSION.Add(key, keyvalue);

            }else{
                SESSION.Remove(key); 
                SESSION.Add(key, keyvalue);
            }

        }

        public string GetItem(string key)
        {

            if (SESSION.ContainsKey(key) == true)
            {
                return SESSION[key].ToString();
               
            }
            else
            {
                return "";
            }
           
          

        }


        public void RemoveItem(string key)
        {

            if (SESSION.ContainsKey(key) == true)
            {
                SESSION.Remove(key);
                return;

            }
            else
            {
                return;
            }

           
        }

        public void ClearAllItem()
        {

            Dictionary<string, string> SESSION = new Dictionary<string, string>();


        }
    }
}
