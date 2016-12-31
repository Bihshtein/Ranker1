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

namespace InitRecipes {
    class AddProducts {

        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public static int totalMissing = 0;
        public static int total = 0;
        public static int totalParsed = 0;
        public static RestDBInterface unit = new RestDBInterface();
        public static void Add() {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            
            var meals = unit.Recipes.GetAll().ToList();
            var foundProducts = new List<Product>();
            foreach (var meal in meals) {
                if (meal.ProductsWeight != null)
                    meal.ProductsWeight.Clear();
                meal.TotalCaloriesNum = 0;
                if (meal.TotalNutValues != null)
                {
                    meal.TotalNutValues.Clear();
                }
                else meal.TotalNutValues = new Dictionary<string, double>();

                foreach (var item in meal.Ingredients) {
                    ParseItem(meal, item.ToLower().Trim());
                }

                unit.Recipes.Update(s => s.ID, meal.ID, meal);
            }

            Console.WriteLine("total meals : " + meals.Count);
            Console.WriteLine("total ingredients : " + total);
            Console.WriteLine("total ingredients parsed : " + totalParsed);
            Console.WriteLine("total ingredients missed : " + totalMissing);
        }

        public static void ParseItem(Recipe meal, string item) {
            item = Map.GetPrettyItem(item);
            var results = ParseWeightAndName(item);
            var innerpart = results.Item1.Trim();
            var weight = results.Item2;
            var relativeMeasure = results.Item3.Trim();

            ++total;
            if (innerpart != string.Empty) {
                innerpart = Map.AdjustInnerPart(innerpart);
                ++totalParsed;
                var res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);

                if (res == null || res.Count == 0) {
                    log.Info(innerpart);
                    ++totalMissing;
                }
                else {
                    var product = res[0];
                   
                    if (relativeMeasure != string.Empty) {
                        weight = TryParseRelativeWeight(relativeMeasure,weight, product, innerpart);
                    }
                    AddItem(product, meal, weight, innerpart);
                }
            }
            else {
                log.Error(item);
            }
        }

        public static double TryParseRelativeWeight(string mes, double weight, Product prd, string fullName) {
            mes = ParseHelpers.GetWithoutLast_S_letter(mes);

            if (prd.Weights.ContainsKey(mes)) {
                return weight * prd.Weights[mes];
            }
            else if (prd.Weights.ContainsKey(fullName)) {
                return weight * prd.Weights[fullName];
            }
            else if (prd.Weights.Keys.Any(key => key.Contains(mes))) {
                return weight * prd.Weights[prd.Weights.Keys.First(key => key.Contains(mes))];
            }
            else if (Map.RecipeToUSDAMeasure.ContainsKey(mes)) {
                mes = Map.RecipeToUSDAMeasure[mes];
                if (prd.Weights.ContainsKey(mes)) {
                    return weight * prd.Weights[mes];
                }
                else if (prd.Weights.Keys.Any(key => key.Contains(mes))) {
                    return weight * prd.Weights[prd.Weights.Keys.First(key => key.Contains(mes))];
                }
                else if (prd.Weights.Keys.Any(key => key.Contains("serving"))) {
                    return weight * prd.Weights.First(key => key.Key.Contains("serving")).Value;
                }
            }
            if (prd.Weights.Keys.Any(key => key.Contains("cup"))) {
                return weight * prd.Weights.First(key => key.Key.Contains("cup")).Value;
            }
            else {
                var defaultMeasure = prd.Weights.First().Value;
                log.Error("measure not found : " + mes + ", returning default, measure : " + prd.Weights.First().Value);
                return weight * defaultMeasure;
            } 
           
            
        }


        public static Tuple<string, double, string> ParseByRelativeMeasures(string[] parts, string item)
        {

            var relativeWeight = 0.0;
            var innerpart = string.Empty;
            var unit = item.Replace(parts[0], "").Replace(parts[1], "");
            if (Map.RelativeSizes.Contains(unit))
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
                var units = item.Replace(innerpart, "");
                try
                {
                    relativeWeight = int.Parse(units.Trim());
                }
                catch (Exception ex)
                {
                    innerpart = string.Empty;
                    log.Error("Failed to parse relative weight for item : " + item, ex);
                }
            }
            return new Tuple<string, double>(innerpart, relativeWeight);
        }
        public static Tuple<string,double> ParseByAbsoluteMeasures(string[] parts, string item)
        {
            var actualWeight = 0.0;
            var innerpart = string.Empty;
          
                var unit = item.Replace(parts[0], "").Replace(parts[1], "");
                if (Map.MeasuresWeights.ContainsKey(unit))
                {
                    try
                    {
                        actualWeight = ParseHelpers.ParseAmount(parts[0]) * Map.MeasuresWeights[unit];
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
            var parts = item.Split(Map.MeasuresWeights.Keys.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                var res = ParseByAbsoluteMeasures(parts, item);
                innerpart = res.Item1;
                weight = res.Item2;
            }
            else
            {
                parts = item.Split(Map.RelativeSizes.ToArray(), StringSplitOptions.RemoveEmptyEntries);
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

        public static void AddItem(Product product, Recipe meal,double weight, string innerpart)
        {
            if (meal.ProductsWeight == null)
                meal.ProductsWeight = new Dictionary<string, double>();
            if (meal.ProductsWeight.ContainsKey(innerpart))
                meal.ProductsWeight[innerpart] += weight;
            else
                meal.ProductsWeight.Add(innerpart, weight);

            var prodNutValues = product.Nutrients().ToList();

            foreach (var entry in prodNutValues)
            {
                if (!meal.TotalNutValues.ContainsKey(entry.Key))
                {
                    meal.TotalNutValues[entry.Key] = 0;
                }

                double curValue = meal.TotalNutValues[entry.Key];
                meal.TotalNutValues[entry.Key] = curValue + (entry.Value * (weight / Formulas.DefaultGrams));
            }

            meal.TotalCaloriesNum += Formulas.GetTotalCalories(weight, product.Protein, product.Fat, product.Carbs);
        }

       
    }
}
