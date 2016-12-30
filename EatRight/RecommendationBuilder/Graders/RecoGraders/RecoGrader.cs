using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Graders.RecoGraders
{
    abstract class RecoGrader : Grader
    {
        public override double Grade(RecommendationObject obj)
        {
            if (!(obj is Recommendation))
            {
                throw new ArgumentException();
            }

            return InternalGrade((Recommendation)obj);
        }

        abstract protected double InternalGrade(Recommendation reco);
    }
}
