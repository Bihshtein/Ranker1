using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    abstract class PerDayGrader : Grader
    {
        public override double Grade(Menu menu)
        {
            int daysNumber = menu.GetDaysNumber();
            if (daysNumber == 0)
            {
                return 0;
            }

            int dayInd = 0;
            double gradeSum = 0;

            for (; dayInd < daysNumber; dayInd++)
            {
                gradeSum += GradeDay(menu.GetDay(dayInd));
            }

            return gradeSum / daysNumber;
        }

        protected abstract double GradeDay(DailyMenu day);
    }
}
