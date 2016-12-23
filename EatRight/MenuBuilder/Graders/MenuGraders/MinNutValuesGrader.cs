using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace MenuBuilder.Graders.MenuGraders
{
    class MinNutValuesGrader : PerDayGrader
    {
        public MinNutValuesGrader()
        {
            Description = "Nutrition values compared to the minimum nutrition values";
            Type = GraderType.MinNutValuesGrader;

            dailyGrader = new MinNutValuesDailyGrader();
        }
    }
}
