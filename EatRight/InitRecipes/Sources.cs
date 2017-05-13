using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    
    class MealData {
        public MealType Meal { get; set; }
        public string Url { get; set; }
        public int MealsLimit { get; set; }
    }
    class SourceData {

        public string Url { get; set; }
        public string PageKeyword { get; set; }
        public string PageSeperator { get; set; }
        public string PageSplitterStr { get; set; }
        public List<MealData> Meals { get; set; }
        public GeneralRecipeParser Parser { get; set; }
}
    class Sources {
        public static Dictionary<RecipesSource, SourceData> RecipesURNs = new Dictionary<RecipesSource, SourceData>() {

            {RecipesSource.Cookpad,  new SourceData {
                Url = "https://cookpad.com/us/recipes/",
                PageKeyword = "page",
                PageSeperator = "?",
                PageSplitterStr = "data-id=\"",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Dinner, Url = "https://cookpad.com/us/search/dinner"},
                    new MealData { Meal = MealType.Breakfast, Url = "https://cookpad.com/us/search/breakfast"},
                    new MealData { Meal = MealType.Lunch, Url = "https://cookpad.com/us/search/lunch"},
                },
                Parser = new GeneralRecipeParser(new CookpadParser())
            } },

            {RecipesSource.AllRecipes,   new SourceData() {
                PageKeyword = "page",
                PageSeperator = "?",
                PageSplitterStr = "data-id=\"",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Dinner, Url = "http://allrecipes.com/recipes/17562/dinner"},
                    new MealData { Meal = MealType.Breakfast, Url = "http://allrecipes.com/recipes/78/breakfast-and-brunch"},
                    new MealData { Meal = MealType.Lunch, Url = "http://allrecipes.com/recipes/17561/lunch/"},
                },
                Parser = new GeneralRecipeParser(new AllRecipesParser()),
                Url ="http://allrecipes.com/recipe/" } },

            {RecipesSource.Food,  new SourceData() {
                PageKeyword = "pn",
                PageSeperator = "?",
                PageSplitterStr = "data-id=\"",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Breakfast, Url = "http://breakfast.food.com", },
                    new MealData { Meal = MealType.Lunch, Url = "http://lunch.food.com"},
                },
                Parser = new GeneralRecipeParser(new FoodParser()),
                Url ="http://www.food.com/recipe/-" } },

            {RecipesSource.BBC,  new SourceData() {
                PageKeyword = "page",
                PageSeperator = "?",
                PageSplitterStr = "data-id=\"",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Dinner, Url = "https://www.bbcgoodfood.com/search/recipesc/course/dinner", },
                },
                Parser = new GeneralRecipeParser(new BBCParser()),
                Url ="https://www.bbcgoodfood.com/recipes/" } },

            {RecipesSource.Epicurious,  new SourceData() {
                PageKeyword = "page",
                PageSeperator = "&",
                PageSplitterStr = "href=\"/recipes/food/views/",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Dinner, Url = "http://www.epicurious.com/search/?meal=dinner", },
                },
                Parser = new GeneralRecipeParser(new EpicuriousParser()),
                Url ="http://www.epicurious.com/recipes/food/views/" } }
        };
    }
}
