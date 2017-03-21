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
            var sorted = MissingCount.OrderBy(i => i.Key.Split(' ').Length).ToList();
            File.WriteAllLines(FolderPath + "MissingIndex.txt", sorted.ConvertAll<string>(i => i.Key + " : " + i.Value));
        }

        private static void AddRecipe(Recipe recipe)
        {
            if (recipe.ProductsWeight != null)
            {
                recipe.ProductsWeight.Clear();
            }
            recipe.TotalCaloriesNum = 0;
            if (recipe.TotalNutValues != null)
            {
                recipe.TotalNutValues.Clear();
            }
            else
            {
                recipe.TotalNutValues = new Dictionary<string, double>();
            }

            foreach (var item in recipe.Ingredients)
            {
                ParseItem(recipe, item.Item1.ToLower().Trim(),item.Item3,item.Item2);
            }
            try
            {
                unit.Recipes.Update(s => s.ID, recipe.ID, recipe);
                unit.TestsRecipes.Update(s => s.ID, recipe.ID, recipe);
            }
            catch (Exception ex)
            {
                log.Error("failed to update recipe " + recipe.ID, ex);
            }
        }

        private static void ParseInnerpart(Recipe recipe, List<Product> res, string innerpart,
            string relativeMeasure, double weight)
        {
            var product = res[0];
            if (relativeMeasure != string.Empty)
            {
                if (relativeMeasure.Contains(innerpart))
                    relativeMeasure = innerpart;
                weight = TryParseRelativeWeight(relativeMeasure, weight, product, innerpart);
            }

            AddItem(product, recipe, weight, innerpart);
        }

        public static Dictionary<string, int> MissingCount = new Dictionary<string, int>();

        public static void ParseItem(Recipe recipe, string innerpart, string relativeMeasure, double weight)
        {
            if (innerpart == "")
            {
                return;
            }
            ++total;         
            var res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);
            if (res != null && res.Count > 0)
            {
                ParseInnerpart(recipe, res, innerpart, relativeMeasure, weight);
            }
            else
            {
                lock (Locker)
                {
                    if (MissingCount.ContainsKey(innerpart))
                    {
                        MissingCount[innerpart]++;
                    }
                    else
                    {
                        MissingCount.Add(innerpart, 1);
                    }
                }
                ++totalMissing;
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
                return weight * prd.Weights.First(key => key.Key.Contains(matchingWord)).Value;
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
