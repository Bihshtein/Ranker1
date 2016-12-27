using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecommendationBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace RecommendationBuilder.Graders.MenuGraders
{
    class PrepTimeGrader : PerDayGrader
    {
        public PrepTimeGrader()
        {
            Description = "How much time does it take to prepare the food";
            Type = GraderType.PrepTimeGrader;

            dailyGrader = new PrepTimeDailyGrader();
        }
    }
}
