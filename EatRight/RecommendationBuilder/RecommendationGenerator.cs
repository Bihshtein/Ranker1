using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using RecommendationBuilder.Graders;
using RecommendationBuilder.Graders.MealGraders;
using RecommendationBuilder.Graders.DailyMenuGraders;
using RecommendationBuilder.Graders.MenuGraders;
using RecommendationBuilder.Filters;
using RecommendationBuilder.Filters.MealFilters;

namespace RecommendationBuilder
{
    public class RecommendationGenerator
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
        private List<MealWrapper> mealsList = null;
        private HashSet<int> usedMeals = null;

        public RecommendationGenerator(RestDBInterface unit, RecommendationDB recommendationDB)
        {
            RecommendationObject.recommendationDB = recommendationDB; // For the initialization of graders map
            if (RecommendationObject.recommendationDB.GradersWeight == null)
            {
                SetDefaultGraderWeights();
            }
            if (RecommendationObject.recommendationDB.FiltersSet == null)
            {
                SetDefaultFilterSet();
            }

            this.unit = unit;

            mealsList = new List<MealWrapper>();
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

        public List<MealWrapper> GetMealsList()
        {
            return mealsList;
        }

        private bool InMenuMode()
        {
            return RecommendationObject.recommendationDB.range.IsMenuSuggestionRange();
        }

        private bool InMealMode()
        {
            return RecommendationObject.recommendationDB.range.IsMealSuggestionRange();
        }

        public Menu GetMenu()
        {
            if (!InMenuMode())
            {
                // TODO: better handle errors
                System.Console.WriteLine("***ERROR*** RecommendationGenerator object created for generating menus, not meals!");
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

        public MealWrapper GetMeal()
        {
            if (!InMealMode())
            {
                // TODO: better handle errors
                System.Console.WriteLine("***ERROR*** RecommendationGenerator object created for generating meals, not menus!");
                System.Environment.Exit(1);
            }

            MealWrapper newMealWrapper = GetBestUnusedMeal();
            if (newMealWrapper == null)
            {
                return null;
            }
            SetMealAsUsed(newMealWrapper);
            return newMealWrapper;
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

        private MealWrapper GetBestUnusedMeal()
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

        private bool IsUsed(MealWrapper meal)
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

        private void SetMealAsUsed(MealWrapper meal)
        {
            usedMeals.Add(meal.Meal.ID);
        }

        private void SetMealAsUnused(MealWrapper meal)
        {
            usedMeals.Remove(meal.Meal.ID);
        }

        private void SetMealAsUnused(int mealIdx)
        {
            usedMeals.Remove(mealIdx);
        }

        private class RecommendationObjectComparer<T> : IComparer<T> where T : RecommendationObject
        {
            public int Compare(T x, T y)
            {
                return (-1) * x.Grade.CompareTo(y.Grade); // Sort in descending order
            }
        }

        private static List<T> FilterList<T,S>(List<T> objList, HashSet<S> filterSet)
            where T : RecommendationObject
            where S : Filter
        {
            var resList = new List<T>();
            foreach (var obj in objList)
            {
                bool validObj = true;

                foreach (var filter in filterSet)
                {
                    if (!filter.IsValid(obj))
                    {
                        validObj = false;
                        break;
                    }
                }

                if (validObj)
                {
                    resList.Add(obj);
                }
            }

            return resList;
        }

        private static void EvaluateObject<T>(RecommendationObject obj, Dictionary<T, double> graderMap)
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
                    System.Console.WriteLine("***ERROR*** Recommendation object type {0} not matching grader type {1}!", obj.GetType().ToString(), grader.ToString());
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
            if (((MenuSuggestionRange)RecommendationObject.recommendationDB.range).MealsInDailyMenu == null)
            {
                ((MenuSuggestionRange)RecommendationObject.recommendationDB.range).MealsInDailyMenu = new List<MealType>()
                    { MealType.Breakfast, MealType.Lunch, MealType.Dinner };
            }

            GenerateDailyMenuList();

            // Take only the best MAX_DAYS_IN_LIST_NUM days
            var filteredDailyMenuList = GetTopRecommendationObject<DailyMenu>(dailyMenusList, Globals.MAX_DAYS_IN_LIST_NUM);

            // Create all possible combinations
            var daysLists = GetSubsetsOfSize(filteredDailyMenuList, RecommendationObject.recommendationDB.range.Length);

            // Initialize graders map
            var graderMap = InitGraderMap(GraderType.GraderMenuStart, GraderType.GraderMenuEnd);

            menusList = daysLists.Select(x => new Menu(x)).ToList();

            menusList.ForEach(x => EvaluateObject(x, graderMap));
            menusList.Sort(new RecommendationObjectComparer<Menu>());
        }

        private void GenerateDailyMenuList()
        {
            GenerateMealsList();

            // Take only the best MAX_MEALS_IN_LIST_NUM days
            var mealsLists = new Dictionary<MealType, List<MealWrapper>>();
            ((MenuSuggestionRange)RecommendationObject.recommendationDB.range).MealsInDailyMenu.ForEach
                (x => mealsLists[x] = mealsList.Where(y => y.Meal.HasType(x)).Take(Globals.MAX_MEALS_IN_LIST_NUM).ToList());
            dailyMenusList = GetAllCombinations(mealsLists).Select(x => new DailyMenu(x)).ToList();

            var graderMap = InitGraderMap(GraderType.DailyMenuGraderStart, GraderType.DailyMenuGraderEnd);

            dailyMenusList.ForEach(x => EvaluateObject(x, graderMap));
            dailyMenusList.Sort(new RecommendationObjectComparer<DailyMenu>());
        }

        private void GenerateMealsList()
        {
            var graderMap = InitGraderMap(GraderType.MealGraderStart, GraderType.MealGraderEnd);
            var filterSet = InitFilterSet(FilterType.MealFilterStart, FilterType.MealFilterEnd);

            var dbMealsList = unit.Meals.GetAll().ToList();

            // Filter meals of irrelevant type
            if (InMealMode())
            {
                dbMealsList = dbMealsList.Where(x => x.Types.Contains(((MealSuggestionRange)RecommendationObject.recommendationDB.range).MealType)).ToList();
            }

            mealsList = dbMealsList.Select(x => new MealWrapper(x)).ToList();
            mealsList = FilterList(mealsList, filterSet);

            mealsList.ForEach(x => EvaluateObject(x, graderMap));
            mealsList.Sort(new RecommendationObjectComparer<MealWrapper>());
        }

        private static List<T> GetTopRecommendationObject<T>(List<T> origList, int maxNumInList)
            where T : RecommendationObject
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
        private static List<Dictionary<MealType, MealWrapper>> GetAllCombinations(Dictionary<MealType, List<MealWrapper>> listsMapping)
        {
            var resList = new List<Dictionary<MealType, MealWrapper>>();

            if (listsMapping.Count == 0)
            {
                resList.Add(new Dictionary<MealType, MealWrapper>());
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
                    var finalComb = new Dictionary<MealType, MealWrapper>(comb);
                    finalComb[firstType] = elem;
                    resList.Add(finalComb);
                }
            }

            return resList;
        }

        private Dictionary<Grader, double> InitGraderMap(GraderType rangeBegin, GraderType rangeEnd)
        {
            var res = new Dictionary<Grader, double>();

            foreach (var entry in RecommendationObject.recommendationDB.GradersWeight)
            {
                var curType = entry.Key;
                if (curType > rangeBegin && curType < rangeEnd)
                {
                    res[Grader.GetGraderByType(curType)] = entry.Value;
                }
            }

            return res;
        }

        private HashSet<Filter> InitFilterSet(FilterType rangeBegin, FilterType rangeEnd)
        {
            var res = new HashSet<Filter>();

            foreach (var filterType in RecommendationObject.recommendationDB.FiltersSet)
            {
                if (filterType > rangeBegin && filterType < rangeEnd)
                {
                    res.Add(Filter.GetFilterByType(filterType));
                }
            }

            return res;
        }

        private void SetDefaultFilterSet()
        {
            if (InMenuMode())
            {
                RecommendationObject.recommendationDB.FiltersSet = new HashSet<FilterType>()
                {
                    // Menu filters

                    // DailyMenu filters

                    // Meal filters
                    FilterType.ForbiddenProductsMealFilter
                };
            }
            else if (InMealMode())
            {
                RecommendationObject.recommendationDB.FiltersSet = new HashSet<FilterType>()
                {
                    // Meal filters
                    FilterType.ForbiddenProductsMealFilter
                };
            }
        }

        private void SetDefaultGraderWeights()
        {
            if (InMenuMode())
            {
                RecommendationObject.recommendationDB.GradersWeight = new Dictionary<GraderType, double>()
                {
                    // Menu graders
                    {GraderType.MinNutValuesGrader, 0.25},
                    {GraderType.CaloriesCountGrader, 0.25},
                    {GraderType.VarietyGrader, 0.19},
                    {GraderType.ProductsTasteGrader, 0.05},
                    {GraderType.FoodCategoryGrader, 0.02},
                    {GraderType.PrepTimeGrader, 0.05},
                    {GraderType.MaxNutValuesGrader, 0.1},
                    {GraderType.ServingsNumGrader, 0.19},
                    {GraderType.CostGrader, 0}, // Currently zero as this is not fully implemented- need to add cost per product

                    // DailyMenu graders
                    {GraderType.MinNutValuesDailyGrader, 0.25},
                    {GraderType.CaloriesCountDailyGrader, 0.25},
                    {GraderType.VarietyDailyGrader, 0.19},
                    {GraderType.ProductsTasteDailyGrader, 0.05},
                    {GraderType.FoodCategoryDailyGrader, 0.02},
                    {GraderType.PrepTimeDailyGrader, 0.05},
                    {GraderType.MaxNutValuesDailyGrader, 0.1},
                    {GraderType.ServingsNumDailyGrader, 0.19},

                    // Meal graders
                    {GraderType.ProductsTasteMealGrader, 0.3},
                    {GraderType.FoodCategoryMealGrader, 0.1},
                    {GraderType.PrepTimeMealGrader, 0.3},
                    {GraderType.ServingsNumMealGrader, 0.3}
                };
            }
            else if (InMealMode())
            {
                RecommendationObject.recommendationDB.GradersWeight = new Dictionary<GraderType, double>()
                {
                    // Meal graders
                    {GraderType.CaloriesCountMealGrader, 0.3},
                    {GraderType.MinNutValuesMealGrader, 0.3},
                    {GraderType.MaxNutValuesMealGrader, 0.1},
                    {GraderType.ProductsTasteMealGrader, 0.08},
                    {GraderType.FoodCategoryMealGrader, 0.04},
                    {GraderType.PrepTimeMealGrader, 0.08},
                    {GraderType.ServingsNumMealGrader, 0.2}
                };
            }
        }
    }
}
