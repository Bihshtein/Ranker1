using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders
{
    abstract class Grader
    {
        /**
         * Returns a double between 0 and 1.
         */
        public abstract double Grade(GradableObject obj);

        public static GraderDB graderDB = null;
    }

    public class GraderDB
    {
        // Information unique for user (by age, weight etc.)
        public Dictionary<string, double> dailyValues;
        public int dailyCaloriesNum;

        // Information chosen by the user
        public int menuDaysNum; // How many days will the menu contain
    }
}
