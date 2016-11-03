using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class Day
    {
        public Day(Breakfast b, Lunch l, Dinner d)
        {
            breakfast = b;
            lunch = l;
            dinner = d;
        }

        private Breakfast breakfast;
        public Breakfast Breakfast { get { return breakfast; } }

        private Lunch lunch;
        public Lunch Lunch { get { return lunch; } }

        private Dinner dinner;
        public Dinner Dinner { get { return dinner; } }
    }
}
