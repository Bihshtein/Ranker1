using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder
{
    abstract class Grader
    {
        /**
         * Returns a double between 0 and 1.
         */
        public abstract double Grade(Menu menu);

        public static GraderDB graderDB = null;
    }

    public class GraderDB
    {
        public Dictionary<string, double> dailyValues;
        public int dailyCaloriesNum;
    }
}
