using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MenuBuilder.Graders;
using MenuBuilder.Graders.DailyMenuGraders;
using MenuBuilder.Graders.MenuGraders;

namespace MenuBuilder
{
    class MenuGenerator
    {
        private class MenuObjectComparer<T> : IComparer<KeyValuePair<T, double>> where T : MenuBuilderObject
        {
            public int Compare(KeyValuePair<T, double> x, KeyValuePair<T, double> y)
            {
                return (-1) * x.Value.CompareTo(y.Value); // Sort in descending order
            }
        }

        private static double EvaluateObject<T>(MenuBuilderObject obj, Dictionary<T, double> graderMap)
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
                {new VarietyGrader(), 0.3}
            };
            var menuList = GetMenuList(unit);

            var menuGradeList = new List<KeyValuePair<Menu, double>>();

            foreach (var menu in menuList)
            {
                menuGradeList.Add(new KeyValuePair<Menu, double>(menu, EvaluateObject(menu, graderMap)));
            }

            menuGradeList.Sort(new MenuObjectComparer<Menu>());

            Grader.graderDB = null;

            return menuGradeList;
        }

        private static List<Menu> GetMenuList(RestDBInterface unit)
        {
            var menuList = new List<Menu>();

            var dailyMenusList = GetDaysList(unit);

            // Take only the best MAX_DAYS_IN_LIST_NUM days
            var graderMap = new Dictionary<DailyMenuGrader, double>()
            {
                {new NutValuesDailyGrader(), 0.35},
                {new CaloriesCountDailyGrader(), 0.35},
                {new VarietyDailyGrader(), 0.3}
            };
            var ratedDailyMenusList = new List<KeyValuePair<DailyMenu, double>>();
            foreach (DailyMenu dailyMenu in dailyMenusList)
            {
                ratedDailyMenusList.Add(new KeyValuePair<DailyMenu, double>(dailyMenu, EvaluateObject(dailyMenu, graderMap)));
            }
            ratedDailyMenusList.Sort(new MenuObjectComparer<DailyMenu>());
            var sortedDailyMenuList = new List<DailyMenu>();
            foreach (var entry in ratedDailyMenusList)
            {
                sortedDailyMenuList.Add(entry.Key);
            }
            sortedDailyMenuList.RemoveRange(Globals.MAX_DAYS_IN_LIST_NUM, sortedDailyMenuList.Count - Globals.MAX_DAYS_IN_LIST_NUM);

            // Create all possible combinations
            var daysLists = GetSubsetsOfSize(sortedDailyMenuList, Grader.graderDB.menuDaysNum);
            foreach (var list in daysLists)
            {
                menuList.Add(new Menu(list));
            }

            return menuList;
        }

        private static List<DailyMenu> GetDaysList(RestDBInterface unit)
        {
            var daysList = new List<DailyMenu>();

            /* We go over all possible combinations for all of the DB meals.
             * This may be too much and we may filter it in the future (for example by classifying the meals as breakfast/lunch/dinner.
             */
            var mealsList = unit.Meals.GetAll();

            int cou = 0;
            // Iterate over breaskfast meals
            foreach (Meal breakfast in mealsList)
            {
                cou++;
                // Iterate over lunch meals
                foreach (Meal lunch in mealsList)
                {
                    // Iterate over dinner meals
                    foreach (Meal dinner in mealsList) {
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
