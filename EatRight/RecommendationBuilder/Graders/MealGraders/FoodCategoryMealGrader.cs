using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.MealGraders
{
    class FoodCategoryMealGrader : TasteMealGrader<MealCategory>
    {
        public FoodCategoryMealGrader()
        {
            Description = "Compatibility of meal category to the user flavor";
            Type = GraderType.FoodCategoryMealGrader;
        }

        protected override void InitDataStructures(Meal meal)
        {
            flavorDict = RecommendationObject.recommendationDB.mealCategoryGrade;
            if (meal.Recipe.Categories != null)
            {
                objectList = meal.Recipe.Categories.ToList();
            }
        }
    }
}
