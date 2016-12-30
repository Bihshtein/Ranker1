using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.RecoGraders
{
    class GradeAverageRecoGrader : RecoGrader
    {
        public GradeAverageRecoGrader()
        {
            Description = "Average of gardes of meals";
            Type = RestModel.GraderType.GradeAverageRecoGrader;
        }

        protected override double InternalGrade(Recommendation reco)
        {
            double gradeSum = 0;
            foreach (var mealWrapper in reco.MealsSet)
            {
                gradeSum += mealWrapper.Grade;
            }

            return gradeSum / ((double)reco.MealsSet.Count);
        }
    }
}
