using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MenuGraders
{
    class FoodCategoryGrader : PerDayGrader
    {
        public FoodCategoryGrader()
        {
            Description = "Compatibility of meals category to the user flavor";

            dailyGrader = new FoodCategoryDailyGrader();
        }
    }
}
