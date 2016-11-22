﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.MealGraders;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class FoodCategoryDailyGrader : PerMealGrader
    {
        public FoodCategoryDailyGrader()
        {
            Description = "Compatibility of meals category to the user flavor";

            mealGrader = new FoodCategoryMealGrader();
        }
    }
}
