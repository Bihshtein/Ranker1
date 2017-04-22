using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using RestModel;

namespace InitProducts {
    public class USDA
    {
      
        public static Dictionary<string, string> FoodGroups = new Dictionary<string, string>() {
            {"Baked","1800"},
            {"Fast foods","2100"},
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
            {"Sausages", "0700"},
            { "Snacks", "2500"}
        };
        public static Dictionary<string, BasicValidator> Validators = new Dictionary<string, BasicValidator>() {
            { "Fast foods", new FastFoodsValidator()},
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
            { "Snacks", new SnacksValidator()},
            { "Legumes", new LegumesValidator()},
            { "Sausages", new SausagesValidator()},
            };
    }
  

    public class ProductBuilder {
        public static Product GetProduct(string groupName, int id, string name, JArray nutrients) {
            var validator = USDA.Validators[groupName];
            var p = new Product() { ID = id, Types = new HashSet<ProductType>() };
            p.FoodGroup = groupName.ToLower();
            var parts = name.Split(',').ToList();
            if (groupName == "manual")
                p.Name1 = name;
            else 
                parts.ForEach((item) => TryMatchPartToProperty(p, item, validator));
            if (nutrients != null)
            {
                SetWeights(nutrients, "Protein", p);
                SetNutrientProperties(nutrients, p);
            }
            
			if (IsMeatProduct(p))
            {
                p.Types.Add(ProductType.Meat);
            }
            if (IsDairyProduct(p))
            {
                p.Types.Add(ProductType.Dairy);
            }
            if (IsFishProduct(p))
            {
                p.Types.Add(ProductType.Fish);
            }
            if (IsSeafoodProduct(p))
            {
                p.Types.Add(ProductType.Seafood);
            }
            if (IsFromAnimalProduct(p)) // Must run this after IsMeatProduct, IsDairyProduct, IsFishProduct, IsSeafoodProduct
            {
                p.Types.Add(ProductType.FromAnimal);
            }
            if (IsNonKosherProduct(p)) // Must run this after IsSeafoodProduct
            {
                p.Types.Add(ProductType.NonKosher);
            }
            return p;
        }

               private class InvertedDuplicateComparer<T> : IComparer<T> where T : IComparable
        {
            public int Compare(T x, T y)
            {
                var result = y.CompareTo(x);
                if (result == 0)
                    return 1;   // Handle equality as being greater
                else
                    return result;
            }
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
                if (nameAndCut.Item2 != null && nameAndCut.Item2 != string.Empty)
                    p.Name3 = nameAndCut.Item2;
                p.Name2 = p.Name2.ToLower();

            }
            if (validator.IsThirdPart(item)) { 
                p.Name3 = validator.GetThirdPart(item);
                p.Name3 = p.Name3.ToLower();
            }
        }

        

        private static void SetNutrientProperties(JArray nutrients, Product p) {
            p.Nutrients = new Dictionary<string, Dictionary<string, double>>();
            nutrients.ToList().ForEach(n => SetNutrient(nutrients, (dynamic)n, p));
        }

      

        private static void SetNutrient(JArray nutrients, dynamic obj, Product p) {
            double value,weight = 0;
            double.TryParse(obj.measures[0].value.ToString(), out value);
            double.TryParse(obj.measures[0].eqv.ToString(), out weight);
            if (!p.Nutrients.ContainsKey((string)obj.group))
                p.Nutrients.Add((string)obj.group, new Dictionary<string, double>());
            var name = ((string)obj.name);
            if (!p.Nutrients[(string)obj.group].ContainsKey(name)) 
                p.Nutrients[(string)obj.group].Add(name, value * (100.0 / weight));
        }

