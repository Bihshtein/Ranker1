using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class DailyMenu
    {
        public DailyMenu(Meal b, Meal l, Meal d)
        {
            breakfast = b;
            lunch = l;
            dinner = d;
        }

        private Meal breakfast;
        public Meal Breakfast { get { return breakfast; } }

        private Meal lunch;
        public Meal Lunch { get { return lunch; } }

        private Meal dinner;
        public Meal Dinner { get { return dinner; } }
    }
}
