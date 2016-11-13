using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class DailyMenu : GradableObject
    {
        public MenuMeal Breakfast { get; set; }
        public MenuMeal Lunch { get; set; }
        public MenuMeal Dinner { get; set; }
    }
}
