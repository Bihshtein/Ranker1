using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder
{
    public abstract class RecommendationObject
    {
        public double Grade { set; get; }
        public GradeInfo GradeInfo { set; get; }

        public static RecommendationDB recommendationDB = null;
    }
}
