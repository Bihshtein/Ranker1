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

    public abstract class SuggestionRange
    {
        public int Length { get; set; }

        public virtual bool IsMenuSuggestionRange() { return false; }
        public virtual bool IsMealSuggestionRange() { return false; }

        public static SuggestionRange SingleDay()
        {
            return new MenuSuggestionRange() { Length = 1 };
        }

        public static SuggestionRange WholeWeek()
        {
            return new MenuSuggestionRange() { Length = 7 };
        }

        public static SuggestionRange OneMeal()
        {
            return new MealSuggestionRange() { Length = 1 };
        }
    }

    public class MenuSuggestionRange : SuggestionRange
    {
        public List<MealType> MealsInDailyMenu { get; set; }

        public override bool IsMenuSuggestionRange() { return true; }

        public MenuSuggestionRange()
        {
            MealsInDailyMenu = null;
        }

        public override string ToString()
        {
            string res = "Menu suggestion: " + Length + " Days";
            if (MealsInDailyMenu != null)
            {
                res += ", {";
                bool first = true;
                foreach (var mealType in MealsInDailyMenu)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res += ", ";
                    }
                    res += mealType.ToString();
                }
                res += "} in each day";
            }
            return res;
        }
    }

    public class MealSuggestionRange : SuggestionRange
    {
        public override bool IsMealSuggestionRange() { return true; }

        public override string ToString()
        {
            return "Meal suggestion: " + Length + " Meals";
        }
    }

    public class GraderDB
    {
        // Information unique for user (by age, weight etc.)
        public Dictionary<string, double> dailyValues;
        public int dailyCaloriesNum;
        public Dictionary<string, double> productFlavorGrade; // Maps a product to a double in the range [-1,1] where -1 means dislike and 1 means like
        public Dictionary<MealCategory, double> mealCategoryGrade; // Maps a meal category to a double in the range [-1,1] where -1 means dislike and 1 means like

        // Information chosen by the user
        public SuggestionRange range; // How many days/meals will the menu contain
        public double budget = Double.PositiveInfinity; // Specific user's budget, default is infinity (no budget)

        public override string ToString()
        {
            var retStr = "GraderDB: \n";

            var dLines = dailyValues.Select(x => x.Key + ": " + x.Value.ToString());
            retStr += "DailyValues: \n" + string.Join(Environment.NewLine, dLines) + "\n\n";
            retStr += "Daily Calories: " + dailyCaloriesNum + "\n";
            retStr += "Suggestion: " + range + "\n";

            return retStr;
        }
    }
}
