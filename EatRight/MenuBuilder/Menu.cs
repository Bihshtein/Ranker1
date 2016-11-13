using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class Menu : GradableObject
    {
        public Menu(List<DailyMenu> days)
        {
            this.days = days;
        }

        private List<DailyMenu> days;

        public int GetDaysNumber() { return days.Count; }
        public DailyMenu GetDay(int dayInx) { return days[dayInx]; }
    }
}
