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
        private static int totalAdded = 0;
        private static int totalSkipped = 0;
        public static Dictionary<string, string> FoodGroups = new Dictionary<string, string>() {{ "Pork","1000"},{ "Beef","1300"}};   
        public static Dictionary<string, Func<string, bool>> ProductGropusValidation = new Dictionary<string, Func<string, bool>>() {{ "Beef", BeefValidator.IsBeefParameter},{ "Pork", PorkValidator.IsPorkParameter}};

        public static void InitProductsCollection() {
            var unit = new RestDBInterface();
            MongoData._database.DropCollection("products");// Remove in case you want to override the existing products 
            var nutrientsQuery = string.Empty;
            foreach (var item in QueryData.Nutrients) {
                nutrientsQuery += "nutrients=" + item+"&";
            }
            AddFoodGroups(unit, nutrientsQuery);
            AddManual(unit, nutrientsQuery);
            Console.WriteLine("Total Added : " + totalAdded);
            Console.WriteLine("Total Skipped : " + totalSkipped);
        }
     
        private static void AddFoodGroups(RestDBInterface unit, string nutrientsQuery) {
            foreach (var item in FoodGroups.Keys) {

                var str = string.Format(QueryData.GroupUrl, FoodGroups[item], QueryData.Format, QueryData.ApiKey, nutrientsQuery);
                var data = new WebClient().DownloadString(str);
                var res = JsonConvert.DeserializeObject<dynamic>(data);
                JArray foods = res.report.foods;
                foreach (dynamic food in foods) {
                    var id = ((object)food.ndbno).ToString();
                    var name = ((object)food.name).ToString();
                    var _params = name.Split(',').ToList();
                    var allParamsAreKnown = _params.All(ProductGropusValidation[item] );
                    if (allParamsAreKnown) {
                        var idAlreadyInDB = unit.Products.Get(int.Parse(id)) != null;
                        if (idAlreadyInDB)
                            SkipDebug(name, "already in DB");
                        else
                            AddProduct(unit, id, name, food.nutrients, double.Parse(((object)food.weight).ToString()));
                    }
                    else
                        SkipDebug(name, "unknow parameters");
                }
            }
        }
        private static void AddManual(RestDBInterface unit, string nutrientsQuery) {
            var lines = File.ReadAllLines(Path.Combine(FolderPath, "Products_IDS.csv"));

            foreach (var line in lines) {
                var parts = line.Split(',');
                var name = parts[0];
                var id = parts[1];
                if (unit.Products.Get(int.Parse(id)) != null) {
                    SkipDebug(name, "already in DB");
                }
                else {
                    var singleStr = string.Format(QueryData.SingleUrl, nutrientsQuery, id, QueryData.Format, QueryData.ApiKey);
                    var singleData = new WebClient().DownloadString(singleStr);
                    var singleRes = JsonConvert.DeserializeObject<dynamic>(singleData);
                    JArray _foods = singleRes.report.foods;
                    dynamic _food = _foods[0];
                    AddProduct(unit, id, name, _food.nutrients, double.Parse(((object)_food.weight).ToString()));
                }
            }
        }

        public static void AddProduct(RestDBInterface unit, string id, string name, JArray nutrients, double weight) {
            AddDebug(name);
            var collection = MongoData._database.GetCollection<BsonDocument>(MongoData.CollectionName);
            var newProduct = ProductBuilder.GetProduct(int.Parse(id), name, nutrients, weight);
            unit.Products.Add(newProduct);
        }

       
        private static void SkipDebug(string name, string reason) {
            totalSkipped++;
            Console.WriteLine("Skipping: " + name + ", reason : " + reason);
        }

        private static void AddDebug(string name) {
            totalAdded++;
            Console.WriteLine("Adding: " + name );
        }


    }
}
