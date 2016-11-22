using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MenuGraders
{
    class CaloriesCountGrader : PerDayGrader
    {
        public CaloriesCountGrader()
        {
            Description = "Calories count compared to the ideal calories count value";

            dailyGrader = new CaloriesCountDailyGrader();
        }
    }
}
