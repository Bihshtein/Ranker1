using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.MealGraders
{
    class PrepTimeMealGrader : MealGrader
    {
        public PrepTimeMealGrader()
        {
            Description = "How much time does it take to prepare the food";
            Type = GraderType.PrepTimeMealGrader;
        }

        protected override double InternalGrade(Meal meal)
        {
            // For 0-180 minutes, the grade will be 100 minus the number of minutes. For more, the grade will be 0
            return Math.Max((new TimeSpan(3, 0, 0) - meal.Recipe.PrepTime).TotalMinutes, new TimeSpan(0).Minutes) / 180.0;
        }
    }
}
