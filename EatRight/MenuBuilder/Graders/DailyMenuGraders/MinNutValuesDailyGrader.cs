using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class MinNutValuesDailyGrader : NutValuesDailyGrader
    {
        public MinNutValuesDailyGrader():
            base()
        {
            Type = GraderType.MinNutValuesDailyGrader;
            IsMinGrader = true;
        }
    }
}
