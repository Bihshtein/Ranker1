using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.MealGraders
{
    class FoodCategoryMealGrader : TasteMealGrader<MealCategory>
    {
        public FoodCategoryMealGrader()
        {
            Description = "Compatibility of meal category to the user flavor";
        }

        protected override void InitDataStructures(MenuMeal meal)
        {
            flavorDict = graderDB.mealCategoryGrade;
            if (meal.Meal.Categories != null)
            {
                objectList = meal.Meal.Categories.ToList();
            }
        }
    }
}
