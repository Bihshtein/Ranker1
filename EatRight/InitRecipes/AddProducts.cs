using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Linq.Expressions;
using Logic;
using System.IO;

namespace InitRecipes {
    class AddProducts {

        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\";


        public static int totalMissing = 0;
        private static object Locker = new object();
        public static int total = 0;
        public static RestDBInterface unit = new RestDBInterface();
        public static void Add() {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;

            var recipes = unit.Recipes.GetAllList();
            var tasks = new List<Task>();
            recipes.ForEach(r => tasks.Add(new Task(new Action(()=>AddRecipe(r)))));
            var foundProducts = new List<Product>();
            tasks.ForEach(task => task.Start());
            tasks.ForEach(task => task.Wait());

            Console.WriteLine("total recipes : " + recipes.Count);
            Console.WriteLine("total ingredients : " + total);
            Console.WriteLine("total ingredients missed : " + totalMissing);
            var sorted = MissingCount.OrderByDescending(i => i.Value).ToList();
            File.WriteAllLines(FolderPath + "MissingIndex.txt", sorted.ConvertAll<string>(i => i.Key + " : " + i.Value));
        }

        private static void AddRecipe(Recipe recipe) {
            if (recipe.ProductsWeight != null)
                recipe.ProductsWeight.Clear();
            recipe.TotalCaloriesNum = 0;
            if (recipe.TotalNutValues != null) {
                recipe.TotalNutValues.Clear();
            }
            else recipe.TotalNutValues = new Dictionary<string, double>();

            if (recipe.USDAProducts == null) recipe.USDAProducts = new Dictionary<string, List<string>>();
            recipe.USDAProducts.Clear();

            foreach (var item in recipe.Ingredients) {
                ParseItem(recipe, item.ToLower().Trim());
            }

            unit.Recipes.Update(s => s.ID, recipe.ID, recipe);
            unit.TestsRecipes.Update(s => s.ID, recipe.ID, recipe);
        }

        private static void ParseInnerpart(Recipe recipe, List<Product> res, string innerpart,
            string relativeMeasure, double weight)
        {
            var product = res[0];

            recipe.ProductTypes.UnionWith(product.Types);

            var retProducts = new List<string>();

            var usdaKey = string.Format("{0} => <0/{3}> [{1}] {2}", innerpart, product.ID, product.USDAString, res.Count);

            res.ForEach(x => retProducts.Add(string.Format("[{0}] {1}", x.ID, x.USDAString)));

            if (recipe.USDAProducts.ContainsKey(usdaKey))
            {
                Console.WriteLine("Warning: Recipe {0} contains USDA Product duplicate {1}", recipe.ID, innerpart);
            }
            else
            {
                recipe.USDAProducts.Add(usdaKey,retProducts);
            }

            if (relativeMeasure != string.Empty)
            {
                if (relativeMeasure.Contains(innerpart))
                    relativeMeasure = innerpart;
                weight = TryParseRelativeWeight(relativeMeasure, weight, product, innerpart);
            }


            AddItem(product, recipe, weight, innerpart);
        }

        public static Dictionary<string, int> MissingCount = new Dictionary<string, int>();

        public static void ParseItem(Recipe recipe, string item) {
            item = Map.AdjustNames(item);
            var unifiedInnerpart = "";
            item = Map.AdjustInnerPart(item, ref unifiedInnerpart);
            var results = ParseWeightAndName(item);
            var innerpart = results.Item1.Trim();
            var weight = results.Item2;
            var relativeMeasure = results.Item3. Trim();
            if (innerpart.Contains("to taste"))
                innerpart = innerpart.Replace("to taste", "");
            ++total;
            if (innerpart != string.Empty) {

                var res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);

                var oldInnerpart = innerpart;
                if (unifiedInnerpart.Length > 0 && (res == null || res.Count == 0))
                {
                    innerpart = unifiedInnerpart;
                    res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);
                }

