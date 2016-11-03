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
            { "Pork","1000"},
            { "Beef","1300"}
        };
        public static List<string> CookingOptions = new List<string>() {"slow roasted","boiled","fast roasted","fast fried","grilled","pan-browned","crumbles","pickled","unprepared","microwaved","baked", "pickled","roasted", "pan-broiled", "pan-broil", "broiled", "raw", "pan-fried", "braised" };
        public static List<string> StorageOptions = new List<string>() {"ground", "pre-sliced","frozen" , "cooked" , "canned" , "heated" ,"unheated" };
        public static List<string> FatOptions = new List<string>() {"97% lean meat / 3% fat","70% lean meat / 30% fat","85% lean meat / 15% fat","93% lean meat /7% fat","93% lean meat / 7% fat","97% lean meat /3% fat","95% lean meat / 5% fat","93% lean meat / 7% fat","90% lean meat / 10% fat","80% lean meat / 20% fat","75% lean meat / 25% fat","72% lean / 28% fat", "84% lean / 16% fat", "96% lean / 4% fat", "extra lean","separable fat","extra lean (approximately 5% fat)","regular (approximately 13% fat)","regular (approximately 11% fat)","extra lean (approximately 4% fat)","extra lean and regular","rendered fat", "separable lean and fat", "separable lean only" };
        public static List<string> BoneOptions = new List<string>() { "bone-in", "boneless" };
        public static List<string> SecondPartOptions = new List<string>() {
            "leg cap steak","blade", "tenderloin", "country-style ribs", "rump", "shank", "slice", "whole", "spiral slice" , "arm picnic","rump half", "shank half", "steak","blade roll","blade (chops or roasts)","center rib (chops or roasts)",
            "blade (chops)","top loin (chops)", "sirloin (chops)", "center rib (chops)", "center loin (chops)","top loin (chops)", "top loin (chops)","sirloin (chops or roasts)",
            "blade (roasts)","top loin (roasts)", "sirloin (roasts)", "center rib (roasts)", "center loin (roasts)","center slice"};
        
        public static List<string> PreperationOptions = new List<string>() { "fresh", "cured" };

        public static List<string> PorkMainParts = new List<string>() {"carcass", "belly", "salt pork", "backfat", "backribs", "leg (ham)", "ham", "feet", "bacon", "loin", "shoulder", "spareribs", "ham -- water added", "ham and water product", "ham with natural juices" };

        public static List<string> BeefMainParts = new List<string>() {"liver","heart","kidney","tongue","loin","flank", "chuck","rib","round", "brisket","plate","short loin"};
        public static List<string> BeefSecondParts = new List<string>()
        {"small end rib","striploin","strip loin","rump centre","ribs prepared","oyster blade","flat","hind shin", "eye round","cube roll","chuck eye roll","bolar blade","chuck eye Country-Style ribs","clod","ribeye","top loin petite", "ribeye petite" ,"ribeye  petite", "top loin petite", "top round","tri - tip","tri-tip", "arm pot", "mock tender","blade","top blade",
            "chuck eye", "short ribs","bottom round","point half","whole", "flat half","knuckle","eye of round","outside round",
            "tip round","top round","tenderloin","shoulder top blade","eye","small end (ribs 10-12)","whole (ribs 6-12)", "shoulder pot","large end (ribs 6-9)",
            "shortribs","back ribs","rib eye","back ribs", "porterhouse",  "t-bone","retail cuts","bottom sirloin butt","top sirloin petite", 
            "top loin", "top sirloin", "bottom sirloin", "chuck for stew", "composite of trimmed retail cuts" };


        public static Dictionary<string, Func<string,bool>> ProductGropusValidation = new Dictionary<string, Func<string, bool>>() {
            { "Beef", IsInLists2},
            { "Pork", IsInLists}
        };
        public static List<string> BeefCuts = new List<string>() {"cap-off steak/roast","navel end","point end","pot roast or steak","pot roast","center steak","filet","cap steak","roast/fillet","roast/filet", "roast or steak","steak/roast", "steak/fillet","fillet", "cap", "roast", "full cut", "steak", "roast" };

        public static List<string> BeefDescription = new List<string>() {"grass-fed", "Wagyu","variety meats and by-products","America's Beef Roast", "Denver Cut" ,"imported"};
        public static List<string> BeefImportDescription = new List<string>() {  "New Zealand", "Australian" };
        public static List<string> BeefCutDetails = new List<string>() {"under blade","lip off", "lip-on", "lip-off", "outside skirt", "inside skirt",  };

        public static List<string> BeefGrades = new List<string>() {"Aust. marble score 4/5","Aust. marble score 9", "prime","all grades", "choice", "select", };
        public static List<string> BeefTrimmedDetails = new List<string>() { "trimmed to 1/4\" fat", "trimmed to 1 / 4\" fat", "trimmed to 1/8\" fat", "trimmed to 1 / 8\" fat", "trimmed to 0\" fat", "trimmed to 1/8\"fat", };
                                                                            
        public static List<string> GroundBeefTypes = new List<string>() { "loaf", "patty cooked","patty","crumbles" };
        public static string CollectionName = "products";


        public static void InitProductsCollection() {
            var unit = new RestDBInterface();
            _database.DropCollection("products");// Remove in case you want to override the existing products 
            var nutrientsQuery = string.Empty;
            foreach (var item in Nutrients) {
                nutrientsQuery += "nutrients=" + item+"&";
            }
            AddFoodGroups(unit, nutrientsQuery);
            AddManual(unit, nutrientsQuery);

        }

        private static void AddFoodGroups(RestDBInterface unit, string nutrientsQuery) {
            foreach (var item in FoodGroups.Keys) {

                var str = string.Format(GroupUrl, FoodGroups[item], Format, ApiKey, nutrientsQuery);
                var data = new WebClient().DownloadString(str);
                var res = JsonConvert.DeserializeObject<dynamic>(data);
                JArray foods = res.report.foods;
                foreach (dynamic food in foods) {
                    var id = ((object)food.ndbno).ToString();
                    var name = ((object)food.name).ToString();
                    var parts = name.Split(',').ToList();
                    if (parts.All(ProductGropusValidation[item])) {
                        if (unit.Products.Get(int.Parse(id)) != null)
                            Console.WriteLine("Skipping : " + name);
                        else
                            AddProduct(unit, id, name, food.nutrients, double.Parse(((object)food.weight).ToString()));

                    }
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
                    Console.WriteLine("Skipping : " + name);
                }
                else {
                    Console.WriteLine("Adding : " + name);
                    var singleStr = string.Format(SingleUrl, nutrientsQuery, id, Format, ApiKey);
                    var singleData = new WebClient().DownloadString(singleStr);
                    var singleRes = JsonConvert.DeserializeObject<dynamic>(singleData);
                    JArray _foods = singleRes.report.foods;
                    dynamic _food = _foods[0];
                    AddProduct(unit, id, name, _food.nutrients, double.Parse(((object)_food.weight).ToString()));
                }

            }
        }
        
        private static bool IsInLists(string namePart) {
            namePart = namePart.Trim();
            return (CookingOptions.Contains(namePart) ||
                    StorageOptions.Contains(namePart) ||
                    FatOptions.Contains(namePart) ||
                    BoneOptions.Contains(namePart) ||
                    SecondPartOptions.Contains(namePart) ||
                    PorkMainParts.Contains(namePart) ||
                    PreperationOptions.Contains(namePart) ||
                    FoodGroups.Keys.Contains(namePart)
                    );
        }

        private static bool IsInLists2(string namePart) {
            namePart = namePart.Trim();
            return (CookingOptions.Contains(namePart) ||
                    StorageOptions.Contains(namePart) ||
                    FatOptions.Contains(namePart) ||
                    BoneOptions.Contains(namePart) ||
                    BeefSecondParts.Contains(namePart) ||
                    BeefMainParts.Contains(namePart) ||
                    BeefImportDescription.Contains(namePart) ||
                    CheckWithoutCut(namePart) ||
                    BeefDescription.Contains(namePart) ||
                    BeefCuts.Contains(namePart) ||
                    BeefCutDetails.Contains(namePart) ||
                    PreperationOptions.Contains(namePart) ||
                    BeefGrades.Contains(namePart) ||
                    BeefTrimmedDetails.Contains(namePart) ||
                    GroundBeefTypes.Contains(namePart) ||
                    FoodGroups.Keys.Contains(namePart)
                    );
        }

        private static bool CheckWithoutCut(string namePart) {
            return
            BeefCuts.Any((cut) => BeefMainParts.Contains(namePart.Replace(cut, string.Empty).Trim())) ||
            BeefCuts.Any((cut) => BeefSecondParts.Contains(namePart.Replace(cut, string.Empty).Trim())) ||
            BeefCuts.Any((cut) => BeefCutDetails.Contains(namePart.Replace(cut, string.Empty).Trim()));
        }


        public static void AddProduct(RestDBInterface unit, string id, string name, JArray nutrients, double weight) {
            Console.WriteLine("Adding : " + name);
            var collection = _database.GetCollection<BsonDocument>(CollectionName);
            unit.Products.Add(ProductBuilder.GetProduct(int.Parse(id), name, nutrients,weight));
        }

      
    }
}
