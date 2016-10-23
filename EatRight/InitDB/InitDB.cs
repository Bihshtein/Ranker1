using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    public class InitDB {

        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        public static MongoClient _client = new MongoClient();
        public static IMongoDatabase _database = _client.GetDatabase("test");
        public static string ApiKey = "fo9SmQOhJvGXLkWqXbFUGFEb89SYgVgTA1CkEbua";
        public static string Format = "json";
        public static string Url = "http://api.nal.usda.gov/ndb/reports/?ndbno={0}&type=f&format={1}&api_key={2}";
        public static string CollectionName = "products";


        public static void InitProductsCollection() {
            var unit = new RestDBInterface();
            var productsIDsIndex = File.ReadAllLines(FolderPath + "Products_IDS.csv").ToList();
           // _database.DropCollection("products");// Remove in case you want to override the existing products 
            productsIDsIndex.ForEach((products) => AddProduct(unit, products, true));
        }


        public static void AddProduct(RestDBInterface unit, string line, bool online) {
            var parts = line.Split(',');
            var name = parts[0];
            var id = parts[1];
            if (unit.Products.Get(int.Parse(id)) != null)
                Console.WriteLine("Skipping : " + name);
            else {
                Console.WriteLine("Adding : " + name);
                string data;
                var path = Path.Combine(FolderPath, "JSONS", name + ".txt");

                if (online)
                {
                    var str = string.Format(Url, id, Format, ApiKey);
                    data = new WebClient().DownloadString(str);
                    File.WriteAllText(path, data);
                }
                else
                {
                    data = File.ReadAllText(path);
                }
                var res = JsonConvert.DeserializeObject<dynamic>(data);
                var collection = _database.GetCollection<BsonDocument>(CollectionName);
                unit.Products.Add(USDAJsonParser.GetProduct(int.Parse(id), name, res));
            }
        }

      
    }
}
