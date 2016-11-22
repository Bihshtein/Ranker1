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
        // Input
        RestDBInterface unit;
        GraderDB graderDB;

        // Output
        // Menus
        private List<Menu> menusList = null;
        private int currentMenuIdx = -1;
        // Daily menus
        private List<DailyMenu> dailyMenusList = null;
        private HashSet<int> usedDailyMenus = null;
        // Meals
        private List<MenuMeal> mealsList = null;
        private HashSet<int> usedMeals = null;

        public MenuGenerator(RestDBInterface unit, GraderDB graderDB)
        {
            this.unit = unit;
            this.graderDB = graderDB;

            mealsList = new List<MenuMeal>();
            if (graderDB.range.Type == SuggestionRangeType.Days)
            {
                GenerateMenusList();
            }
            else if (graderDB.range.Type == SuggestionRangeType.Meals)
            {
                GenerateMealsList();
            }

            usedDailyMenus = new HashSet<int>();
            usedMeals = new HashSet<int>();
        }

        private bool InMenuMode()
        {
            return graderDB.range.Type == SuggestionRangeType.Days;
        }

        private bool InMealMode()
        {
            return graderDB.range.Type == SuggestionRangeType.Meals;
        }

        public Menu GetMenu()
        {
            if (!InMenuMode())
            {
                // TODO: better handle errors
                System.Console.WriteLine("***ERROR*** MenuGenerator object created for generating menus, not meals!");
                System.Environment.Exit(1);
            }

            Menu newMenu = menusList[++currentMenuIdx];

            SetMenuAsUsed(newMenu);

            return newMenu;
        }

        public void ReplaceDayByInd(Menu menu, int index)
        {
            if (index >= menu.GetDaysNumber())
            {
                // TODO: better handle warnings
                System.Console.WriteLine("***WARNING*** day in index {0} doesn't exist in relevant menu!", index);
                return;
            }

            DailyMenu newDailyMenu = GetBestUnusedDailyMenu();
            SetDailyMenuAsUnused(menu.GetDay(index));
            SetDailyMenuAsUsed(newDailyMenu);
            menu.Days[index] = newDailyMenu;
        }

        public Meal ReplaceMeal(Meal oldMeal)
        {
            if (!InMealMode())
            {
                // TODO: better handle errors
                System.Console.WriteLine("***ERROR*** MenuGenerator object created for generating meals, not menus!");
                System.Environment.Exit(1);
            }

            MenuMeal newMenuMeal = GetBestUnusedMeal();
            if (newMenuMeal == null)
            {
                return null;
            }
            SetMealAsUnused(oldMeal.ID);
            SetMealAsUsed(newMenuMeal);
            return newMenuMeal.Meal;
        }

        private void SetMenuAsUsed(Menu menu)
        {
            // Update the used daily menus
            usedDailyMenus.Clear(); // Remove the old used days- we only have one menu relevant at each point at time
            menu.Days.ForEach(x => SetDailyMenuAsUsed(x));
        }

        private DailyMenu GetBestUnusedDailyMenu()
        {
            foreach (var dailyMenu in dailyMenusList)
            {
                if (!IsUsed(dailyMenu))
                {
                    return dailyMenu;
                }
            }

            // TODO: better handle warnings
            System.Console.WriteLine("***WARNING*** No unused daily menus!");
            return null;
        }

        private MenuMeal GetBestUnusedMeal()
        {
            foreach (var meal in mealsList)
            {
                if (!IsUsed(meal))
                {
                    return meal;
                }
            }

            // TODO: better handle warnings
            System.Console.WriteLine("***WARNING*** No unused meals!");
            return null;
        }

        private bool IsUsed(DailyMenu dailyMenu)
        {
            return usedDailyMenus.Contains(dailyMenu.ID);
        }

        private bool IsUsed(MenuMeal meal)
        {
            return usedDailyMenus.Contains(meal.Meal.ID);
        }

        private void SetDailyMenuAsUsed(DailyMenu dailyMenu)
        {
            usedDailyMenus.Add(dailyMenu.ID);

            // Update the used meals
            dailyMenu.Meals.ForEach(x => SetMealAsUsed(x));
        }

        private void SetDailyMenuAsUnused(DailyMenu dailyMenu)
        {
            usedDailyMenus.Remove(dailyMenu.ID);

            // Update the used meals
            dailyMenu.Meals.ForEach(x => SetMealAsUnused(x));
        }

        private void SetMealAsUsed(MenuMeal meal)
        {
            usedMeals.Add(meal.Meal.ID);
        }

        private void SetMealAsUnused(MenuMeal meal)
        {
            usedMeals.Remove(meal.Meal.ID);
        }

        private void SetMealAsUnused(int mealIdx)
        {
            usedMeals.Remove(mealIdx);
        }

        private class GradableObjectComparer<T> : IComparer<T> where T : GradableObject
        {
            public int Compare(T x, T y)
            {
                return (-1) * x.Grade.CompareTo(y.Grade); // Sort in descending order
            }
        }

        private static void EvaluateObject<T>(GradableObject obj, Dictionary<T, double> graderMap)
            where T : Grader
        {
            double graderWeightSum = 0;
            int gradersNum = graderMap.Count;
            double grade = 0;
            var graderStrToGain = new Dictionary<string, double>();
            var graderStrToLoss = new Dictionary<string, double>();

            foreach (var entry in graderMap)
            {
                graderWeightSum += entry.Value;

                var grader = entry.Key;
                try
                {
                    var curGrade = grader.Grade(obj);
                    var curScaledGrade = curGrade * entry.Value;
                    grade += curScaledGrade;
                    graderStrToGain[grader.Description] = curScaledGrade;
                    graderStrToLoss[grader.Description] = entry.Value - curScaledGrade;
                }
                catch (ArgumentException)
                {
                    // TODO: better handle this
                    System.Console.WriteLine("***ERROR*** Gradable object type {0} not matching grader type {1}!", obj.GetType().ToString(), grader.ToString());
                    Environment.Exit(1);
                }
            }

            // Return the scaled grade
            obj.Grade = (grade / graderWeightSum) * 100;
            var bestGraders = graderStrToGain.OrderByDescending(x => x.Value).Take(gradersNum).Select(y => y.Key).ToList();
            var worstGraders = graderStrToLoss.OrderByDescending(x => x.Value).Take(gradersNum).Select(y => y.Key).ToList();
            obj.GradeInfo = new GradeInfo() { BestGraders = bestGraders, WorstGraders = worstGraders };
        }

        private void GenerateMenusList()
        {
            Grader.graderDB = graderDB; // For the initialization of graders map

            GenerateDailyMenuList();

            // Take only the best MAX_DAYS_IN_LIST_NUM days
            var filteredDailyMenuList = GetTopGradableObject<DailyMenu>(dailyMenusList, Globals.MAX_DAYS_IN_LIST_NUM);

            // Create all possible combinations
            var daysLists = GetSubsetsOfSize(filteredDailyMenuList, Grader.graderDB.range.Length);

            // Initialize graders map
            var graderMap = new Dictionary<MenuGrader, double>()
            {
                {new NutValuesGrader(), 0.3},
                {new CaloriesCountGrader(), 0.3},
                {new VarietyGrader(), 0.25},
                {new ProductsTasteGrader(), 0.1},
                {new FoodCategoryGrader(), 0.05},
                {new CostGrader(), 0}, // Currently zero as this is not fully implemented- need to add cost per product
                {new TypeFitnessGrader(), 0} // Currently zero as this is not fully implemented
            };

            menusList = daysLists.Select(x => new Menu(x)).ToList();

            menusList.ForEach(x => EvaluateObject(x, graderMap));
            menusList.Sort(new GradableObjectComparer<Menu>());

            Grader.graderDB = null;
        }

        private void GenerateDailyMenuList()
        {
            GenerateMealsList();

            // Take only the best MAX_MEALS_IN_LIST_NUM days
            var mealsLists = new List<List<MenuMeal>>();
            Enum.GetValues(typeof(MealType)).Cast<MealType>().ToList().ForEach
                (x => mealsLists.Add(mealsList.Where(y => y.Meal.HasType(x)).Take(Globals.MAX_MEALS_IN_LIST_NUM).ToList()));
            dailyMenusList = GetAllCombinations<MenuMeal>(mealsLists).Select(x => new DailyMenu(x)).ToList();

            var graderMap = new Dictionary<DailyMenuGrader, double>()
            {
                {new NutValuesDailyGrader(), 0.3},
                {new CaloriesCountDailyGrader(), 0.3},
                {new VarietyDailyGrader(), 0.25},
                {new ProductsTasteDailyGrader(), 0.1},
                {new FoodCategoryDailyGrader(), 0.05},
                {new TypeFitnessDailyGrader(), 0} // Currently zero as this is not fully implemented
            };

            dailyMenusList.ForEach(x => EvaluateObject(x, graderMap));
            dailyMenusList.Sort(new GradableObjectComparer<DailyMenu>());
        }

        private void GenerateMealsList()
        {
            var dbMealsList = unit.Meals.GetAll();
            mealsList = dbMealsList.Select(x => new MenuMeal() { Meal = x }).ToList();

            var graderMap = new Dictionary<MealGrader, double>()
            {
                {new ProductsTasteMealGrader(), 0.66},
                {new FoodCategoryMealGrader(), 0.34},
                {new TypeFitnessMealGrader(), 0} // Currently zero as this is not fully implemented
            };

            mealsList.ForEach(x => EvaluateObject(x, graderMap));
            mealsList.Sort(new GradableObjectComparer<MenuMeal>());
        }

        private static List<T> GetTopGradableObject<T>(List<T> origList, int maxNumInList)
            where T : GradableObject
        {
            var filteredList = new List<T>(origList);
            if (filteredList.Count > maxNumInList)
            {
                filteredList.RemoveRange(maxNumInList, filteredList.Count - maxNumInList);
            }

            return filteredList;
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

        /**
         * Get all combinations of list of lists.
         */
        private static List<List<T>> GetAllCombinations<T>(List<List<T>> listOfLists)
        {
            var resList = new List<List<T>>();

            if (listOfLists.Count == 0)
            {
                resList.Add(new List<T>());
                return resList;
            }

            // Get all the lists without elements from the first list
            var firstList = listOfLists[0];
            var restOfLists = listOfLists.Skip(1).Take(listOfLists.Count - 1).ToList();
            var combWithoutFirstList = GetAllCombinations(restOfLists);

            // Add elements from first list to all the combinations
            foreach (var elem in firstList)
            {
                foreach (var comb in combWithoutFirstList)
                {
                    var finalComb = new List<T>(comb);
                    finalComb.Insert(0, elem);
                    resList.Add(finalComb);
                }
            }

            return resList;
        }
    }
}
