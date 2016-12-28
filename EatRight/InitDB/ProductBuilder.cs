using InitDB.Validators;
using Newtonsoft.Json.Linq;
using RestModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB {
    class ProductBuilder {
        public static Product GetProduct(string groupName, int id, string name, JArray nutrients, double weight) {
            var validator = InitDB.Validators[groupName];
            var p = new Product() { ID = id};
            p.FoodGroup = groupName.ToLower();
            var parts = name.Split(',').ToList();
            if (groupName == "manual")
                p.Name1 = name;
            else 
                parts.ForEach((item) => TryMatchPartToProperty(p, item, validator));
            p.Weight = weight;
            SetNutrientProperties(nutrients, p, weight);
            return p;
        }

        public static void TryMatchPartToProperty(Product p, string item,BasicValidator validator) {
            var part = item.Trim();
            TrySetCommonProperties(p, part);
            TrySetCustomProperties(p, part,validator);
        }

        private static string GetUpdatedStringParam(List<string> containerList, string item, string param) {
            if (containerList.Any((i) => item.Contains(i))) {
                param = GetUpdatedStringParam(item, param);
            }
            return param;
        }

        private static string GetUpdatedStringParam(string item, string param) {
                if (param == null)
                    return item;
                else if (!param.Contains(item))
                    return param + '|' + item;
            return param;
        }

        public static void TrySetCommonProperties(Product p, string item) {
            if (CommonValidator.IsPreparationOption(item))
                p.PreparationMethod = GetUpdatedStringParam(CommonValidator.GetPreparationOption(item),  p.PreparationMethod);
            p.StorageMethod = GetUpdatedStringParam(CommonValidator.StorageOptions, item, p.StorageMethod);
            p.FatDetails = GetUpdatedStringParam(CommonValidator.FatOptions, item, p.FatDetails);
            p.PackDetails = GetUpdatedStringParam(CommonValidator.PackOptions, item, p.PackDetails);
            p.PeelDetails = GetUpdatedStringParam(CommonValidator.PeelingOptions, item, p.PeelDetails);
            if (CommonValidator.CheckWithWord(CommonValidator.AdditionalFoodsData, item)) {
                if (p.FoodsData == null)
                    p.FoodsData = item;
                else
                    p.FoodsData += '|' + item;
            }
            if (CommonValidator.CheckWithWord(CommonValidator.AdditionalHealthData, item)) {
                if (p.HealthData == null)
                    p.HealthData = item;
                else
                    p.HealthData += '|' + item;
            }
            if (CommonValidator.BoneOptions.Contains(item))
                p.BoneDetails = item;
        }

        public static void TrySetCustomProperties(Product p, string item, BasicValidator validator) {

            if (validator.IsMainPart(item)) {
                if (p.Name1 == null)
                    p.Name1 = validator.GetPrettyName(item);
                else
                    p.Name1 = item + '|' + p.Name1;
                p.Name1 = p.Name1.ToLower();

            }

            if (validator.IsSecondPart(item)) {
                var nameAndCut = validator.GetNameAndDescription(item);
                p.Name2 = nameAndCut.Item1;
                if (nameAndCut.Item2 != string.Empty)
                    p.Name3 = nameAndCut.Item2;
                p.Name2 = p.Name2.ToLower();

            }
            if (validator.IsThirdPart(item)) { 
                p.Name3 = item;
                p.Name3 = p.Name3.ToLower();
            }
        }

        private static void SetNutrientProperties(JArray nutrients, Product p, double weight) {
            p.Protein = GetNutrient(nutrients, "Protein", weight);   //203
            p.Sugar = GetNutrient(nutrients, "Sugars, total", weight);//269
            p.Fat = GetNutrient(nutrients, "Total lipid (fat)", weight);//204
            p.Fiber = GetNutrient(nutrients, "Fiber, total dietary", weight);//291
            p.Carbs = GetNutrient(nutrients, "Carbohydrate, by difference", weight);//205
            p.VitaminC = GetNutrient(nutrients, "Vitamin C, total ascorbic acid", weight);//401
            p.Thiamin = GetNutrient(nutrients, "Thiamin", weight);//404
            p.Riboflavin = GetNutrient(nutrients, "Riboflavin", weight);//405
            p.Niacin = GetNutrient(nutrients, "Niacin", weight);//406
            p.PantothenicAcid = GetNutrient(nutrients, "Pantothenic acid", weight);//410
            p.VitaminB6 = GetNutrient(nutrients, "Vitamin B-6", weight);//415
            p.VitaminD = GetNutrient(nutrients, "Vitamin D", weight);//328
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

        private static double GetNutrient(JArray array, string item, double weight) {
            var obj = ((dynamic)array.FirstOrDefault((i) => ((dynamic)i).nutrient == item));
            if (obj == null)
                return 0;
            double result = 0;
            double.TryParse(obj.value.ToString(), out result);
            return result * (100.0 / weight);
        }


        // Curently out of use for eficiency issues
        public static byte[] GetPNG(string majorName, string animal) {
            var imgPath = InitDB.FolderPath + majorName + ".png";
            byte[] imgBytes = null;
            if (File.Exists(imgPath))
                imgBytes = File.ReadAllBytes(imgPath);
            else if (majorName.Length > 5)
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Morty.png");
            else
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Rick.png");
            return imgBytes;
        }
    }
}
