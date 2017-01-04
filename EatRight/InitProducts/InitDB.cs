﻿using InitDB.Validators;
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

        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\USDA";
        private static int totalAdded = 0;
        private static int totalSkipped = 0;
        public static Dictionary<string, string> FoodGroups = new Dictionary<string, string>() {
            {"Baked","1800"},
            {"Beverages", "1400"},
            {"Sweets","1900"},
            {"Legumes","1600"},
            {"SoupAndSauce","0600"},
            {"Nuts","1200"},
            {"OilsAndFats","0400"},
            {"SpicesAndHerbs","0200"},
            {"Fish","1500"},
            {"Carbs", "2000"},
            {"Dairy", "0100"},
            {"Fruits", "0900"},
            {"Vegs", "1100"},
            {"Chicken", "0500"},
            {"Pork", "1000"},
            {"Beef", "1300"},
            {"Sausages", "0700"}
        };
        public static Dictionary<string, BasicValidator> Validators = new Dictionary<string, BasicValidator>() {
            { "Sweets", new SweetsValidator()},
            { "Nuts", new NutsValidator()},
            { "Pork", new PorkValidator()},
            { "Beef", new BeefValidator()},
            { "Vegs", new VegsValidator()},
            { "Fruits", new FruitsValidator()},
            { "Chicken", new ChickenValidator()},
            { "Dairy", new DairyValidator()},
            { "Carbs", new CarbsValidator()},
            { "Beverages", new BeveragesValidator()},
            { "Fish", new FishValidator()},
            { "Baked", new BakedValidator()},
            { "SpicesAndHerbs", new SpicesAndHerbsValidator()},
            { "OilsAndFats", new OilsAndFatsValidator()},
            { "SoupAndSauce", new SoupAndSauceValidator()},
            { "Legumes", new LegumesValidator()},
            { "Sausages", new SausagesValidator()},
            };

        public static void InitProductsCollection(bool loadGroups, bool loadManual, bool overrideDB) {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            var unit = new RestDBInterface();
            if (overrideDB)
                MongoData._database.DropCollection("products");
            var nutrientsQuery = "nutrients=203&";
            if (loadGroups)
                AddFoodGroups(unit, nutrientsQuery);

            Console.WriteLine("Total Added : " + totalAdded);
            Console.WriteLine("Total Skipped : " + totalSkipped);
        }

        private static void AddFoodGroups(RestDBInterface unit, string nutrientsQuery) {
            foreach (var foodGroup in FoodGroups.Keys) {
                Console.WriteLine("Adding group :\t\t" + foodGroup);
                var added = totalAdded;
                var skipped = totalSkipped;
                var foods = GetFoodsList(QueryData.GroupUrl, FoodGroups[foodGroup], nutrientsQuery);
                for (int i = 0; i < foods.Count; i++) {
                    var food = (dynamic)foods[i];
                    var id = ((object)food.ndbno).ToString();
                    var name = ((object)food.name).ToString();
                    name = name.Replace(".", "");// can't have this in mongo for some silly reason
                    var _params = name.Split(',').ToList();
                    var allParamsAreKnown = _params.All(Validators[foodGroup].IsValidPart);
                    if (allParamsAreKnown) {
                        var idAlreadyInDB = unit.Products.Get(int.Parse(id)) != null;
                        if (idAlreadyInDB)
                            SkipDebug(name, "already in DB");
                        else 
                            QueryAndAddSingleProduct(unit, foodGroup, name, id);
                    }
                    else
                        SkipDebug(name, "unknow parameters");
                }
                Console.WriteLine("Group Added :\t\t " + (totalAdded-added));
                Console.WriteLine("Group Skipped :\t\t " + (totalSkipped-skipped));
                Console.WriteLine();
            }
        }

        private static void QueryAndAddSingleProduct(RestDBInterface unit, string groupName, string name, string id) {
            if (unit.Products.Get(int.Parse(id)) != null) {
                SkipDebug(name, "already in DB");
            }
            else {
                var url1 = string.Format(QueryData.SingleUrl,  id, QueryData.Format, QueryData.ApiKey);
                var food1 = (dynamic)GetFood(url1, "", id);
                JArray nutrients = (food1).nutrients;
                AddProduct(groupName, unit, id, name, nutrients);
            }
        }

        public static void AddProduct(string groupName, RestDBInterface unit, string id, string name, JArray nutrients) {
            AddDebug(name);
            var collection = MongoData._database.GetCollection<BsonDocument>(MongoData.CollectionName);
            var newProduct = ProductBuilder.GetProduct(groupName, int.Parse(id), name, nutrients);
            unit.Products.Add(newProduct);
        }

        private static JArray GetFoodsList(string url, string foodGroup, string nutrientQuery) {
            var fullUrl = string.Format(QueryData.GroupUrl, foodGroup, QueryData.Format, QueryData.ApiKey, nutrientQuery);
            return JArray.FromObject(GetFood(fullUrl, foodGroup));
        }

        private static object GetFood(string fullUrl, string foodGroup = "", string num = "") {
            string data;
            var path = Path.Combine(FolderPath, foodGroup + num + ".txt");
            if (!File.Exists(path)) {
                data = new WebClient().DownloadString(fullUrl);
                File.WriteAllText(Path.Combine(FolderPath, path), data);
            }
            else
                data = File.ReadAllText(path);
            var res = JsonConvert.DeserializeObject<dynamic>(data);
            if (foodGroup != string.Empty)
                return res.report.foods;
            else
                return res.report.food;
        }


        private static void SkipDebug(string name, string reason) {
            totalSkipped++;
        }

        private static void AddDebug(string name) {
            totalAdded++;
        }


    }

   
}
