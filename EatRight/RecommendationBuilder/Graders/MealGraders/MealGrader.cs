using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.MealGraders
{
    abstract class MealGrader : Grader
    {
        public override double Grade(RecommendationObject obj)
        {
            if (!(obj is MealWrapper))
            {
                throw new ArgumentException();
            }

            return InternalGrade((MealWrapper)obj);
        }

        abstract protected double InternalGrade(MealWrapper meal);
    }
}
