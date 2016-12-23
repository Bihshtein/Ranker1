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

namespace InitRecipes {
    class MatchProductsInRecipes {

        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, double> MeasuresRelativeSizes = new Dictionary<string, double>() {
            // relative to the weight stated by the USDA for the product
            { "slice ",0.5 },
            { "slices",0.5 },
            { "large",1.2 },
            { "small",0.8 },
            { "medium",1 },
            {"cup ",1 },
            { "cups ",1 }
        };
        public static Dictionary<string, double> MeasuresWeights = new Dictionary<string, double>(){
            {"bunch",60 },
            //{"cloves",3 },//garlic
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

        };

        public static int totalMissing = 0;
        public static int total = 0;
        public static int totalParsed = 0;
        public static RestDBInterface unit = new RestDBInterface();
        public static void AddActualProductsToMealsDB() {
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

        public static void ParseItem(Meal meal, string item) {
            var actualWeight = 0.0;
            var relativeWeight = 0.0;
            var parts = item.Split(MeasuresWeights.Keys.ToArray(), StringSplitOptions.None);
            if (parts.Length > 1) {
                var unit = item.Replace(parts[0], "").Replace(parts[1], "");
                actualWeight = ParseAmount(parts[0]) * MeasuresWeights[unit];
            }
            else {
                parts = item.Split(MeasuresRelativeSizes.Keys.ToArray(), StringSplitOptions.None);
                if (parts.Length > 1) {
                    var unit = item.Replace(parts[0], "").Replace(parts[1], "");
                    relativeWeight = ParseAmount(parts[0]) * MeasuresRelativeSizes[unit];
                }
            }

            ++total;
            if (parts.Length == 1)
                parts = Regex.Split(item, @"\d");
            if (parts.Length > 1) {
                ++totalParsed;
                var innerpart = parts[1].Split(',')[0].Trim();
                innerpart = innerpart.Replace("(optional)", "");
                innerpart = innerpart.Replace("(110 degrees f/45 degrees c)", "");
                var res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);

                if (res == null || res.Count == 0) {
                    log.Info(innerpart);
                    ++totalMissing;
                }
                else {
                    if (relativeWeight > 0)
                        actualWeight = res[0].Weight * relativeWeight;
                    if (meal.ProductsWeight == null)
                        meal.ProductsWeight = new Dictionary<string, double>();
                    if (meal.ProductsWeight.ContainsKey(innerpart))
                        meal.ProductsWeight[innerpart] += actualWeight;
                    else
                        meal.ProductsWeight.Add(innerpart, actualWeight);
                }
            }
        }

        private static double ParseAmount(string fraction) {
            fraction = fraction.Trim();

            if (!fraction.Contains('('))
                return GetFractionedNumber(fraction);
            else {
                var parts = fraction.Split('(');
                var fractioned = GetFractionedNumber(parts[0]);
                double relative;
                if (parts[1].Contains('.'))
                    relative = double.Parse("0" + parts[1]);
                else
                    relative = double.Parse(parts[1]);
                return fractioned * relative;
            }
          
        }

        private static double GetFractionedNumber(string fraction) {
            if (!fraction.Contains('/'))
                return double.Parse(fraction);
            else {
                var wholeAndFract = fraction.Split(' ');
                var num = 0;
                var fractIndex = 0;
                if (wholeAndFract.Length > 1) {
                    num = int.Parse(wholeAndFract[0]);
                    fractIndex = 1;
                }

                var fract = wholeAndFract[fractIndex].Split('/');
                return num + (double.Parse(fract[0]) / double.Parse(fract[1]));
            }
        }
    }
}
