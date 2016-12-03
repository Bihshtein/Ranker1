using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace MenuBuilder.Graders.MenuGraders
{
    class NutValuesGrader : PerDayGrader
    {
        public NutValuesGrader()
        {
            Description = "Nutrition values compared to the ideal nutrition values";
            Type = GraderType.NutValuesGrader;

            dailyGrader = new NutValuesDailyGrader();
        }
    }
}
