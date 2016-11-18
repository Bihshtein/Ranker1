using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class DailyMenu : GradableObject
    {
        public DailyMenu()
        {
            ID = IDCounter++;
        }

        public MenuMeal Breakfast { get; set; }
        public MenuMeal Lunch { get; set; }
        public MenuMeal Dinner { get; set; }
        public int ID { get; private set; }

        public List<string> GetAllProducts()
        {
            var bp = Breakfast.Meal.Products.ToList();
            var lp = Lunch.Meal.Products.ToList();
            var dp = Dinner.Meal.Products.ToList();
            return bp.Concat(lp).Concat(dp).ToList();
        }

        private static int IDCounter = 0;
    }
}
