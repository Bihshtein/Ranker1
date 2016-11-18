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
            this.Days = days;
        }

        public List<DailyMenu> Days { get; private set; }

        public int GetDaysNumber() { return Days.Count; }
        public DailyMenu GetDay(int dayInx) { return Days[dayInx]; }
    }
}