        private static void SetWeights(JArray array, string item, Product p) {
            var obj = ((dynamic)array.FirstOrDefault((i) => ((dynamic)i).name == item));
            if (obj != null) {
                JArray measures = JArray.FromObject(obj.measures);
                if (p.Weights == null)
                    p.Weights = new Dictionary<string, double>();
                measures.ToList().ForEach(measure => {
                    try {
                        if (!p.Weights.ContainsKey(((dynamic)measure).label.ToString().Replace('.', ',')))
                            p.Weights.Add(((dynamic)measure).label.ToString().Replace('.', ','),
                            double.Parse(((dynamic)measure).eqv.ToString()) / double.Parse(((dynamic)measure).qty.ToString()));
                    }
                    catch (Exception ex) {
                        Console.WriteLine("Failed parsing measure"+ measure.ToString() + ex.Message);
                    }
               });
            }
        }

        private static bool IsProductOutsideFoodgroup(Product product, HashSet<string> prodWords,
            HashSet<string> exceptionWords)
        {
            var hasProdWord = false;
            foreach (var str in prodWords)
            {
                if (product.NameContains(str))
                {
                    hasProdWord = true;
                    break;
                }
            }

            if (!hasProdWord)
            {
                return false;
            }

            foreach (var exStr in exceptionWords)
            {
                if (product.NameContains(exStr))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsMeatProduct(Product product)
        {
            var meatWords = new HashSet<string>() { "chicken", "beef", "duck", "pork" };
            // We need exception because of the duck egg product
            var exceptionWords = new HashSet<string>() { "egg" };

            return
                product.FoodGroup == "beef" ||
                product.FoodGroup == "chicken" ||
                product.FoodGroup == "pork" ||
                product.FoodGroup == "sausages" ||
                IsProductOutsideFoodgroup(product, meatWords, exceptionWords);
        }

        public static bool IsDairyProduct(Product product)
        {
            var dairyWords = new HashSet<string>() { "milk", "chocolate", "brownies", "cheese", "buttermilk", "mocha",
            "vanilla coffee", "ovaltine", "dannon", "shortening" };
            var exceptionWords = new HashSet<string>() { "almond milk", "coconut milk", "soy milk", "rice milk", "milkfish" };

            return
                (product.FoodGroup == "dairy" && !product.NameContains("egg")) ||
                (IsProductOutsideFoodgroup(product, dairyWords, exceptionWords));
        }

        public static bool IsFishProduct(Product product)
        {
            var fishWords = new HashSet<string>() { "fish" };

            return
                product.FoodGroup == "fish" ||
                IsProductOutsideFoodgroup(product, fishWords, new HashSet<string>());
        }

        public static bool IsSeafoodProduct(Product product)
        {
            var seafoodWords = new HashSet<string>() { "oyster", "crustaceans", "shrimp", "caviar", "roe" };
            // We need exception because of the oyster mushrooms, salsify, oyster blade products
            var exceptionWords = new HashSet<string>() { "mushrooms", "salsify", "oyster blade" };

            return
                IsProductOutsideFoodgroup(product, seafoodWords, exceptionWords);
        }

        public static bool IsFromAnimalProduct(Product product)
        {
            var animalWords = new HashSet<string>() { "egg" };

            return
                product.Types.Contains(ProductType.Meat) ||
                product.Types.Contains(ProductType.Dairy) ||
                product.Types.Contains(ProductType.Fish) ||
                product.Types.Contains(ProductType.Seafood) ||
                IsProductOutsideFoodgroup(product, animalWords, new HashSet<string>());
        }

        public static bool IsNonKosherProduct(Product product)
        {
            var nonkosherWords = new HashSet<string>() { "eel", "shark", "swrodfish", "camel", "rabbit", "horse", "donkey" };
            // We need exception because of horseradish, peel products
            var exceptionWords = new HashSet<string>() { "horseradish", "peel" };

            return
                product.FoodGroup == "pork" ||
                product.Types.Contains(ProductType.Seafood) ||
                IsProductOutsideFoodgroup(product, nonkosherWords, exceptionWords);
        }
    }
}
