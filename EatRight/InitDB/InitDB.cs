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


        public static void Init() {
            var unit = new RestDBInterface();
            var lines = File.ReadAllLines(FolderPath + "Products_IDS.csv").ToList();
            _database.DropCollection("products");
            lines.ForEach((line) => AddProduct(unit, line,true));
        }


        public static void AddProduct(RestDBInterface unit, string line, bool online) {
            var parts = line.Split(',');
            var name = parts[0];
            if (unit.Products.Get(name) != null)
                Console.WriteLine("Skipping : " + name);
            else {
                Console.WriteLine("Adding : " + name);
                string data;
                var path = Path.Combine(FolderPath, "JSONS", name + ".txt");

                if (online)
                {
                    var str = string.Format(Url, parts[1], Format, ApiKey);
                    data = new WebClient().DownloadString(str);
                    File.WriteAllText(path, data);
                }
                else
                {
                    data = File.ReadAllText(path);
                }
                var res = JsonConvert.DeserializeObject<dynamic>(data);
                var collection = _database.GetCollection<BsonDocument>("products");
                unit.Products.Add(GetProduct(name, res));
            }
        }

        public static List<string> PotentialSizeNames = new List<string>() {
            "medium", "large", "slice", "breast", "piece whole"
        };

        public static List<string> PotentialServingNames = new List<string>() {
             "NLEA serving", "cup", "steak", "fillet",
        };

        public static Product GetProduct(string name, dynamic jsonReponse) {
            JArray nutrients = jsonReponse.report.food.nutrients;
            var imgPath = FolderPath + name + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            else if (name.Length > 5)
                imgBytes = File.ReadAllBytes(FolderPath + "Morty.png");
            else
                imgBytes = File.ReadAllBytes(FolderPath + "Rick.png");
            var p = new Product() { Name = name, Image = imgBytes };
           /* p.Protein = GetMeasure(nutrients, "Protein");
            p.Fat = GetMeasure(nutrients, "Total lipid (fat)");
            p.Fiber = GetMeasure(nutrients, "Fiber, total dietary");

            p.Serving= GetSize(jsonReponse, PotentialServingNames);
            */
            p.UnitSize = GetSize(jsonReponse, name.ToLower());
            if (p.UnitSize == 0)
                p.UnitSize = GetSize(jsonReponse, PotentialSizeNames);

            return p;
        }

        public static double GetSize(dynamic json, List<string> PotentialSizeNames) {
            double size =0;
            for (int i = 0; i < PotentialSizeNames.Count && size == 0; i++)
                size = GetSize(json, PotentialSizeNames[i]);
            return size;
        }
        public static double GetMeasure(JArray array, string item ) {
            var obj = ((dynamic)array.FirstOrDefault((i) => ((dynamic)i).name == item));
            if (obj == null)
                return 0;
            return double.Parse(obj.value.ToString());
        }

        public static double GetSize(dynamic json, string item) {
            JArray firstMeasures = json.report.food.nutrients[0].measures;
            var obj = ((dynamic)firstMeasures.FirstOrDefault((i) => ((string)((dynamic)i).label).StartsWith(item)));
            if (obj == null)
                return 0;
            return double.Parse(obj.eqv.ToString());
        }
    }
}
