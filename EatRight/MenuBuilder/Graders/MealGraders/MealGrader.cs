using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.MealGraders
{
    abstract class MealGrader : Grader
    {
        public override double Grade(GradableObject obj)
        {
            if (!(obj is MenuMeal))
            {
                throw new ArgumentException();
            }

            return InternalGrade((MenuMeal)obj);
        }

        abstract protected double InternalGrade(MenuMeal meal);
    }
}
