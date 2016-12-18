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
        public static List<string> CutDetails = new List<string> { "sprig", "sprigs", "ground", "shredded", "cubed", "head", "heads", "sliced", "stalk", "stalks", "diced", "minced", "chopped" };
        public static List<string> CookDetails = new List<string> { "cooked", "fresh" };
        public static List<string> PackDetails = new List<string> { "can" };
        public static RestDBInterface unit = new RestDBInterface();
        public static void AddActualProductsToMealsDB() {
            int totalMissing = 0;
            int total = 0;
            int totalParsed = 0;
            var list = new string[] { "slice ", "bunch", "cloves", "whole", "pinch", "slices", "large", "small", "medium", "teaspoon ", "teaspoons", "tablespoons", "tablespoon ", "pound ", "pound)", "pounds ", "ounces", "ounce ", "ounce)", "cup ", "cups " };


            var meals = unit.Meals.GetAll().ToList();
            var foundProducts = new List<Product>();
            foreach (var meal in meals) {
                foreach (var item in meal.Ingredients) {
                    var parts = item.ToLower().Split(list, StringSplitOptions.None);
                    ++total;
                    if (parts.Length == 1)
                        parts = Regex.Split(item, @"\d");
                    if (parts.Length > 1) {
                        ++totalParsed;
                        var innerparts = parts[1].Split(',');
                        var innerSplit = innerparts[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        List<Product> res = null;
                        if (innerSplit.Length == 1)
                            res = HandleSingleWordName(innerSplit[0]);
                        else if (innerSplit.Length == 2)
                            res = HandleDoubleWordName(innerSplit[0], innerSplit[1]);
                        else if (innerSplit.Length == 3)
                            res = HandleTripleWordName(innerSplit[0], innerSplit[1], innerSplit[2]);

                        if (res != null && res.Count > 0) {
                            log.Info(innerparts[0]);
                            ++totalMissing;
                        }
                    }
                }
            }
            Console.WriteLine("total meals : " + meals.Count);
            Console.WriteLine("total ingredients : " + total);
            Console.WriteLine("total ingredients parsed : " + totalParsed);
            Console.WriteLine("total ingredients missed : " + totalMissing);
        }


        public static List<Product> HandleSingleWordName(string name) {
            return unit.Products.Queries.TryMatchWholeProduct(name);
        }

        public static List<Product> HandleDoubleWordName(string part1, string part2) {
            if (CutDetails.Any(cut => cut.Equals(part1)) || CookDetails.Any(cook => cook.Equals(part1)))
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
    }
}
