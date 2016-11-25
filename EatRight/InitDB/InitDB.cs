using InitDB.Validators;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InitDB {
    public class InitDB {

        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\FruitsDB\";
        private static int totalAdded = 0;
        private static int totalSkipped = 0;
        public static Dictionary<string, string> FoodGroups = new Dictionary<string, string>() { {"Fish","1500"}, { "Beverages", "1400" } ,{ "Carbs", "2000" }, { "Dairy", "0100" }, { "Fruits", "0900" }, { "Vegs", "1100" }, { "Chicken", "0500" }, { "Pork", "1000" }, { "Beef", "1300" } };
        public static Dictionary<string, BasicValidator> Validators = new Dictionary<string, BasicValidator>() {
                { "Pork", new PorkValidator()},
                { "Beef", new BeefValidator()},
                { "Vegs", new VegsValidator()},
                { "Fruits", new FruitsValidator()},
                { "Chicken", new ChickenValidator()},
                { "Dairy", new DairyValidator()},
                { "Carbs", new CarbsValidator()},
                { "Beverages", new BeveragesValidator()},
                { "Fish", new FishValidator()},
                { "manual", null}
            };

        public static void InitProductsCollection(bool loadGroups, bool loadManual, bool overrideDB) {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            var unit = new RestDBInterface();
            if (overrideDB)
                MongoData._database.DropCollection("products");
            var nutrientsQuery1 = string.Empty;
            var nutrientsQuery2 = string.Empty;
            foreach (var item in QueryData.Nutrients1) {
                nutrientsQuery1 += "nutrients=" + item + "&";
            }
            foreach (var item in QueryData.Nutrients2) {
                nutrientsQuery2 += "nutrients=" + item + "&";
            }
            if (loadGroups)
                AddFoodGroups(unit, nutrientsQuery1, nutrientsQuery2);
            if (loadManual)
                AddManual(unit, nutrientsQuery1, nutrientsQuery2);

            Console.WriteLine("Total Added : " + totalAdded);
            Console.WriteLine("Total Skipped : " + totalSkipped);
        }

        private static void AddFoodGroups(RestDBInterface unit, string nutrientsQuery1, string nutrientsQuery2) {
            foreach (var item in FoodGroups.Keys) {
                Console.WriteLine("Adding group :\t\t" + item);
                var added = totalAdded;
                var skipped = totalSkipped;
                var foods1 = GetFoods(QueryData.GroupUrl, FoodGroups[item], nutrientsQuery1, "1");
                var foods2 = GetFoods(QueryData.GroupUrl, FoodGroups[item], nutrientsQuery2, "2");

                for (int i = 0; i < Math.Min(foods1.Count, foods2.Count); i++) {
                    var food1 = (dynamic)foods1[i];
                    var food2 = (dynamic)foods2[i];
                    var id1 = ((object)food1.ndbno).ToString();
                    var name1 = ((object)food1.name).ToString();
                    var id2 = ((object)food1.ndbno).ToString();
                    var name2 = ((object)food1.name).ToString();
                    if (id1 == id2 && name1 == name2) {
                        var _params = name1.Split(',').ToList();

                        var allParamsAreKnown = _params.All(Validators[item].IsValidPart);
                        if (allParamsAreKnown) {
                            var idAlreadyInDB = unit.Products.Get(int.Parse(id1)) != null;
                            if (idAlreadyInDB)
                                SkipDebug(name1, "already in DB");
                            else {
                                JArray nutrients1 = food1.nutrients;
                                JArray nutrients2 = food2.nutrients;
                                AddProduct(item, unit, id1, name1, new JArray(nutrients1.Concat(nutrients2)), double.Parse(((object)food1.weight).ToString()));
                            }
                        }
                        else
                            SkipDebug(name1, "unknow parameters");
                    }
                }
                Console.WriteLine("Group Added :\t\t " + (totalAdded-added));
                Console.WriteLine("Group Skipped :\t\t " + (totalSkipped-skipped));
                Console.WriteLine();
            }
        }
        private static void AddManual(RestDBInterface unit, string nutrientsQuery1, string nutrientsQuery2) {
            var lines = File.ReadAllLines(Path.Combine(FolderPath, "Products_IDS.csv"));

            foreach (var line in lines) {
                var parts = line.Split(',');
                var name = parts[0];
                var id = parts[1];
                if (unit.Products.Get(int.Parse(id)) != null) {
                    SkipDebug(name, "already in DB");
                }
                else {

                    var url1 = string.Format(QueryData.SingleUrl, nutrientsQuery1, id, QueryData.Format, QueryData.ApiKey);
                    var url2 = string.Format(QueryData.SingleUrl, nutrientsQuery2, id, QueryData.Format, QueryData.ApiKey);
                    var food1 = (dynamic)GetFoods(url1)[0];

                    var food2 = (dynamic)GetFoods(url2)[0];
                    JArray nutrients1 = (food1).nutrients;
                    JArray nutrients2 = (food2).nutrients;
                    AddProduct("manual", unit, id, name, new JArray(nutrients1.Concat(nutrients2)), double.Parse(((object)food1.weight).ToString()));
                }
            }
        }

        private static JArray GetFoods(string url, string foodGroup, string nutrientQuery, string num = "") {
            var fullUrl = string.Format(QueryData.GroupUrl, foodGroup, QueryData.Format, QueryData.ApiKey, nutrientQuery);
            return GetFoods(fullUrl, foodGroup, num);
        }

        private static JArray GetFoods(string fullUrl, string foodGroup = "", string num = "") {
            string data;
            var path = Path.Combine(FolderPath, foodGroup + num + ".txt");
            if (!File.Exists(path)) {
                data = new WebClient().DownloadString(fullUrl);
                File.WriteAllText(Path.Combine(FolderPath, path), data);
            }
            else
                data = File.ReadAllText(path);
            var res = JsonConvert.DeserializeObject<dynamic>(data);
            return res.report.foods;
        }

        public static void AddProduct(string groupName, RestDBInterface unit, string id, string name, JArray nutrients, double weight) {
            AddDebug(name);
            var collection = MongoData._database.GetCollection<BsonDocument>(MongoData.CollectionName);
            var newProduct = ProductBuilder.GetProduct(groupName, int.Parse(id), name, nutrients, weight);
            unit.Products.Add(newProduct);
        }


        private static void SkipDebug(string name, string reason) {
            totalSkipped++;
             //Console.WriteLine("Skipping: " + name + ", reason : " + reason);
        }

        private static void AddDebug(string name) {
            totalAdded++;
            //Console.WriteLine("Adding: " + name + "");
        }


    }
}
