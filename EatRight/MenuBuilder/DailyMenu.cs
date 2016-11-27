using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    public class DailyMenu : GradableObject
    {
        public DailyMenu()
        {
            ID = IDCounter++;
            Meals = new Dictionary<MealType, MenuMeal>();
        }

        public DailyMenu(Dictionary<MealType, MenuMeal> meals)
        {
            ID = IDCounter++;
            Meals = meals;
        }

        public Dictionary<MealType, MenuMeal> Meals { get; private set; }
        public int ID { get; private set; }

        public List<string> GetAllProducts()
        {
            var resList = new List<string>();
            Meals.Values.ToList().ForEach(x => resList.AddRange(x.Meal.Products.ToList()));
            return resList;
        }

        private static int IDCounter = 0;
    }
}
