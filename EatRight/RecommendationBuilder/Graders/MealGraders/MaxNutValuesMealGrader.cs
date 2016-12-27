using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using RecommendationBuilder.Graders.DailyMenuGraders;

namespace RecommendationBuilder.Graders.MealGraders
{
    class MaxNutValuesMealGrader : NutValuesMealGrader
    {
        public MaxNutValuesMealGrader():
            base()
        {
            Type = GraderType.MaxNutValuesMealGrader;
            IsMinGrader = false;
        }
    }
}
