﻿using InitDB.Validators;
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
        public static Product GetProduct(int id, string name, JArray nutrients, double weight) {
            var p = new Product() { ID = id};
            var parts = name.Split(',').ToList();
            parts.ForEach((item) => TryMatchPartToProperty(p, item));
            SetNameForManuallyAddedProduct(p, name);
            SetNutrientProperties(nutrients, p, weight);
            return p;
        }

        public static void SetNameForManuallyAddedProduct(Product p, string name) {
            if(p.Name == null && p.Animal == null)
                p.Name = name;
        }

        public static void TryMatchPartToProperty(Product p, string item) {
            var part = item.Trim();
            TrySetCommonProperty(p, part);
            TryAnimalCommonProperty(p, part);
        }

        public static void TrySetCommonProperty(Product p, string item) {
            if (CommonValidator.CookingOptions.Contains(item))
                p.CookingMethod = item;
            if (CommonValidator.PreservationOptions.Contains(item))
                p.PreservationMethod = item;
            if (CommonValidator.StorageOptions.Contains(item))
                p.StorageMethod = item;
            if (CommonValidator.FatOptions.Contains(item))
                p.FatDetails = item;
            if (CommonValidator.BoneOptions.Contains(item))
                p.BoneDetails = item;
        }

        public static void TryAnimalCommonProperty(Product p, string item) {
            BasicValidator validator;
            if (InitDB.FoodGroups.ContainsKey(item))
                p.Animal = item;
            if (p.Animal != null) {
                validator = InitDB.Validators[p.Animal];

                if (validator.MainParts.Contains(item)) {
                    p.Name = item;
                    p.Name = validator.GetPrettyName(p.Name);
                }
                if (validator.IsSecondPart(item)) {
                    p.MinorName = p.Name;
                    var nameAndCut = validator.GetNameAndCut(item);
                    p.Name = nameAndCut.Item1;
                    if (nameAndCut.Item2 != string.Empty)
                        p.MeatCut = nameAndCut.Item2;
                }
                if (validator.Cuts.Contains(item))
                    p.MeatCut = item;
            }
        }

        private static void SetNutrientProperties(JArray nutrients, Product p, double weight) {
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
            else if (RestRepository<Product>.Animals.Contains(animal))
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + animal + ".png");
            else if (majorName.Length > 5)
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Morty.png");
            else
                imgBytes = File.ReadAllBytes(InitDB.FolderPath + "Rick.png");
            return imgBytes;
        }
    }
}
