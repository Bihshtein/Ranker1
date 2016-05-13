using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1 {
    public class ItemData {
        public string Name { get; private set; }
        public double Quantity { get; private set; }
        public double Price { get; private set; }
        public ItemData(string name, double quantity, double price) {
            this.Name = name;
            this.Quantity = quantity;
            this.Price = price;
        }
    }

    public class CompanyData {
        public long PriceFileID { get; private set; }
        public long StoreFileID { get; private set; }
        public string StoreFileSuffix { get; private set; }
        public string Usermame { get; private set; }
        public string Password { get; private set; }
        public CompanyData(long priceFileID, string usermame, string password, long storeFileID, string storeFileSuffix) {
            this.PriceFileID = priceFileID;
            this.Usermame = usermame;
            this.Password = password;
            this.StoreFileID = storeFileID;
            this.StoreFileSuffix = storeFileSuffix;
        }
    }  

    public enum CompanyEnum {
        TivTaam,
        RamiLevi,
        FreshMarket,
        Keshet
    }
    public class DataStructures {
        public static Dictionary<CompanyEnum, CompanyData> LoginDetails = new Dictionary<CompanyEnum, CompanyData>() {
            { CompanyEnum.Keshet, new CompanyData(7290785400000,"keshet",  string.Empty, 7290785400000 ,"2305") },
            { CompanyEnum.TivTaam, new CompanyData(7290873255550,"TivTaam", string.Empty, 7290873255550,"2005") },
            { CompanyEnum.RamiLevi, new CompanyData(7290058140886,"RamiLevi", string.Empty, 7290058140886,"2005") },
            { CompanyEnum.FreshMarket, new CompanyData(7290876100000,"freshmarket", "f_efrd", 7290876100000 ,"0900") }

        };

    
    }
}
