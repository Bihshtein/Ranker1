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
        public GraderInfo(GraderType type, double weight, double grade)
        {
            Type = type;
            Weight = weight;
            Grade = grade;
        }

        public GraderType Type { get; private set; }
        public double Weight { get; private set; }
        public double Grade { get; private set; }
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
