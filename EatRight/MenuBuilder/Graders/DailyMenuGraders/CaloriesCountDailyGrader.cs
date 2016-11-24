using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder.Graders.DailyMenuGraders
{
    class CaloriesCountDailyGrader : DailyMenuGrader
    {
        private int dailyCaloriesNum;

        public CaloriesCountDailyGrader()
        {
            Description = "Calories count compared to the ideal calories count value";

            this.dailyCaloriesNum = Grader.graderDB.dailyCaloriesNum;
        }

        protected override double InternalGrade(DailyMenu day)
        {
            double caloriesNum = 0;
            day.Meals.ForEach(x => caloriesNum += x.CaloriesNum);

            return GradeRatio(caloriesNum / dailyCaloriesNum);
        }

        static private double GradeRatio(double ratio)
        {
            if (ratio <= 1)
            {
                return ratio;
            }

            if (ratio <= 2)
            {
                return 2 - ratio;
            }

            return 0;
        }
    }
}
