﻿using RestModel;
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
        public static int totalAdded = 0;
        public static int totalWeightsNotFound = 0;
        private static object Locker = new object();
        public static int total = 0;
        public static RestDBInterface unit = new RestDBInterface();


        public static void DumpDebug() {
            var missingRecipes = File.ReadAllLines(FolderPath + "InnerRecipes.txt");
            Console.WriteLine("total added recipes : " + totalAdded);
            Console.WriteLine("total ingredients : " + total);
            Console.WriteLine("total ingredients missed : " + totalMissing);
            Console.WriteLine("total weights not found: " + totalWeightsNotFound);
            var sorted = MissingCount.OrderBy(i => i.Key.Split(' ').Length).ToList();
            sorted.RemoveAll(i => i.Value < 5);
            sorted.RemoveAll(i => missingRecipes.Contains(i.Key));
            File.WriteAllLines(FolderPath + "MissingIndex.csv", sorted.ConvertAll<string>(i => i.Key + " , " + i.Value));
            sorted = MissingWeightsCount.OrderBy(i => i.Key.Split(' ').Length).ToList();
            File.WriteAllLines(FolderPath + "MissingWeights.csv", sorted.ConvertAll<string>(i => i.Key + " , " + i.Value));
        }


        public static int CurrId = 0;

        public static void AddWeights(Recipe recipe) {
            recipe.ProductsWeight = new Dictionary<string, double>();
            foreach (var item in recipe.Ingredients) {
                ParseItem(recipe, item);
            }
            recipe.CalculateProperties();

            if (recipe.ProductsWeight != null && recipe.Ingredients != null &&
                recipe.ProductsWeight.Count == recipe.Ingredients.Count &&
                recipe.ImageUrl != "http://img.food.com/fdc/img/FDC_share-logo.png" &&// empty photo food.com 
                recipe.ImageUrl != null) {
                lock (Locker) {
                    try {
                        recipe.ID = CurrId++;
                        unit.Recipes.Add(recipe);
                        ++totalAdded;
                    }
                    catch (Exception ex) {
                        log.Error(ex);
                        CurrId--;
                        totalAdded--;
                    }
                }
            }
              
        }

        private static List<Product> ReduceProductList(List<Product> productList, string additionalInfo)
        {
            var resList = new List<Product>();
            if (additionalInfo.Length > 0)
            {
                foreach (var product in productList)
                {
                    if (product.NameContains(additionalInfo))
                    {
                        resList.Add(product);
                    }
                }
            }

            return (resList.Count != 0) ? resList : productList;
        }

        private static Product SummarizeProductList(List<Product> productList)
        {
            // Special case: if the product list is of size one, save time and return that product
            if (productList.Count == 1)
            {
                return productList[0];
            }

            Dictionary<string, KeyValuePair<double, int>> nutrientsSumAndCount = new Dictionary<string, KeyValuePair<double, int>>();
            KeyValuePair<double, int> sumCountPair = new KeyValuePair<double, int>(0,0);

            // First get the nutrients sum and count
            foreach (var product in productList)
            {
                foreach (var nutPair in product.NutrientsList())
                {
                    if (nutrientsSumAndCount.TryGetValue(nutPair.Key, out sumCountPair))
                    {
                        nutrientsSumAndCount[nutPair.Key] = new KeyValuePair<double, int>(sumCountPair.Key + nutPair.Value, sumCountPair.Value + 1);
                    }
                    else
                    {
                        nutrientsSumAndCount[nutPair.Key] = new KeyValuePair<double, int>(nutPair.Value, 1);
                    }
                }
            }

            // Now, get the nutrients average
            Dictionary<string, double> nutrientsAverage = new Dictionary<string, double>();
            foreach (var entry in nutrientsSumAndCount)
            {
                nutrientsAverage[entry.Key] = entry.Value.Key / (double)entry.Value.Value;
            }

            // And finally, estimate how much each product is far from the averages
            double minDistance = Double.PositiveInfinity;
            Product closestProduct = null;
            foreach (var product in productList)
            {
                double distance = 0;
                Dictionary<string, double> nutrientsAverageCopy = new Dictionary<string, double>(nutrientsAverage);

                // Go over all the product's nutrients
                foreach (var nutPair in product.NutrientsList())
                {
                    distance += Math.Abs(nutPair.Value - nutrientsAverageCopy[nutPair.Key]);
                    nutrientsAverageCopy.Remove(nutPair.Key);
                }

                // Check if there are some missing nutrients
                foreach (var entry in nutrientsAverageCopy)
                {
                    distance += entry.Value;
                }

                // Check if this is the minimal distance product so far
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestProduct = product;
                }
            }

            return closestProduct;
        }

        private static Product GetmatchingProductFromList(List<Product> productList, IngredientInfo info)
        {
            productList = ReduceProductList(productList, info.AdditionalInfo);
            return SummarizeProductList(productList);
        }

        private static void ParseInnerpart(Recipe recipe, List<Product> res, string innerpart,
            string relativeMeasure, double weight, IngredientInfo info)
        {
            var product = GetmatchingProductFromList(res, info);
            if (relativeMeasure != string.Empty) {
                if (relativeMeasure.Contains(innerpart))
                    relativeMeasure = innerpart;
            }
            else if (weight == 1) {
                relativeMeasure = innerpart;
            }
            weight = TryParseRelativeWeight(relativeMeasure, weight, ref product, innerpart, res);

            AddItem(product, recipe, weight, innerpart);
        }

        public static Dictionary<string, int> MissingCount = new Dictionary<string, int>();
        public static Dictionary<string, int> MissingWeightsCount = new Dictionary<string,int>();

        public static void ParseItem(Recipe recipe, IngredientInfo info)
        {
            string innerpart = info.Name;
            string relativeMeasure = info.ReltiveSizeMeasure;
            double weight = info.Quantity;

            ++total;
            if (innerpart == "")
            {
                return;
            }
            
            var res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);
            if (res != null && res.Count > 0)
            {
                ParseInnerpart(recipe, res, innerpart, relativeMeasure, weight, info);
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

        public static double TryParseRelativeWeight(string measure, double weight, ref Product prd, string fullName, List<Product> prodList)
        {
            Product origPrd = prd;
            int resInd = 0;
            var mes = ParseHelpers.GetWithoutLast_S_letter(measure);
            var keys = prd.Weights.Keys;

            while (true)
            {
                if (measure == "")
                {
                    if (weight == 0)
                        return prd.Weights.First().Value;
                    else
                        return weight;
                }
                mes = ParseHelpers.GetWithoutLast_S_letter(measure);
                keys = prd.Weights.Keys;
                var mesPartsList = mes.Split(' ').ToList();
                if (prd.Weights.ContainsKey(mes))
                {
                    return weight * prd.Weights[mes];
                }

                else if (prd.Weights.ContainsKey(fullName))
                {
                    return weight * prd.Weights[fullName];
                }

                else if (keys.Any(key => key.Contains(mes)))
                {
                    return weight * prd.Weights[keys.First(key => key.Contains(mes))];
                }

                var matchingWord = MatchAnyWord(mesPartsList, keys.ToList());
                if (matchingWord != null && keys.Any(key => key.Contains(matchingWord)))
                {
                    return weight * prd.Weights.First(key => key.Key.Contains(matchingWord)).Value;
                }
                else if (Map.RecipeToUSDAMeasure.ContainsKey(mes))
                {
                    mes = Map.RecipeToUSDAMeasure[mes];
                    if (prd.Weights.ContainsKey(mes))
                    {
                        return weight * prd.Weights[mes];
                    }
                    else if (keys.Any(key => key.Contains(mes)))
                    {
                        return weight * prd.Weights[keys.First(key => key.Contains(mes))];
                    }
                    else if (keys.Any(key => key.Contains("serving")))
                    {
                        return weight * prd.Weights.First(key => key.Key.Contains("serving")).Value;
                    }

                }

                if (Map.MeasureToMeasure.ContainsKey(mes))
                {
                    var ratio = Map.MeasureToMeasure[mes].Item2;
                    mes = Map.MeasureToMeasure[mes].Item1;
                    if (prd.Weights.ContainsKey(mes))
                    {
                        return weight * prd.Weights[mes] * ratio;
                    }
                }
                if (Formulas.MeasuresWeights.ContainsKey(mes))
                {
                    return weight * Formulas.MeasuresWeights[mes];
                }
                if (keys.Any(key => key.Contains("medium")))
                {
                    return weight * prd.Weights.First(key => key.Key.Contains("medium")).Value;
                }

                if (prodList.Count > 1 && prodList.Count > resInd)
                {
                    prd = prodList[resInd];
                    resInd++;
                }
                else
                {
                    break;
                }
            }

            prd = origPrd;
            mes = ParseHelpers.GetWithoutLast_S_letter(measure);
            keys = prd.Weights.Keys;

            var defaultMeasure = prd.Weights.First().Value;
            var str = "measures:";
            keys.ToList().ForEach(k => str += k + ',');
            str += "recipe_measure:" + mes;
            lock (Locker) {
                if (MissingWeightsCount.ContainsKey(str)) {
                    MissingWeightsCount[str]++;
                }
                else {
                    MissingWeightsCount.Add(str, 1);
                }
            }
            ++totalWeightsNotFound;
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
        }

       
    }
}
