using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MenuGraders
{
    class NutValuesGrader : PerDayGrader
    {
        public NutValuesGrader()
        {
            Description = "Nutrition values compared to the ideal nutrition values";

            dailyGrader = new NutValuesDailyGrader();
        }
    }
}
