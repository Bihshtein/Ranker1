using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace RecommendationBuilder.Graders.MenuGraders
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
