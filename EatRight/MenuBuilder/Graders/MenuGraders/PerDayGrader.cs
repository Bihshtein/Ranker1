using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;

namespace MenuBuilder.Graders.MenuGraders
{
    abstract class PerDayGrader : MenuGrader
    {
        protected override double InternalGrade(Menu menu)
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
                try
                {
                    gradeSum += dailyGrader.Grade(menu.GetDay(dayInd));
                }
                catch
                {
                    throw;
                }
            }

            return gradeSum / daysNumber;
        }

        protected DailyMenuGrader dailyGrader;
    }
}
