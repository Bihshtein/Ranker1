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

        public static Dictionary<string, double> MeasuresRelativeSizes = new Dictionary<string, double>() {
            // relative to the weight stated by the USDA for the product
            { "slice ",0.5 },
            { "slices",0.5 },
            { "large",1.2 },
            { "small",0.8 },
            { "medium",1 },
            {"cup ",1 },
            {"cup small",1 },
            { "cups ",1 }
        };
        public static Dictionary<string, double> MeasuresWeights = new Dictionary<string, double>(){
            {"bunch",60 },
            {"cloves",3 },//garlic
            {"pinch",0.25 },//green onion or whatever
            {"teaspoon ",5 },
            {"teaspoons",5 },
            {"tablespoons",14 },
            {"tablespoon ",14 },
            {"pound ",453.5},
            {"pound)",453.5},
            {"pounds ",453.5},
            {"ounces",28.3 },
            {"ounce ",28.3 },
            {"ounce)",28.3 },
            {"fluid ounce)",28.3 },
            {"fluid ounce ",28.3 },
            {"fluid ounces",28.3 },

        };

        public static int totalMissing = 0;
        public static int total = 0;
        public static int totalParsed = 0;
        public static RestDBInterface unit = new RestDBInterface();
        public static void Add() {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;

            var meals = unit.Meals.GetAll().ToList();
            var foundProducts = new List<Product>();
            foreach (var meal in meals) {
                if (meal.ProductsWeight != null)
                    meal.ProductsWeight.Clear();
                foreach (var item in meal.Ingredients) {
                    ParseItem(meal, item.ToLower().Trim());
                }
                unit.Meals.Update(s => s.ID, meal.ID, meal);
            }

            Console.WriteLine("total meals : " + meals.Count);
            Console.WriteLine("total ingredients : " + total);
            Console.WriteLine("total ingredients parsed : " + totalParsed);
            Console.WriteLine("total ingredients missed : " + totalMissing);
        }

        public static Dictionary<string, string> SimpleNames = new Dictionary<string, string>() {
            { "skinless, boneless chicken","chicken"},
            { "fresh mozzarella, cut into small cubes", "mozzarella" },
            { "leaves", "leaf" },
            { "greek-style", "greek" },
        };
        public static Tuple<string, double> ParseByRelativeMeasures(string[] parts, string item)
        {

            var relativeWeight = 0.0;
            var innerpart = string.Empty;
            var unit = item.Replace(parts[0], "").Replace(parts[1], "");
            if (MeasuresRelativeSizes.ContainsKey(unit))
            {
                try
                {
                    relativeWeight = Expressions.ParseAmount(parts[0]) * MeasuresRelativeSizes[unit];
                }
                catch (Exception ex)
                {
                    log.Error("Failed to parse relative weight for item : " + item, ex);
                }
                innerpart = parts[1];
            }
            else
                log.Error("Failed to parse relative weight for item : " + item);
            return new Tuple<string, double>(innerpart, relativeWeight);
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
                if (MeasuresWeights.ContainsKey(unit))
                {
                    try
                    {
                        actualWeight = Expressions.ParseAmount(parts[0]) * MeasuresWeights[unit];
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

        public static Tuple<string,double, bool> ParseWeightAndName(string item)
        {
            var weight = 0.0;
            var isRelativeWeight = true;
            var innerpart = "";

            var parts = item.Split(MeasuresWeights.Keys.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                var res = ParseByAbsoluteMeasures(parts, item);
                innerpart = res.Item1;
                weight = res.Item2;
                isRelativeWeight = false;
            }
            else
            {
                parts = item.Split(MeasuresRelativeSizes.Keys.ToArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 1)
                {
                    var res = ParseByRelativeMeasures(parts, item);
                    innerpart = res.Item1;
                    weight = res.Item2;
                }
                else
                {
                    var res = ParseByRelativeNumber(parts, item);
                    innerpart = res.Item1;
                    weight = res.Item2;
                }
            }
            return new Tuple<string, double, bool>(innerpart, weight, isRelativeWeight);
        }

    public static void ParseItem(Meal meal, string item) {
        item = GetPrettyItem(item);
        var results = ParseWeightAndName(item);
        var innerpart = results.Item1;   
          
        ++total;
        if (innerpart != string.Empty) {
            innerpart = AdjustInnerPart(innerpart);
            var res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);

            if (res == null || res.Count == 0) {
                log.Info(innerpart);
                ++totalMissing;
            }
            else 
                AddItem(res[0], meal, results.Item3, results.Item2, innerpart);
        }
        else {
            log.Error(item);
        }
    }

        public static string GetPrettyItem(string item)
        {
            SimpleNames.Keys.ToList().ForEach(key => {
                if (item.Contains(key))
                    item = item.Replace(key, SimpleNames[key]);
            });
            return item;
        }

        public static string AdjustInnerPart(string innerpart)
        {
            var split = innerpart.Split(',');
            if (split[0] != string.Empty)
                innerpart = split[0];
            else
                innerpart = split[1];
            ++totalParsed;
            innerpart = innerpart.Replace("(optional)", "");
            innerpart = innerpart.Replace("(110 degrees f/45 degrees c)", "");
            return innerpart;
        }

        public static void AddItem(Product product, Meal meal, bool isRelativeWeight, double weight, string innerpart)
        {
            if (isRelativeWeight)
                weight = product.Weight * weight;
            if (meal.ProductsWeight == null)
                meal.ProductsWeight = new Dictionary<string, double>();
            if (meal.ProductsWeight.ContainsKey(innerpart))
                meal.ProductsWeight[innerpart] += weight;
            else
                meal.ProductsWeight.Add(innerpart, weight);
        }

       
    }
}
