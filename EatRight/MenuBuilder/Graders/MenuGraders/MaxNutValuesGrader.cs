using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace MenuBuilder.Graders.MenuGraders
{
    class MaxNutValuesGrader : PerDayGrader
    {
        public MaxNutValuesGrader()
        {
            Description = "Nutrition values compared to the maximum nutrition values";
            Type = GraderType.MaxNutValuesGrader;

            dailyGrader = new MaxNutValuesDailyGrader();
        }
    }
}
