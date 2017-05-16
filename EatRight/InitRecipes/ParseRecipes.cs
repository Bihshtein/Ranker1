using log4net;
using Logic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InitRecipes.Parsers.RecipesPageParsers;

namespace InitRecipes {
    class ParseRecipes {
        public static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string FolderPath = Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\LocalDB\";
        public static object Locker = new object();
        public static List<string> RecipeIds;

        public static void CreateDB(bool offline, bool dropTable, int limit, RecipesSource? source=null, MealType? mealType=null) {
            RecipeIds = new List<string>();
            var unit = new RestDBInterface();
            if (dropTable)
                unit.Recipes.Empty();
            var loaded = unit.Recipes.GetAllList();
            var ids = loaded.Select(r => r.ID).ToList();
            ids.Sort();
            if (ids.Count > 0)
                AddProducts.CurrId = ids[ids.Count - 1];
            AddProducts.CurrId++;
            log.Debug("Num of recipes before : " + ids.Count);
            if (source.HasValue && mealType.HasValue)
                AddRecipesByMealType(source.Value, Sources.RecipesURNs[source.Value].Meals.Find(m => m.Meal == mealType.Value), unit, offline, loaded, limit);
            else
                Sources.RecipesURNs.ToList().ForEach(s => AddRecipesBySource(s.Key, unit, offline, loaded.FindAll(r => r.Source == s.Key), limit));
            AddProducts.DumpDebug();
        }

        public static void AddRecipesBySource(RecipesSource source, RestDBInterface unit, bool offline, List<Recipe> loadedRecipes, int limit) {
            Sources.RecipesURNs[source].Meals.ToList().ForEach(m => AddRecipesByMealType(source, m, unit, offline,loadedRecipes.FindAll(r => r.Types.Contains(m.Meal)), limit));
        }

        public static void AddRecipesByMealType(RecipesSource source, MealData mealData, RestDBInterface unit, bool offline, List<Recipe> loadedRecipes, int limit,int loadBulkSize = 1000) {
            var loadedIds = loadedRecipes.Select(r => r.OriginalID).ToList();
            if (source == RecipesSource.AllRecipes && !offline)
                loadBulkSize = 10;
            if (offline) {
                var files = Directory.GetFiles(Path.Combine(FolderPath, source.ToString(), mealData.Meal.ToString())).ToList();
                files.ForEach(f =>RecipeIds.Add(Path.GetFileNameWithoutExtension(f)));
                RecipeIds.RemoveAll(i => loadedIds.Contains(i));
                RecipeIds = RecipeIds.Take(limit).ToList();
            }
            else
                AddRecipesByURL(source, mealData, unit, limit);
                
            while (RecipeIds.Count > 9) {
                log.Debug("Loading bulk, tasks left : " + RecipeIds.Count());
                var loadMealsBulkSize = RecipeIds.Count > loadBulkSize ? loadBulkSize : RecipeIds.Count;
                var tasks = new List<Task>();
                RecipeIds.Take(loadMealsBulkSize).ToList().ForEach(a => tasks.Add(new Task(new Action(() => ParseRecipe(a, Sources.RecipesURNs[source].Url, mealData.Meal, source)))));
                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
            }
        }
      
        private static void AddRecipesByURL(RecipesSource source, MealData mealType,  RestDBInterface unit, int recipesLimit) {
            RecipeIds.Clear();
            log.Debug("Locating recipes in ->" +mealType.Url + " - started");
            var tasksCount = 8;
            int page = 0;
            shouldStop = false;
            while (RecipeIds.Count < recipesLimit && !shouldStop) {
                var tasks = new List<Task>();
                var beforeCount = RecipeIds.Count;
                for (int i = 0; i < tasksCount; i++) {
                    tasks.Add(new Task(new Action(() => ReadPage(source, mealType, unit, page++, new WebClient()))));
                }

                tasks.ForEach(task => task.Start());
                tasks.ForEach(task => task.Wait());
                if (beforeCount == RecipeIds.Count)
                    shouldStop = true;

            }
            
            log.Debug("Locating recipes in ->" + mealType.Url + " - completed. RecipeIds count : " + RecipeIds.Count);
        }
        private static bool shouldStop;
        private static void ReadPage(RecipesSource source, MealData mealType,  RestDBInterface unit, int currPage, WebClient client) {
            string pageStr = null;
            var readWorked = false;
            var urlSuffix = currPage == 0 ? "" : (Sources.RecipesURNs[source].PageSeperator + Sources.RecipesURNs[source].PageKeyword + "=" + currPage);
            var uri = mealType.Url + urlSuffix;
            for (int retries = 0; readWorked == false && retries < 10; retries++) {
                try {
                    pageStr = client.DownloadString(uri);
                    readWorked = true;
                    IRecipesPageParser pageParser = RecipesPageParserFactory.CreateRecipesPageParser(source);
                    pageParser.AddRecipesFromPage(pageStr, unit);
                }
                catch (Exception) {
                    log.Error(string.Format("Failed to load page num {0}, might be the last page", currPage));
                    if (retries >9)
                        shouldStop = true;
                }
            }
            log.Debug("Page num :" + currPage);
        }

        public static void ParseRecipe(string recipeId, string urn, MealType mealType, RecipesSource source) {
            var customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone(); customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            var unit = new RestDBInterface();
            var page = string.Empty;
            try {
                Directory.CreateDirectory(Path.Combine(FolderPath, source.ToString()));
                Directory.CreateDirectory(Path.Combine(FolderPath, source.ToString(), mealType.ToString()));
                var filePath = Path.Combine(FolderPath, source.ToString(), mealType.ToString(),recipeId + ".txt");
                if (File.Exists(filePath))
                    page = File.ReadAllText(filePath);
                else {
                    page = new WebClient().DownloadString(urn + recipeId);
                    File.WriteAllText(filePath, page);
                }
            }
            catch {
                log.Error("Failed to load recipe : " + recipeId);
                return;
            }
           
            var recipe = new Recipe{ 
                OriginalID = recipeId,
                Source = source,
                Urn = Sources.RecipesURNs[source].Url.Split(new string[1] { "//" }, StringSplitOptions.None)[1],
                Name = Sources.RecipesURNs[source].Parser.Parser.GetRecipeName(page),
                Ingredients = Sources.RecipesURNs[source].Parser.GetIngredients(page),
                Types = new HashSet<MealType>() { mealType },
                Servings = Sources.RecipesURNs[source].Parser.GetServings(page),
                StepsNum = Sources.RecipesURNs[source].Parser.GetStepsNum(page),
                PrepTime = Sources.RecipesURNs[source].Parser.Parser.GetPrepTime(page),
                ImageUrl = Sources.RecipesURNs[source].Parser.GetImageUrl(page)
            };

            AddProducts.AddWeightsAndCalories(recipe);

            lock (Locker) {
                RecipeIds.Remove(recipeId);
            }
        }
    }
}
