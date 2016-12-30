using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.MealGraders
{
    class ServingsNumMealGrader : MealGrader
    {
        public int idealServingsNum;

        public ServingsNumMealGrader()
        {
            Description = "Compatibiliy of servings number to user's request";
            Type = GraderType.ServingsNumMealGrader;

            idealServingsNum = RecommendationObject.recommendationDB.idealServingsNum;
        }

        protected override double InternalGrade(Meal meal)
        {
            /* 
             * The ideal servings num will be graded 100.
             * If we missed by one we'll get 66.6, by two we'll get 33.3, and by more- 0.
             */
            int dist = Math.Abs(meal.Recipe.Servings - idealServingsNum);
            if (dist > 3)
            {
                return 0;
            }
            return (3 - dist) / 3.0;
        }
    }
}
