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
            Meals = new List<MenuMeal>();
        }

        public DailyMenu(List<MenuMeal> meals)
        {
            ID = IDCounter++;
            Meals = meals;
        }

        public List<MenuMeal> Meals { get; private set; }
        public int ID { get; private set; }

        public List<string> GetAllProducts()
        {
            var resList = new List<string>();
            Meals.ForEach(x => resList.AddRange(x.Meal.Products.ToList()));
            return resList;
        }

        private static int IDCounter = 0;
    }
}
