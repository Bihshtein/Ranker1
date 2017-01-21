using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder
{
    public class GraderInfo
    {
        public GraderType Type { get; set; }
        public double Weight { get; set; }
        public double Grade { get; set; }
    }

    public class GradeInfo
    {
        public List<string> BestGraders { get; set; }
        public List<string> WorstGraders { get; set; }

        public HashSet<GraderInfo> GradersInfo { get; set; }
        public Dictionary<string, double> MinNutrientGrades { get; set; }
        public Dictionary<string, double> MaxNutrientGrades { get; set; }
    }
}
