using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.MealGraders;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class TasteDailyGrader : PerMealGrader
    {
        public TasteDailyGrader()
        {
            mealGrader = new TasteMealGrader();
        }
    }
}
