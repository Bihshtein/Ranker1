﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Graders.MenuGraders
{
    abstract class MenuGrader : Grader
    {
        public override double Grade(RecommendationObject obj)
        {
            if (!(obj is Menu))
            {
                throw new ArgumentException();
            }

            return InternalGrade((Menu)obj);
        }

        abstract protected double InternalGrade(Menu menu);
    }
}
