using RestModel.Validators;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RestModel {
    public class USDA
    {
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
    }
  

    public class ProductBuilder {
        public static Product GetProduct(string groupName, int id, string name, JArray nutrients) {
            var validator = USDA.Validators[groupName];
            var p = new Product() { ID = id, Types = new HashSet<ProductType>() };
            p.FoodGroup = groupName.ToLower();
            p.USDAString = name;
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
            if (IsFromAnimalProduct(p)) // Must run thie after IsMeatProduct, IsDairyProduct, IsFishProduct, IsSeafoodproduct
            {
                p.Types.Add(ProductType.FromAnimal);
            }
            return p;
        }

        public static Product GetProductFromString(string searchQuery)
        {
            // TODO: Find out how to seperate the search query
            searchQuery = Regex.Replace(searchQuery.Replace(",", " "), @"\s+", " ");
            var groupAndSearchQuery = ExtractFoodGroupAndSearchQuery(searchQuery.Split(' '));
            var group = groupAndSearchQuery.Item1;
            var fixSearchQuery = String.Join(",", groupAndSearchQuery.Item2);
            return GetProduct(group, 0, fixSearchQuery, null);
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

        public static IEnumerable<IEnumerable<T>> GetPermutationIndexes<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutationIndexes(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }

        protected static List<string[]> GetPermutations(string[] searchQueryWords)
        {
            if ((searchQueryWords == null) || (searchQueryWords.Length <= 1)) return new List<string[]> { searchQueryWords };

            var indexArray = Enumerable.Range(0, searchQueryWords.Length - 1);
            var indexArray2 = Enumerable.Range(0, searchQueryWords.Length);

            var permutationList = new List<string[]>();

            permutationList.Add(new string[] { string.Join(" ", searchQueryWords) });
            for (int i = 1; i < indexArray2.Count(); i++)
            {
                foreach (var x in GetPermutationIndexes(indexArray, indexArray2.ElementAt(i)))
                {
                    var seps = x.ToList();
                    string partialString = string.Empty;
                    List<string> curPermutation = new List<string>();

                    for (int indx = 0; indx < searchQueryWords.Length; indx++)
                    {
                        if (partialString != string.Empty)
                            partialString += " ";
                        partialString += searchQueryWords[indx];
                        if (seps.Contains(indx)) { curPermutation.Add(partialString); partialString = string.Empty; }
                    }

                    if (partialString != string.Empty)
                        curPermutation.Add(partialString);

                    if (curPermutation.Count > 0)
                        permutationList.Add(curPermutation.ToArray());
                }
            }
            return permutationList;

            /*
            'frozen peas on fire'

            frozen, peas on fire
            frozen, peas, on fire
            frozen, peas, on, fire
            frozen peas, on fire
            frozen peas, on, fire
            frozen peas on, fire
            frozen peas on fire

            seperatosr = { } zero positions
            seperators = { {0}, {1}, {2} } one position
            seperators = { {0, 1}, {1, 2} } two positions
            seperators = { {0, 1, 2} } three positions


              */
            // [ a,b,c,d]
            // [ 
            // [ a,b,c,d ]
            // [ a,b,c d ]
            // [ a b,c d ]
            // [ a b,c,d ]
            // [ a b c,d ]
            // [ a b c d ]
        }

        /// <summary>
        /// TODO: from all permutations => go through all validators and get the best of score
        /// </summary>
        /// <param name="searchQueryWords"></param>
        /// <returns></returns>
        public static Tuple<string, string[]> ExtractFoodGroupAndSearchQuery(string[] searchQuery)
        {
            if ((searchQuery == null) || (searchQuery.Length <= 0))
                return new Tuple<string, string[]>(string.Empty, null);

            var validatorsByScore = new SortedList<double, Tuple<string, string[]>>(new InvertedDuplicateComparer<double>());

            var searchQueryPermutations = GetPermutations(searchQuery);
            if ((searchQueryPermutations == null) ||
                (searchQueryPermutations.Count == 0)) return new Tuple<string, string[]>(string.Empty, null);

            foreach (var validator in USDA.Validators)
            {
                var valid = validator;
                foreach (var searchQueryWords in searchQueryPermutations)
                {
                    var score = 1.0;
                    int mainPart, secondPart, thirdPart, validPart;
                    mainPart = secondPart = thirdPart = validPart = 0;

                    foreach (var sqWord in searchQueryWords)
                    {
                        if ((mainPart == 0) && (validator.Value.IsExactlyMainPart(sqWord))) mainPart++;
                        else if ((secondPart == 0) && (validator.Value.IsExactlySecondPart(sqWord))) secondPart++;
                        else if ((thirdPart == 0) && (validator.Value.IsExactlyThirdPart(sqWord))) thirdPart++;
                    //    else if (validator.Value.IsValidPart(sqWord)) validPart++;
                    }

                    if ((mainPart + secondPart + thirdPart) == 0)
                        score = 1.0;
                    else
                        score = ((mainPart + secondPart + thirdPart + validPart) / (double)searchQueryWords.Length) * 100;

                    if ((score == 1.0) && (validatorsByScore.Count > 0))
                        continue;
                    validatorsByScore.Add(score, new Tuple<string, string[]>(validator.Key, searchQueryWords));
                }
            }

            return validatorsByScore.First().Value;
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
                p.Name3 = item;
                p.Name3 = p.Name3.ToLower();
            }
        }

        private static void SetNutrientProperties(JArray nutrients, Product p) {
            p.Protein = GetNutrient(nutrients, "Protein");   //203
            p.Sugar = GetNutrient(nutrients, "Sugars, total");//269
            p.Fat = GetNutrient(nutrients, "Total lipid (fat)");//204
            p.Fiber = GetNutrient(nutrients, "Fiber, total dietary");//291
            p.Carbs = GetNutrient(nutrients, "Carbohydrate, by difference");//205
            p.VitaminC = GetNutrient(nutrients, "Vitamin C, total ascorbic acid");//401
            p.Thiamin = GetNutrient(nutrients, "Thiamin");//404
            p.Riboflavin = GetNutrient(nutrients, "Riboflavin");//405
            p.Niacin = GetNutrient(nutrients, "Niacin");//406
            p.PantothenicAcid = GetNutrient(nutrients, "Pantothenic acid");//410
            p.VitaminB6 = GetNutrient(nutrients, "Vitamin B-6");//415
            p.VitaminD = GetNutrient(nutrients, "Vitamin D");//328
            p.VitaminB12 = GetNutrient(nutrients, "Vitamin B-12");//418
            p.Folate = GetNutrient(nutrients, "Folate, total");//417
            p.VitaminA = GetNutrient(nutrients, "Vitamin A, IU");//318
            p.VitaminE = GetNutrient(nutrients, "Vitamin E (alpha-tocopherol)");//323
            p.VitaminK = GetNutrient(nutrients, "Vitamin K (phylloquinone)");//430
            p.Calcium = GetNutrient(nutrients, "Calcium, Ca");//301
            p.Iron = GetNutrient(nutrients, "Iron, Fe");//303
            p.Magnesium = GetNutrient(nutrients, "Magnesium, Mg");//304
            p.Phosphorus = GetNutrient(nutrients, "Phosphorus, P");//305
            p.Potassium = GetNutrient(nutrients, "Potassium, K");//306
            p.Sodium = GetNutrient(nutrients, "Sodium, Na");//307
            p.Zinc = GetNutrient(nutrients, "Zinc, Zn");//309
        }

        private static double GetNutrient(JArray array, string item) {
            var obj = ((dynamic)array.FirstOrDefault((i) => ((dynamic)i).name == item));
            if (obj == null)
                return 0;
            double value,weight = 0;
            double.TryParse(obj.measures[0].value.ToString(), out value);
            double.TryParse(obj.measures[0].eqv.ToString(), out weight);
            return value * (100.0 / weight);
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
            var nonkosherWords = new HashSet<string>() { "eel", "shark", "swrodfish" };

            return
                // TODO: Add more, this is currently not functional
                IsProductOutsideFoodgroup(product, nonkosherWords, new HashSet<string>());
        }
    }
}
