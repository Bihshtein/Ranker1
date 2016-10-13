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
            lines.ForEach((line) => AddProduct(unit, line,false));
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
                var collection = _database.GetCollection<BsonDocument>("products");
                unit.Products.Add(GetProduct(int.Parse(id), name, res));
            }
        }

        public static List<string> PotentialSizeNames = new List<string>() {
            "medium", "large", "slice", "breast", "piece whole","piece"
        };


        public static Product GetProduct(int id, string name, dynamic jsonReponse) {
            var parts = name.Split('_');
            bool hasSkin = true;
            string animal = string.Empty;
            string servingState = "Raw";
            if (parts.Length == 2)
            {
                name = parts[0];
                servingState = parts[1];
            }
            else if (parts.Length > 2)
            {
                animal = parts[0];
                name = parts[1];
                servingState = parts[2];
                if (parts.Length == 4)
                {
                    hasSkin = !(parts[3] == "NoSkin");
                }
            }

            JArray nutrients = jsonReponse.report.food.nutrients;
            var imgPath = FolderPath + name + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            else if (name.Length > 5)
                imgBytes = File.ReadAllBytes(FolderPath + "Morty.png");
            else
                imgBytes = File.ReadAllBytes(FolderPath + "Rick.png");
            var p = new Product() {ID=id, Name = name, Image = imgBytes, Animal = animal, ServingState = servingState, HasSkin = hasSkin };

            p.Protein = GetMeasure(nutrients, "Protein");
            p.Fat = GetMeasure(nutrients, "Total lipid (fat)");
            p.Fiber = GetMeasure(nutrients, "Fiber, total dietary");
            p.Carbs = GetMeasure(nutrients, "Carbohydrate, by difference");
            p.VitaminC = GetMeasure(nutrients, "Vitamin C, total ascorbic acid");
            p.Thiamin = GetMeasure(nutrients, "Thiamin");
            p.Riboflavin = GetMeasure(nutrients, "Riboflavin");
            p.Niacin = GetMeasure(nutrients, "Niacin");
            p.PantothenicAcid = GetMeasure(nutrients, "Pantothenic acid");
            p.VitaminB6 = GetMeasure(nutrients, "Vitamin B-6");
            p.Betaine = GetMeasure(nutrients, "Betaine");
            p.VitaminB12 = GetMeasure(nutrients, "Vitamin B-12");
            p.Folate = GetMeasure(nutrients, "Folate, total");
            p.VitaminA = GetMeasure(nutrients, "Vitamin A, IU");
            p.VitaminE = GetMeasure(nutrients, "Vitamin E (alpha-tocopherol)");
            p.VitaminK = GetMeasure(nutrients, "Vitamin K (phylloquinone)");



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
