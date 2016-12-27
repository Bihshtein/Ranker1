using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder
{
    public class Menu : RecommendationObject
    {
        public Menu(List<DailyMenu> days)
        {
            ID = IDCounter++;
            this.Days = days;
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Menu m = (Menu)obj;
            return ID.Equals(m.ID);
        }

        public List<DailyMenu> Days { get; private set; }

        public int GetDaysNumber() { return Days.Count; }
        public DailyMenu GetDay(int dayInx) { return Days[dayInx]; }
        public int ID { get; private set; }

        private static int IDCounter = 0;
    }
}
