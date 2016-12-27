using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace RecommendationBuilder.Graders.MenuGraders
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
