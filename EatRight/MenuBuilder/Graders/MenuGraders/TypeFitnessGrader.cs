using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MenuGraders
{
    class TypeFitnessGrader : PerDayGrader
    {
        public TypeFitnessGrader()
        {
            Description = "Fitness of products to meals type";

            dailyGrader = new TypeFitnessDailyGrader();
        }
    }
}
