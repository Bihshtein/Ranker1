using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace MenuBuilder.Graders.MenuGraders
{
    class VarietyGrader : PerDayGrader
    {
        public VarietyGrader()
        {
            Description = "Variety in products";
            Type = GraderType.VarietyGrader;

            dailyGrader = new VarietyDailyGrader();
        }
    }
}