                if (res == null || res.Count == 0)
                {
                    innerpart = oldInnerpart;
                    var hasAnd = false;
                    var hasOr = false;
                    if (innerpart.Contains(" and "))
                    {
                        hasAnd = true;
                    }
                    if (innerpart.Contains(" or "))
                    {
                        hasOr = true;
                    }
                    if (hasAnd || hasOr) // Try parsing the two parts seperated
                    {
                        var splitStr = new string[] { " and " };
                        if (hasOr)
                        {
                            splitStr = new string[] { " or " };
                        }
                        string[] innerparts = innerpart.Split(splitStr, StringSplitOptions.None);
                        total += (hasAnd ? (innerparts.Length - 1) : 1);
                        var parsed = false;
                        foreach (var itervar in innerparts)
                        {
                            res = Queries<Product>.GetMatchingProductsForIngredient(itervar);
                            if (res == null || res.Count == 0)
                            {
                                if (hasAnd)
                                {
                                    log.Error(itervar + " : " + item);
                                    ++totalMissing;
                                }
                            }
                            else
                            {
                                ParseInnerpart(recipe, res, itervar, relativeMeasure, weight);
                                if (hasOr)
                                {
                                    parsed = true;
                                    break;
                                }
                            }
                        }
                        if (hasOr && !parsed)
                        {
                            lock (Locker) {
                                if (MissingCount.ContainsKey(innerpart))
                                    MissingCount[innerpart]++;
                                else
                                    MissingCount.Add(innerpart, 1);
                                ++totalMissing;
                            }
                        }
                    }
                    else
                    {
                        lock (Locker) {
                            if (MissingCount.ContainsKey(innerpart))
                                MissingCount[innerpart]++;
                            else
                                MissingCount.Add(innerpart, 1);
                            ++totalMissing;
                        }
                    }
                }
                else
                {
                    ParseInnerpart(recipe, res, innerpart, relativeMeasure, weight);
                }
            }
            else {
                lock (Locker) {
                    if (MissingCount.ContainsKey(item))
                        MissingCount[item]++;
                    else
                        MissingCount.Add(item, 1);
                    ++totalMissing;
                }
            }
        }

        public static double TryParseRelativeWeight(string mes, double weight, Product prd, string fullName) {
            var noSMes = ParseHelpers.GetWithoutLast_S_letter(mes);
            var keys = prd.Weights.Keys;
            var mesPartsList = mes.Split(' ').ToList();
            if (prd.Weights.ContainsKey(mes)) {
                return weight * prd.Weights[mes];
            }

            else if (prd.Weights.ContainsKey(noSMes)) {
                return weight * prd.Weights[noSMes];
            }
            else if (prd.Weights.ContainsKey(fullName)) {
                return weight * prd.Weights[fullName];
            }

            else if (keys.Any(key => key.Contains(mes))) {
                return weight * prd.Weights[keys.First(key => key.Contains(mes))];
            }
        
            var matchingWord = MatchAnyWord(mesPartsList, keys.ToList());
            if (matchingWord == null)
                matchingWord = "cup";
            if (keys.Any(key => key.Contains(matchingWord))) {
                return weight * prd.Weights.First(key => key.Key.StartsWith(matchingWord)).Value;
            }
            else if (Map.RecipeToUSDAMeasure.ContainsKey(mes)) {
                mes = Map.RecipeToUSDAMeasure[mes];
                if (prd.Weights.ContainsKey(mes)) {
                    return weight * prd.Weights[mes];
                }
                else if (keys.Any(key => key.Contains(mes))) {
                    return weight * prd.Weights[keys.First(key => key.Contains(mes))];
                }
                else if (keys.Any(key => key.Contains("serving"))) {
                    return weight * prd.Weights.First(key => key.Key.Contains("serving")).Value;
                }
            }
          
            var defaultMeasure = prd.Weights.First().Value;
            log.Debug("measure not found : " + mes + ", returning default, measure : " + prd.Weights.First().Value);
            return weight * defaultMeasure;
        }

        private static string MatchAnyWord(List<string>  list1, List<string> list2) {
            foreach (var part1 in list1) {
                var res = list2.FirstOrDefault(part2 => part2.StartsWith(part1));
                if (res != null)
                    return res;
            }

            foreach (var part1 in list2) {
                var res = list1.FirstOrDefault(part2 => part2.StartsWith(part1));
                if (res != null)
                    return part1;
            }
            return null;
        }

        public static Tuple<string, double, string> ParseByRelativeMeasures(string[] parts, string item)
        {

            var relativeWeight = 0.0;
            var innerpart = string.Empty;
            var unit = item.Replace(parts[0], "").Replace(parts[1], "");
            if (Formulas.RelativeSizes.Contains(unit))
            {
                try
                {
                    relativeWeight = ParseHelpers.ParseAmount(parts[0]);                    
                }
                catch (Exception ex)
                {
                    log.Error("Failed to parse relative weight for item : " + item, ex);
                }
                innerpart = parts[1];
            }
            else
                log.Error("Failed to parse relative weight for item : " + item);
            return new Tuple<string, double,string>(innerpart, relativeWeight,unit);
        }
        public static Tuple<string, double> ParseByRelativeNumber(string[] parts, string item)
        {
            var relativeWeight = 0.0;
            var innerpart = string.Empty;
            if (item != Regex.Replace(item, @"\d", ""))
            {
                innerpart = Regex.Replace(item, @"\d", "").Trim();
                if (innerpart != string.Empty) {
                    var units = item.Replace(innerpart, "");
                    try {
                        relativeWeight = int.Parse(units.Trim());
                    }
                    catch (Exception ex) {
                        innerpart = string.Empty;
                        log.Error("Failed to parse relative weight for item : " + item, ex);
                    }
                }
            }
            return new Tuple<string, double>(innerpart, relativeWeight);
        }
        public static Tuple<string,double> ParseByAbsoluteMeasures(string[] parts, string item)
        {
            var actualWeight = 0.0;
            var innerpart = string.Empty;
          
                var unit = item.Replace(parts[0], "").Replace(parts[1], "");
                if (Formulas.MeasuresWeights.ContainsKey(unit))
                {
                    try
                    {
                        actualWeight = ParseHelpers.ParseAmount(parts[0]) * Formulas.MeasuresWeights[unit];
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to parse actual weight for item : " + item, ex);
                    }
                    innerpart = parts[1];

                }
                else
                    log.Error("Failed to parse actual weight for item : " + item);
            return new Tuple<string, double>(innerpart, actualWeight);
        }
        public static Tuple<string,double, string> ParseWeightAndName(string item)
        {
            var weight = 0.0;
            var innerpart = "";
            var weightKey = "";
            var parts = item.Split(Formulas.MeasuresWeights.Keys.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                var res = ParseByAbsoluteMeasures(parts, item);
                innerpart = res.Item1;
                weight = res.Item2;
            }
            else
            {
                parts = item.Split(Formulas.RelativeSizes.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    var res = ParseByRelativeMeasures(parts, item);
                    innerpart = res.Item1;
                    weight = res.Item2;
                    weightKey = res.Item3;
                }
                else
                {
                    var res = ParseByRelativeNumber(parts, item);
                    innerpart = res.Item1;
                    weight = res.Item2;
                    weightKey = res.Item1; // the product is the actual key
                }
            }
            return new Tuple<string, double, string>(innerpart, weight, weightKey);
        }   

        public static void AddItem(Product product, Recipe recipe,double weight, string innerpart)
        {
            if (recipe.ProductsWeight == null)
                recipe.ProductsWeight = new Dictionary<string, double>();
            if (recipe.ProductsWeight.ContainsKey(innerpart))
                recipe.ProductsWeight[innerpart] += weight;
            else
                recipe.ProductsWeight.Add(innerpart, weight);

            var prodNutValues = product.NutrientsList().ToList();

            foreach (var entry in prodNutValues)
            {
                if (!recipe.TotalNutValues.ContainsKey(entry.Key))
                {
                    recipe.TotalNutValues[entry.Key] = 0;
                }

                double curValue = recipe.TotalNutValues[entry.Key];
                recipe.TotalNutValues[entry.Key] = curValue + (entry.Value * (weight / Formulas.DefaultGrams));
            }

            recipe.TotalCaloriesNum += Formulas.GetTotalCalories(weight,
                    product.Nutrients["Proximates"]["Protein"],
                    product.Nutrients["Proximates"]["Total lipid (fat)"],
                    product.Nutrients["Proximates"]["Carbohydrate, by difference"]);
        }

       
    }
}
