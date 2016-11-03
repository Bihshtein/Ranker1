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
    class ProductBuilder
    {
        public static Product GetProduct(int id, string name, JArray nutrients, double weight)
        {
            bool hasSkin = true;
            string animal = string.Empty;
            string servingState = "Raw";
            var parts = name.Split(',');
            string newName = name;
            foreach (var item in parts) {
                var newItem = item.Trim();
                if (InitDB.FoodGroups.ContainsKey(newItem))
                    animal = newItem;
                if (CommonValidator.CookingOptions.Contains(newItem))
                    servingState = item;
                if (PorkValidator.PorkMainParts.Contains(newItem))
                    newName = newItem;
                if (BeefValidator.CheckWithoutCut(newItem, BeefValidator.BeefMainParts))
                    newName = newItem;
                if (BeefValidator.CheckWithoutCut(newItem, BeefValidator.BeefSecondParts) && newName == name)
                    newName = newItem;
                if (BeefValidator.CheckWithoutCut(newItem, BeefValidator.BeefCutDetails) && newName == name)
                    newName = newItem;
            }

            var imgPath = InitDB.FolderPath + newName + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            else if (RestRepository<Product>.Animals.Contains(animal))
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + animal + ".png");
            else if (newName.Length > 5)
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Morty.png");
            else
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Rick.png");
            var p = new Product() { ID = id, Name = newName, /*Image = imgBytes,*/ Animal = animal, ServingState = servingState, HasSkin = hasSkin,Weight = weight };
            AddNutrients(nutrients, p, weight);

            return p;
        }

        private static void AddNutrients(JArray nutrients, Product p, double weight ) {
            p.Protein = GetNutrient(nutrients, "Protein", weight);   //203
            p.Fat = GetNutrient(nutrients, "Total lipid (fat)", weight);//204
            p.Fiber = GetNutrient(nutrients, "Fiber, total dietary", weight);//291
            p.Carbs = GetNutrient(nutrients, "Carbohydrate, by difference", weight);//205
            p.VitaminC = GetNutrient(nutrients, "Vitamin C, total ascorbic acid", weight);//401
            p.Thiamin = GetNutrient(nutrients, "Thiamin", weight);//404
            p.Riboflavin = GetNutrient(nutrients, "Riboflavin", weight);//405
            p.Niacin = GetNutrient(nutrients, "Niacin", weight);//406
            p.PantothenicAcid = GetNutrient(nutrients, "Pantothenic acid", weight);//410
            p.VitaminB6 = GetNutrient(nutrients, "Vitamin B-6", weight);//415
            p.Betaine = GetNutrient(nutrients, "Betaine", weight);//454
            p.VitaminB12 = GetNutrient(nutrients, "Vitamin B-12", weight);//418
            p.Folate = GetNutrient(nutrients, "Folate, total", weight);//417
            p.VitaminA = GetNutrient(nutrients, "Vitamin A, IU", weight);//318
            p.VitaminE = GetNutrient(nutrients, "Vitamin E (alpha-tocopherol)", weight);//323
            p.VitaminK = GetNutrient(nutrients, "Vitamin K (phylloquinone)", weight);//430
            p.Calcium = GetNutrient(nutrients, "Calcium, Ca", weight);//301
            p.Iron = GetNutrient(nutrients, "Iron, Fe", weight);//303
            p.Magnesium = GetNutrient(nutrients, "Magnesium, Mg", weight);//304
            p.Phosphorus = GetNutrient(nutrients, "Phosphorus, P", weight);//305
            p.Potassium = GetNutrient(nutrients, "Potassium, K", weight);//306
            p.Sodium = GetNutrient(nutrients, "Sodium, Na", weight);//307
            p.Zinc = GetNutrient(nutrients, "Zinc, Zn", weight);//309
        }

        private static double GetNutrient(JArray array, string item, double weight)
        {
            var obj = ((dynamic)array.FirstOrDefault((i) => ((dynamic)i).nutrient == item));
            if (obj == null)
                return 0;
            double result = 0;
            double.TryParse(obj.value.ToString(), out result);
            return result * (100.0/weight);
        }
    }
}
