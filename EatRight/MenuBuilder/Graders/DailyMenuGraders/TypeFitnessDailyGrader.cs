using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.MealGraders;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class TypeFitnessDailyGrader : PerMealGrader
    {
        public TypeFitnessDailyGrader()
        {
            Description = "Fitness of products to meals type";

            mealGrader = new TypeFitnessMealGrader();
        }
    }
}
