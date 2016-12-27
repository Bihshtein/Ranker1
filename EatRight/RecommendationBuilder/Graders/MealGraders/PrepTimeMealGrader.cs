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

        protected override double InternalGrade(MealWrapper mealWrapper)
        {
            // For 0-100 minutes, the grade will be 100 minus the number of minutes. For more, the grade will be 0
            return Math.Max((100.0 - mealWrapper.Meal.PrepTime), 0.0) / 100.0;
        }
    }
}
