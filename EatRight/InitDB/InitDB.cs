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
        public static List<int> Nutrients = new List<int> { 203, 204, 291, 205, 401, 404, 405, 406, 410, 415, 454, 418, 417, 318, 323, 430, 301, 303, 304,/* 305, 306, 307,*/ 309 };

        public static MongoClient _client = new MongoClient();
        public static IMongoDatabase _database = _client.GetDatabase("test");
        public static string ApiKey = "fo9SmQOhJvGXLkWqXbFUGFEb89SYgVgTA1CkEbua";
        public static string Format = "json";
        public static string SingleUrl = "http://api.nal.usda.gov/ndb/nutrients/?{0}ndbno={1}&type=f&format={2}&api_key={3}";
        public static string GroupUrl = "http://api.nal.usda.gov/ndb/nutrients/?fg={0}&format={1}&api_key={2}&{3}&max=1500";
        public static Dictionary<string, string> FoodGroups = new Dictionary<string, string>() {
            { "Pork","1000"}
        };
        public static List<string> CookingOptions = new List<string>() {"crumbles","pickled","unprepared","microwaved","baked", "pickled","roasted", "pan-broiled", "pan-broil", "broiled", "raw", "pan-fried", "braised" };
        public static List<string> StorageOptions = new List<string>() {"ground", "pre-sliced","frozen" , "cooked" , "canned" , "heated" ,"unheated" };
        public static List<string> FatOptions = new List<string>() {"72% lean / 28% fat", "84% lean / 16% fat", "96% lean / 4% fat", "extra lean","separable fat","extra lean (approximately 5% fat)","regular (approximately 13% fat)","regular (approximately 11% fat)","extra lean (approximately 4% fat)","extra lean and regular","rendered fat", "separable lean and fat", "separable lean only" };
        public static List<string> BoneOptions = new List<string>() { "bone-in", "boneless" };
        public static List<string> SecondPartOptions = new List<string>() {
            "leg cap steak","blade", "tenderloin", "country-style ribs", "rump", "shank", "slice", "whole", "spiral slice" , "arm picnic","rump half", "shank half", "steak","blade roll","blade (chops or roasts)","center rib (chops or roasts)",
            "blade (chops)","top loin (chops)", "sirloin (chops)", "center rib (chops)", "center loin (chops)","top loin (chops)", "top loin (chops)","sirloin (chops or roasts)",
            "blade (roasts)","top loin (roasts)", "sirloin (roasts)", "center rib (roasts)", "center loin (roasts)","center slice"};
        public static List<string> MainPartOptions = new List<string>() {"carcass","belly","salt pork","backfat","backribs","leg (ham)","ham","feet","bacon", "loin", "shoulder","spareribs", "ham -- water added", "ham and water product", "ham with natural juices" };
        public static List<string> PreperationOptions = new List<string>() { "fresh", "cured" };

        public static string CollectionName = "products";


        public static void InitProductsCollection() {
            var unit = new RestDBInterface();
            _database.DropCollection("products");// Remove in case you want to override the existing products 

            var nutrientsQuery = string.Empty;
            foreach (var item in Nutrients) {
                nutrientsQuery += "nutrients=" + item+"&";
            }
            
            var lines = File.ReadAllLines(Path.Combine(FolderPath, "Products_IDS.csv"));

            foreach (var line in lines) {
                var parts = line.Split(',');
                var name = parts[0];
                var id = parts[1];
                var singleStr = string.Format(SingleUrl, nutrientsQuery, id, Format, ApiKey);
                var singleData = new WebClient().DownloadString(singleStr);
                var singleRes = JsonConvert.DeserializeObject<dynamic>(singleData);
                JArray _foods = singleRes.report.foods;
                dynamic _food = _foods[0];
                AddProduct(unit, id, name, _food.nutrients, double.Parse(((object)_food.weight).ToString()));
            }


            var str = string.Format(GroupUrl, FoodGroups["Pork"], Format, ApiKey, nutrientsQuery);
            var data = new WebClient().DownloadString(str);
            var res = JsonConvert.DeserializeObject<dynamic>(data);
            JArray foods = res.report.foods;
            foreach (dynamic food in foods) {
                var id = ((object)food.ndbno).ToString();
                var name = ((object)food.name).ToString();
                var parts = name.Split(',').ToList();
                if (parts.All(IsInLists))
                    AddProduct(unit, id, name, food.nutrients, double.Parse(((object)food.weight).ToString()));
            }

       

        }
        
        private static bool IsInLists(string namePart) {
            namePart = namePart.Trim();
            return (CookingOptions.Contains(namePart) ||
                    StorageOptions.Contains(namePart) ||
                    FatOptions.Contains(namePart) ||
                    BoneOptions.Contains(namePart) ||
                    SecondPartOptions.Contains(namePart) ||
                    MainPartOptions.Contains(namePart) ||
                    PreperationOptions.Contains(namePart) ||
                    FoodGroups.Keys.Contains(namePart)
                    );
        }


        public static void AddProduct(RestDBInterface unit, string id, string name, JArray nutrients, double weight) {
            if (unit.Products.Get(int.Parse(id)) != null)
                Console.WriteLine("Skipping : " + name);
            else {
                Console.WriteLine("Adding : " + name);
                var collection = _database.GetCollection<BsonDocument>(CollectionName);
                unit.Products.Add(ProductBuilder.GetProduct(int.Parse(id), name, nutrients,weight));
            }
        }

      
    }
}
