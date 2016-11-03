using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class Menu
    {
        public Menu(List<Day> days)
        {
            this.days = days;
        }

        private List<Day> days;

        public int GetDaysNumber() { return days.Count; }
        public Day GetDay(int dayInx) { return days[dayInx]; }
    }
}
