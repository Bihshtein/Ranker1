using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.MealGraders
{
    class StepsNumMealGrader : MealGrader
    {
        public StepsNumMealGrader()
        {
            Description = "How many preperation steps does the meal take";
            Type = GraderType.StepsNumMealGrader;
        }

        protected override double InternalGrade(Meal meal)
        {
            // For 1 step, the grade will be 100. Each step will cost 25 points
            if (meal.Recipe.StepsNum > 10)
            {
                return 0;
            }
            if (meal.Recipe.StepsNum < 1)
            {
                return 0.1;
            }
            return (10 - (meal.Recipe.StepsNum - 1)) / 10;
        }
    }
}
