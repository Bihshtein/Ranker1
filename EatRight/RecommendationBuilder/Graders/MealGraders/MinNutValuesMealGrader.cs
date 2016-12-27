using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using RecommendationBuilder.Graders.DailyMenuGraders;

namespace RecommendationBuilder.Graders.MealGraders
{
    class MinNutValuesMealGrader : NutValuesMealGrader
    {
        public MinNutValuesMealGrader():
            base()
        {
            Type = GraderType.MinNutValuesMealGrader;
            IsMinGrader = true;
        }
    }
}
