using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.MealGraders;
using RestModel;

namespace RecommendationBuilder.Graders.DailyMenuGraders
{
    class ServingsNumDailyGrader : PerMealGrader
    {
        public ServingsNumDailyGrader()
        {
            Description = "Compatibiliy of servings number to user's request";
            Type = GraderType.ServingsNumDailyGrader;

            mealGrader = new ServingsNumMealGrader();
        }
    }
}
