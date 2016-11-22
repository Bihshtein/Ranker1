using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder.Graders.MealGraders
{
    class TasteMealGrader : MealGrader
    {
        public TasteMealGrader()
        {
            Description = "Compatibility to the user flavor";
        }

        protected override double InternalGrade(MenuMeal meal)
        {
            double totalFlavorGrade = 0;
            foreach (var product in meal.Meal.Products)
            {
                if (graderDB.productFlavorGrade.ContainsKey(product))
                {
                    totalFlavorGrade += graderDB.productFlavorGrade[product];
                }
            }

            int productsNum = meal.Meal.Products.Count;
            double scaledTotalFlavorGrade = totalFlavorGrade + productsNum;
            double maxFlavorGrade = 2 * productsNum;

            return scaledTotalFlavorGrade / maxFlavorGrade;
        }
    }
}
