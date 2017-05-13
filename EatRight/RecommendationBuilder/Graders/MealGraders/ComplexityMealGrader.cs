using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.MealGraders
{
    class ComplexityMealGrader : MealGrader
    {
        public ComplexityMealGrader()
        {
            Description = "How much complex it is to prepare the food";
            Type = GraderType.ComplexityMealGrader;
        }

        protected override double InternalGrade(Meal meal)
        {
            // For 1 step, the grade will be 100. Each step will cost 25 points
            if (meal.Recipe.StepsNum > 5)
            {
                return 0;
            }
            if (meal.Recipe.StepsNum < 1)
            {
                return 100;
            }
            return 100 - (meal.Recipe.StepsNum - 1) * 25;
        }
    }
}
