using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.MealGraders;
using RestModel;

namespace RecommendationBuilder.Graders.DailyMenuGraders
{
    class FoodCategoryDailyGrader : PerMealGrader
    {
        public FoodCategoryDailyGrader()
        {
            Description = "Compatibility of meals category to the user flavor";
            Type = GraderType.FoodCategoryDailyGrader;

            mealGrader = new FoodCategoryMealGrader();
        }
    }
}
