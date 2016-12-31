using RecommendationBuilder.Graders;
using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder
{
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

    public class RecommendationDB
    {
        // Information unique for user (by age, weight etc.)
        public Dictionary<string, MinMaxDouble> dailyValues;
        public int dailyCaloriesNum;
        public Dictionary<string, double> productFlavorGrade; // Maps a product to a double in the range [-1,1] where -1 means dislike and 1 means like
        public Dictionary<RecipeCategory, double> recipeCategoryGrade; // Maps a recipe category to a double in the range [-1,1] where -1 means dislike and 1 means like
        public HashSet<string> forbiddenProducts; // All products that the user will never eat
        public int idealServingsNum;
        public List<NutritionDietType> dietRestrictions;

        public Dictionary<GraderType, double> GradersWeight; // Maps grader type to the weight that this grader will have
        public HashSet<FilterType> FiltersSet; // Set of filters for current user

        // Information chosen by the user
        public SuggestionRange range; // How many days/meals will the menu contain
        public double budget = Double.PositiveInfinity; // Specific user's budget, default is infinity (no budget)
        public HashSet<UserPreference> preferences; // Preferences for meal, for example- meat
    }
}
