using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.MealGraders
{
    class PrepTimeMealGrader : MealGrader
    {
        public PrepTimeMealGrader()
        {
            Description = "How much time does it take to prepare the food";
            Type = GraderType.PrepTimeMealGrader;
        }

        protected override double InternalGrade(MenuMeal menuMeal)
        {
            // For 0-100 minutes, the grade will be 100 minus the number of minuter. For more, the grade will be 0
            return Math.Max((100.0 - menuMeal.Meal.PrepTime), 0.0) / 100.0;
        }
    }
}
