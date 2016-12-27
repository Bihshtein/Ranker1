using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace RecommendationBuilder.Graders.MenuGraders
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
