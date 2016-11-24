using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    public abstract class GradableObject
    {
        public double Grade { set; get; }
        public GradeInfo GradeInfo { set; get; }
    }
}
