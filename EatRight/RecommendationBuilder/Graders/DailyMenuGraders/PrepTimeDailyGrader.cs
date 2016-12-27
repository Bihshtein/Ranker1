using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.MealGraders;
using RestModel;

namespace RecommendationBuilder.Graders.DailyMenuGraders
{
    class PrepTimeDailyGrader : PerMealGrader
    {
        public PrepTimeDailyGrader()
        {
            Description = "How much time does it take to prepare the food";
            Type = GraderType.PrepTimeDailyGrader;

            mealGrader = new PrepTimeMealGrader();
        }
    }
}
