using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MenuGraders
{
    class VarietyGrader : PerDayGrader
    {
        public VarietyGrader()
        {
            Description = "Variety in products";

            dailyGrader = new VarietyDailyGrader();
        }
    }
}
