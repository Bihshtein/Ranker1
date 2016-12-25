using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace MenuBuilder.Graders.MenuGraders
{
    class ServingsNumGrader : PerDayGrader
    {
        public ServingsNumGrader()
        {
            Description = "Compatibiliy of servings number to user's request";
            Type = GraderType.ServingsNumGrader;

            dailyGrader = new ServingsNumDailyGrader();
        }
    }
}
