using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using System.Reflection;

namespace InitRecipes {
    class MatchProductsInRecipes {
    
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static Dictionary<string, string> RecipeToNutrientDictionary = new Dictionary<string, string> {
            { "white sugar","granulated sugar"},
            { "all-purpose flour","wheat flour"},
            { "kosher salt","table salt"},
        };

        public static List<string> CutDetails = new List<string> { "sprig", "sprigs", "ground", "shredded", "cubed", "head", "heads", "sliced", "stalk", "stalks", "diced", "minced", "chopped" };
        public static List<string> ServeDetails = new List<string> {"warm", "cooked", "fresh" };
        public static List<string> PackDetails = new List<string> { "can","package","packages" };
        public static string[] AmountsSplitArray = {
            "slice ", "bunch", "cloves", "pinch", "slices", "large", "small", "medium", "teaspoon ",
            "teaspoons", "tablespoons", "tablespoon ", "pound ", "pound)", "pounds ", "ounces", "ounce ", "ounce)", "cup ", "cups "
        };

        public static int totalMissing = 0;
        public static int total = 0;
        public static int totalParsed = 0;
        public static RestDBInterface unit = new RestDBInterface();
        public static void AddActualProductsToMealsDB() {
           


            var meals = unit.Meals.GetAll().ToList();
            var foundProducts = new List<Product>();
            foreach (var meal in meals) {
                foreach (var item in meal.Ingredients) {
                        ParseItem(item);
                    }
                }

            Console.WriteLine("total meals : " + meals.Count);
            Console.WriteLine("total ingredients : " + total);
            Console.WriteLine("total ingredients parsed : " + totalParsed);
            Console.WriteLine("total ingredients missed : " + totalMissing);
        }

        public static void ParseItem(string item) {
          
            var parts = item.ToLower().Split(AmountsSplitArray, StringSplitOptions.None);
            ++total;
            if (parts.Length == 1)
                parts = Regex.Split(item, @"\d");
            if (parts.Length > 1) {
                ++totalParsed;
                var innerpart = parts[1].Split(',')[0].Trim();
                if (RecipeToNutrientDictionary.ContainsKey(innerpart))
                    innerpart = RecipeToNutrientDictionary[innerpart];
                var innerSplit = innerpart.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                List<Product> res = null;
                if (innerSplit.Length == 1)
                    res = HandleSingleWordName(innerSplit[0]);
                else if (innerSplit.Length == 2)
                    res = HandleDoubleWordName(innerSplit[0], innerSplit[1]);
                else if (innerSplit.Length == 3)
                    res = HandleTripleWordName(innerSplit[0], innerSplit[1], innerSplit[2]);
                else if (innerSplit.Length == 4 )
                    res = HandleQuatroWordName(innerSplit[0], innerSplit[1], innerSplit[2], innerSplit[3]);

                if (res == null || res.Count == 0) {
                    log.Info(innerpart);
                    ++totalMissing;
                }
            }
        }


        public static List<Product> HandleSingleWordName(string name) {
            return unit.Products.Queries.TryMatchWholeProduct(name);
        }

        public static List<Product> HandleDoubleWordName(string part1, string part2) {
            if (CutDetails.Any(cut => cut.Equals(part1)) || ServeDetails.Any(cook => cook.Equals(part1)))
                return HandleSingleWordName(part2);
            else
                return unit.Products.Queries.TryMatchWholeProduct(part1, part2);
        }

        public static List<Product> HandleTripleWordName(string part1, string part2, string part3) {
            if (CutDetails.Any(cut => cut.Equals(part1)) || PackDetails.Any(pack => pack.Equals(part1)))
                return HandleDoubleWordName(part2, part3);
            else
                return unit.Products.Queries.TryMatchWholeProduct(part1, part2, part3);
        }

        public static List<Product> HandleQuatroWordName(string part1, string part2, string part3, string part4) {
            if (PackDetails.Any(pack => pack.Equals(part1)))
                return HandleTripleWordName(part2, part3, part4);
            else
                return null;
        }
    }
}
