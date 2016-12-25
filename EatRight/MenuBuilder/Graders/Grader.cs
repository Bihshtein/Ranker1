using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MenuBuilder.Graders.MenuGraders;
using MenuBuilder.Graders.DailyMenuGraders;
using MenuBuilder.Graders.MealGraders;

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

        public GraderType Type { protected set; get; }

        public static Grader GetGraderByType(GraderType type)
        {
            switch (type)
            {
                // Menu graders
                case GraderType.CaloriesCountGrader:
                    return new CaloriesCountGrader();
                case GraderType.CostGrader:
                    return new CostGrader();
                case GraderType.FoodCategoryGrader:
                    return new FoodCategoryGrader();
                case GraderType.MinNutValuesGrader:
                    return new MinNutValuesGrader();
                case GraderType.MaxNutValuesGrader:
                    return new MaxNutValuesGrader();
                case GraderType.ProductsTasteGrader:
                    return new ProductsTasteGrader();
                case GraderType.VarietyGrader:
                    return new VarietyGrader();
                case GraderType.PrepTimeGrader:
                    return new PrepTimeGrader();
                case GraderType.ServingsNumGrader:
                    return new ServingsNumGrader();

                // DailyMenu graders
                case GraderType.CaloriesCountDailyGrader:
                    return new CaloriesCountDailyGrader();
                case GraderType.FoodCategoryDailyGrader:
                    return new FoodCategoryDailyGrader();
                case GraderType.MinNutValuesDailyGrader:
                    return new MinNutValuesDailyGrader();
                case GraderType.MaxNutValuesDailyGrader:
                    return new MaxNutValuesDailyGrader();
                case GraderType.ProductsTasteDailyGrader:
                    return new ProductsTasteDailyGrader();
                case GraderType.VarietyDailyGrader:
                    return new VarietyDailyGrader();
                case GraderType.PrepTimeDailyGrader:
                    return new PrepTimeDailyGrader();
                case GraderType.ServingsNumDailyGrader:
                    return new ServingsNumDailyGrader();

                // Meal graders
                case GraderType.CaloriesCountMealGrader:
                    return new CaloriesCountMealGrader();
                case GraderType.MinNutValuesMealGrader:
                    return new MinNutValuesMealGrader();
                case GraderType.MaxNutValuesMealGrader:
                    return new MaxNutValuesMealGrader();
                case GraderType.FoodCategoryMealGrader:
                    return new FoodCategoryMealGrader();
                case GraderType.ProductsTasteMealGrader:
                    return new ProductsTasteMealGrader();
                case GraderType.PrepTimeMealGrader:
                    return new PrepTimeMealGrader();
                case GraderType.ServingsNumMealGrader:
                    return new ServingsNumMealGrader();

                default:
                    // TODO: better handle errors
                    System.Console.WriteLine("***ERROR*** Grader type " + type.ToString() + " is an unknown type!");
                    System.Environment.Exit(1);
                    return null;
            }
        }
    }

    public abstract class SuggestionRange
    {
        public int Length { get; set; }
        public int ServingsNum { get; set; } = 1;

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
        public MealType MealType { get; set; }

        public override bool IsMealSuggestionRange() { return true; }

        public override string ToString()
        {
            return "Meal suggestion: " + Length + " Meals";
        }
    }

    public class GraderDB
    {
        // Information unique for user (by age, weight etc.)
        public Dictionary<string, MinMaxDouble> dailyValues;
        public int dailyCaloriesNum;
        public Dictionary<string, double> productFlavorGrade; // Maps a product to a double in the range [-1,1] where -1 means dislike and 1 means like
        public Dictionary<MealCategory, double> mealCategoryGrade; // Maps a meal category to a double in the range [-1,1] where -1 means dislike and 1 means like
        public HashSet<string> forbiddenProducts; // All products that the user will never eat
        public Dictionary<GraderType, double> GradersWeight; // Maps grader type to the weight that this grader will have
        public int idealServingsNum;

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
