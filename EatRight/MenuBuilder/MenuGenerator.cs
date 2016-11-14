using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MenuBuilder.Graders;
using MenuBuilder.Graders.MealGraders;
using MenuBuilder.Graders.DailyMenuGraders;
using MenuBuilder.Graders.MenuGraders;

namespace MenuBuilder
{
    class MenuGenerator
    {
        private class MenuObjectComparer<T> : IComparer<KeyValuePair<T, double>> where T : GradableObject
        {
            public int Compare(KeyValuePair<T, double> x, KeyValuePair<T, double> y)
            {
                return (-1) * x.Value.CompareTo(y.Value); // Sort in descending order
            }
        }

        private static double EvaluateObject<T>(GradableObject obj, Dictionary<T, double> graderMap)
            where T : Grader
        {
            double graderWeightSum = 0;
            double grade = 0;

            foreach (var entry in graderMap)
            {
                graderWeightSum += entry.Value;

                var grader = entry.Key;
                grade += (grader.Grade(obj) * entry.Value);
            }

            // Return the scaled grade
            return (grade / graderWeightSum) * 100;
        }

        public static List<KeyValuePair<Menu, double>> GenerateMenuList(RestDBInterface unit, GraderDB graderDB)
        {
            Grader.graderDB = graderDB; // For the initialization of graders map

            // Initialize graders map
            var graderMap = new Dictionary<MenuGrader, double>()
            {
                {new NutValuesGrader(), 0.35},
                {new CaloriesCountGrader(), 0.35},
                {new VarietyGrader(), 0.3},
                {new TasteGrader(), 0} // Currently 0 as this is not implemented
            };
            var menuList = GetMenuList(unit);
            var menuGradeList = menuList.Select(x => new KeyValuePair<Menu, double>(x, EvaluateObject(x, graderMap))).ToList();

            menuGradeList.Sort(new MenuObjectComparer<Menu>());

            Grader.graderDB = null;

            return menuGradeList;
        }

        private static List<Menu> GetMenuList(RestDBInterface unit)
        {
            var dailyMenusList = GetDaysList(unit);

            // Take only the best MAX_DAYS_IN_LIST_NUM days
            var graderMap = new Dictionary<DailyMenuGrader, double>()
            {
                {new NutValuesDailyGrader(), 0.35},
                {new CaloriesCountDailyGrader(), 0.35},
                {new VarietyDailyGrader(), 0.3},
                {new TasteDailyGrader(), 0} // Currently 0 as this is not implemented
            };
            var sortedDailyMenuList = GetTopGradableObject<DailyMenu, DailyMenuGrader>(dailyMenusList, graderMap, Globals.MAX_DAYS_IN_LIST_NUM);

            // Create all possible combinations
            var daysLists = GetSubsetsOfSize(sortedDailyMenuList, Grader.graderDB.menuDaysNum);

            return daysLists.Select(x => new Menu(x)).ToList();
        }

        private static List<T> GetTopGradableObject<T, S>(List<T> origList, Dictionary<S, double> graderMap, int maxNumInList)
            where T : GradableObject
            where S : Grader
        {
            var ratedList = origList.Select(x => new KeyValuePair<T, double>(x, EvaluateObject(x, graderMap))).ToList();
            ratedList.Sort(new MenuObjectComparer<T>());

            var sortedList = ratedList.Select(x => x.Key).ToList();
            if (sortedList.Count > maxNumInList)
            {
                sortedList.RemoveRange(maxNumInList, sortedList.Count - maxNumInList);
            }

            return sortedList;
        }

        private static List<DailyMenu> GetDaysList(RestDBInterface unit)
        {
            var daysList = new List<DailyMenu>();

            var mealsList = unit.Meals.GetAll().ToList();
            var menuMealsList = mealsList.Select(x => new MenuMeal() { Meal = x });

            var breakfastList = menuMealsList.Where(x => x.Meal.HasType(MealType.Breakfast)).ToList();
            var lunchList = menuMealsList.Where(x => x.Meal.HasType(MealType.Lunch)).ToList();
            var dinnerList = menuMealsList.Where(x => x.Meal.HasType(MealType.Dinner)).ToList();

            // Take only the best MAX_MEALS_IN_LIST_NUM days
            var graderMap = new Dictionary<MealGrader, double>()
            {
                {new TasteMealGrader(), 1},
            };
            var sortedBreakfastList = GetTopGradableObject<MenuMeal, MealGrader>(breakfastList, graderMap, Globals.MAX_MEALS_IN_LIST_NUM);
            var sortedLunchList = GetTopGradableObject<MenuMeal, MealGrader>(lunchList, graderMap, Globals.MAX_MEALS_IN_LIST_NUM);
            var sortedDinnerList = GetTopGradableObject<MenuMeal, MealGrader>(dinnerList, graderMap, Globals.MAX_MEALS_IN_LIST_NUM);

            // Create all possible combinations
            foreach (var breakfast in sortedBreakfastList)
            {
                foreach (var lunch in sortedLunchList)
                {
                    foreach (var dinner in sortedDinnerList)
                    {
                        daysList.Add(new DailyMenu() { Breakfast = breakfast, Lunch = lunch, Dinner = dinner });
                    }
                }
            }

            return daysList;
        }

        /**
         * Get all subsetes of certain size of the given list.
         */
        private static List<List<T>> GetSubsetsOfSize<T>(List<T> mainList, int size)
        {
            var resList = new List<List<T>>();

            if (size == 0)
            {
                resList.Add(new List<T>());
                return resList;
            }

            if (size == mainList.Count)
            {
                resList.Add(mainList);
                return resList;
            }

            // Get all the lists with and without the first element
            T first = mainList[0];
            var restOfList = new List<T>();
            restOfList.AddRange(mainList);
            restOfList.RemoveAt(0);
            var withFirstList = GetSubsetsOfSize(restOfList, size - 1);
            var withoutFirstList = GetSubsetsOfSize(restOfList, size);

            // First add all the without combinations
            resList.AddRange(withoutFirstList);
            // Second, add the first element to all of the with combinations,, and add them
            foreach (var comb in withFirstList)
            {
                comb.Add(first);
                resList.Add(comb);
            }

            return resList;
        }
    }
}
