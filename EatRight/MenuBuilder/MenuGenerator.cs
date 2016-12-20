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
    public class MenuGenerator
    {
        // Input
        RestDBInterface unit;

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
            Grader.graderDB = graderDB; // For the initialization of graders map
            if (Grader.graderDB.GradersWeight == null)
            {
                SetDefaultGraderWeights();
            }

            this.unit = unit;

            mealsList = new List<MenuMeal>();
            if (InMenuMode())
            {
                GenerateMenusList();
            }
            else if (InMealMode())
            {
                GenerateMealsList();
            }

            usedDailyMenus = new HashSet<int>();
            usedMeals = new HashSet<int>();
        }

        public List<MenuMeal> GetMealsList()
        {
            return mealsList;
        }

        private bool InMenuMode()
        {
            return Grader.graderDB.range.IsMenuSuggestionRange();
        }

        private bool InMealMode()
        {
            return Grader.graderDB.range.IsMealSuggestionRange();
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

        public MenuMeal GetMeal()
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
            SetMealAsUsed(newMenuMeal);
            return newMenuMeal;
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
            return usedMeals.Contains(meal.Meal.ID);
        }

        private void SetDailyMenuAsUsed(DailyMenu dailyMenu)
        {
            usedDailyMenus.Add(dailyMenu.ID);

            // Update the used meals
            foreach (var entry in dailyMenu.Meals)
            {
                SetMealAsUsed(entry.Value);
            }
        }

        private void SetDailyMenuAsUnused(DailyMenu dailyMenu)
        {
            usedDailyMenus.Remove(dailyMenu.ID);

            // Update the used meals
            foreach (var entry in dailyMenu.Meals)
            {
                SetMealAsUnused(entry.Value);
            }
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
            var gradersInfo = new HashSet<GraderInfo>();

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
                    gradersInfo.Add(new GraderInfo(grader.Type, entry.Value, curGrade));
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

            obj.GradeInfo = new GradeInfo()
                { BestGraders = bestGraders, WorstGraders = worstGraders, GradersInfo = gradersInfo };
        }

        private void GenerateMenusList()
        {
            // If the meals type in each day weren't specifiec by the user, set it to the default type
            if (((MenuSuggestionRange)Grader.graderDB.range).MealsInDailyMenu == null)
            {
                ((MenuSuggestionRange)Grader.graderDB.range).MealsInDailyMenu = new List<MealType>()
                    { MealType.Breakfast, MealType.Lunch, MealType.Dinner };
            }

            GenerateDailyMenuList();

            // Take only the best MAX_DAYS_IN_LIST_NUM days
            var filteredDailyMenuList = GetTopGradableObject<DailyMenu>(dailyMenusList, Globals.MAX_DAYS_IN_LIST_NUM);

            // Create all possible combinations
            var daysLists = GetSubsetsOfSize(filteredDailyMenuList, Grader.graderDB.range.Length);

            // Initialize graders map
            var graderMap = InitGraderMap(GraderType.GraderMenuStart, GraderType.GraderMenuEnd);

            menusList = daysLists.Select(x => new Menu(x)).ToList();

            menusList.ForEach(x => EvaluateObject(x, graderMap));
            menusList.Sort(new GradableObjectComparer<Menu>());
        }

        private void GenerateDailyMenuList()
        {
            GenerateMealsList();

            // Take only the best MAX_MEALS_IN_LIST_NUM days
            var mealsLists = new Dictionary<MealType, List<MenuMeal>>();
            ((MenuSuggestionRange)Grader.graderDB.range).MealsInDailyMenu.ForEach
                (x => mealsLists[x] = mealsList.Where(y => y.Meal.HasType(x)).Take(Globals.MAX_MEALS_IN_LIST_NUM).ToList());
            dailyMenusList = GetAllCombinations(mealsLists).Select(x => new DailyMenu(x)).ToList();

            var graderMap = InitGraderMap(GraderType.DailyMenuGraderStart, GraderType.DailyMenuGraderEnd);

            dailyMenusList.ForEach(x => EvaluateObject(x, graderMap));
            dailyMenusList.Sort(new GradableObjectComparer<DailyMenu>());
        }

        private void GenerateMealsList()
        {
            var dbMealsList = unit.Meals.GetAll().ToList();

            // Filter meals with unwanted products
            dbMealsList = FilterDBMealsList(dbMealsList);

            // Filter meals of irrelevant type
            if (InMealMode())
            {
                dbMealsList = dbMealsList.Where(x => x.Types.Contains(((MealSuggestionRange)Grader.graderDB.range).MealType)).ToList();
            }

            mealsList = dbMealsList.Select(x => new MenuMeal(x)).ToList();

            var graderMap = InitGraderMap(GraderType.MealGraderStart, GraderType.MealGraderEnd);

            mealsList.ForEach(x => EvaluateObject(x, graderMap));
            mealsList.Sort(new GradableObjectComparer<MenuMeal>());
        }

        /**
         * Filter meals that contains products that cannot be eaten by the user.
         */
        private List<Meal> FilterDBMealsList(List<Meal> dbMealsList)
        {
            if (Grader.graderDB.forbiddenProducts == null || Grader.graderDB.forbiddenProducts.Count == 0)
            {
                return dbMealsList;
            }

            var resList = new List<Meal>();
            foreach (var meal in dbMealsList)
            {
                bool forbiddenMeal = false;

                foreach (var prodName in meal.ProductsWeight.Keys)
                {
                    if (Grader.graderDB.forbiddenProducts.Contains(prodName))
                    {
                        forbiddenMeal = true;
                        break;
                    }
                }

                if (!forbiddenMeal)
                {
                    resList.Add(meal);
                }
            }

            return resList;
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
         * Get all combinations of possible mapping from meal types to meals.
         */
        private static List<Dictionary<MealType, MenuMeal>> GetAllCombinations(Dictionary<MealType, List<MenuMeal>> listsMapping)
        {
            var resList = new List<Dictionary<MealType, MenuMeal>>();

            if (listsMapping.Count == 0)
            {
                resList.Add(new Dictionary<MealType, MenuMeal>());
                return resList;
            }

            // Get all the lists without elements from the first list
            var firstEntry = listsMapping.First();
            var restOfLists = listsMapping;
            var firstType = firstEntry.Key;
            restOfLists.Remove(firstType);
            var combWithoutFirstList = GetAllCombinations(restOfLists);

            // Add elements from first list to all the combinations
            foreach (var elem in firstEntry.Value)
            {
                foreach (var comb in combWithoutFirstList)
                {
                    var finalComb = new Dictionary<MealType, MenuMeal>(comb);
                    finalComb[firstType] = elem;
                    resList.Add(finalComb);
                }
            }

            return resList;
        }

        private Dictionary<Grader, double> InitGraderMap(GraderType rangeBegin, GraderType rangeEnd)
        {
            var res = new Dictionary<Grader, double>();

            foreach (var entry in Grader.graderDB.GradersWeight)
            {
                var curType = entry.Key;
                if (curType > rangeBegin && curType < rangeEnd)
                {
                    res[Grader.GetGraderByType(curType)] = entry.Value;
                }
            }

            return res;
        }

        private void SetDefaultGraderWeights()
        {
            if (InMenuMode())
            {
                Grader.graderDB.GradersWeight = new Dictionary<GraderType, double>()
                {
                    // Menu graders
                    {GraderType.NutValuesGrader, 0.3},
                    {GraderType.CaloriesCountGrader, 0.3},
                    {GraderType.VarietyGrader, 0.25},
                    {GraderType.ProductsTasteGrader, 0.06},
                    {GraderType.FoodCategoryGrader, 0.03},
                    {GraderType.PrepTimeGrader, 0.06},
                    {GraderType.CostGrader, 0}, // Currently zero as this is not fully implemented- need to add cost per product

                    // DailyMenu graders
                    {GraderType.NutValuesDailyGrader, 0.3},
                    {GraderType.CaloriesCountDailyGrader, 0.3},
                    {GraderType.VarietyDailyGrader, 0.25},
                    {GraderType.ProductsTasteDailyGrader, 0.06},
                    {GraderType.FoodCategoryDailyGrader, 0.03},
                    {GraderType.PrepTimeDailyGrader, 0.06},

                    // Meal graders
                    {GraderType.ProductsTasteMealGrader, 0.4},
                    {GraderType.FoodCategoryMealGrader, 0.2},
                    {GraderType.PrepTimeMealGrader, 0.4}
                };
            }
            else if (InMealMode())
            {
                Grader.graderDB.GradersWeight = new Dictionary<GraderType, double>()
                {
                    // Meal graders
                    {GraderType.CaloriesCountMealGrader, 0.4},
                    {GraderType.NutValuesMealGrader, 0.4},
                    {GraderType.ProductsTasteMealGrader, 0.08},
                    {GraderType.FoodCategoryMealGrader, 0.04},
                    {GraderType.PrepTimeMealGrader, 0.08}
                };
            }
        }
    }
}
