using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders
{
    abstract class Grader
    {
        /**
         * Returns a double between 0 and 1.
         */
        public abstract double Grade(GradableObject obj);

        public static GraderDB graderDB = null;

        public string Description { protected set; get; }
    }

    public enum SuggestionRangeType
    {
        Days,
        Meals
    }

    public class SuggestionRange
    {
        public SuggestionRangeType Type { get; set; }
        public int Length { get; set; }

        public static SuggestionRange SingleDay()
        {
            return new SuggestionRange() { Type = SuggestionRangeType.Days, Length = 1 };
        }

        public static SuggestionRange WholeWeek()
        {
            return new SuggestionRange() { Type = SuggestionRangeType.Days, Length = 7 };
        }

        public static SuggestionRange OneMeal()
        {
            return new SuggestionRange() { Type = SuggestionRangeType.Meals, Length = 1 };
        }
    }

    public class GraderDB
    {
        // Information unique for user (by age, weight etc.)
        public Dictionary<string, double> dailyValues;
        public int dailyCaloriesNum;

        // Information chosen by the user
        public SuggestionRange range; // How many days/meals will the menu contain
    }
}
