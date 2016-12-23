using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class MaxNutValuesDailyGrader : NutValuesDailyGrader
    {
        public MaxNutValuesDailyGrader():
            base()
        {
            Type = GraderType.MaxNutValuesDailyGrader;
            IsMinGrader = false;
        }
    }
}
