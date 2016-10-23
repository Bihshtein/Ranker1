using Newtonsoft.Json.Linq;
using RestModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB
{
    class USDAJsonParser
    {
        public static List<string> PotentialSizeNames = new List<string>() { "medium", "large", "slice", "breast", "piece whole", "piece" };

        public static Product GetProduct(int id, string name, dynamic jsonReponse)
        {
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
            var imgPath = InitDB.FolderPath + name + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            else if (RestRepository<Product>.Animals.Contains(animal))
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + animal + ".png");
            else if (name.Length > 5)
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Morty.png");
            else
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Rick.png");
            var p = new Product() { ID = id, Name = name, Image = imgBytes, Animal = animal, ServingState = servingState, HasSkin = hasSkin };

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
            p.Calcium = GetMeasure(nutrients, "Calcium, Ca");
            p.Iron = GetMeasure(nutrients, "Iron, Fe");
            p.Magnesium = GetMeasure(nutrients, "Magnesium, Mg");
            p.Phosphorus = GetMeasure(nutrients, "Phosphorus, P");
            p.Potassium = GetMeasure(nutrients, "Potassium, K");
            p.Sodium = GetMeasure(nutrients, "Sodium, Na");
            p.Zinc = GetMeasure(nutrients, "Zinc, Zn");


            p.UnitSize = GetSize(jsonReponse, name.ToLower());
            if (p.UnitSize == 0)
                p.UnitSize = GetSize(jsonReponse, PotentialSizeNames);

            return p;
        }

        private static double GetSize(dynamic json, List<string> PotentialSizeNames)
        {
            double size = 0;
            for (int i = 0; i < PotentialSizeNames.Count && size == 0; i++)
                size = GetSize(json, PotentialSizeNames[i]);
            return size;
        }
        private static double GetMeasure(JArray array, string item)
        {
            var obj = ((dynamic)array.FirstOrDefault((i) => ((dynamic)i).name == item));
            if (obj == null)
                return 0;
            return double.Parse(obj.value.ToString());
        }

        private static double GetSize(dynamic json, string item)
        {
            JArray firstMeasures = json.report.food.nutrients[0].measures;
            var obj = ((dynamic)firstMeasures.FirstOrDefault((i) => ((string)((dynamic)i).label).StartsWith(item)));
            if (obj == null)
                return 0;
            return double.Parse(obj.eqv.ToString());
        }
    }
}
