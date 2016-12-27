using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Graders.DailyMenuGraders
{
    abstract class DailyMenuGrader : Grader
    {
        public override double Grade(RecommendationObject obj)
        {
            if (!(obj is DailyMenu))
            {
                throw new ArgumentException();
            }

            return InternalGrade((DailyMenu)obj);
        }

        abstract protected double InternalGrade(DailyMenu dailyMenu);
    }
}
