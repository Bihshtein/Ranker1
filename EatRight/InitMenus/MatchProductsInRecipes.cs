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
                var res = Queries<Product>.GetMatchingProductsForIngredient(innerpart);

                if (res == null || res.Count == 0) {
                    log.Info(innerpart);
                    ++totalMissing;
                }
            }
        }
     
    }
}
