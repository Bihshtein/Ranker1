﻿using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    public enum RecipesSource { AllRecipes=1, Cookpad=2, Food=3 }
    class MealData {
        public MealType Meal { get; set; }
        public string Url { get; set; }
        public int MealsLimit { get; set; }
    }
    class SourceData {
        public string Url { get; set; }
        public string PageKeyword { get; set; }
        public List<MealData> Meals { get; set; }
        public IRecipeParser Parser{ get; set; }
}
    class Sources {

        public static Dictionary<RecipesSource, SourceData> RecipesURNs = new Dictionary<RecipesSource, SourceData>() {
            {RecipesSource.Cookpad,  new SourceData {
                Url = "https://cookpad.com/us/recipes/",
                PageKeyword = "page",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Dinner, Url = "https://cookpad.com/us/search/dinner", MealsLimit=100},
                    new MealData { Meal = MealType.Breakfast, Url = "https://cookpad.com/us/search/breakfast", MealsLimit=100},
                    new MealData { Meal = MealType.Lunch, Url = "https://cookpad.com/us/search/lunch", MealsLimit=100},
                },
                Parser = new CookpadParser()
            } },


            {RecipesSource.AllRecipes,   new SourceData() {
                PageKeyword = "page",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Dinner, Url = "http://allrecipes.com/recipes/17562/dinner", MealsLimit=100},
                    new MealData { Meal = MealType.Breakfast, Url = "http://allrecipes.com/recipes/78/breakfast-and-brunch", MealsLimit=100},
                    new MealData { Meal = MealType.Lunch, Url = "http://allrecipes.com/recipes/17561/lunch/", MealsLimit=100},
                },
                Parser = new AllRecipesParser(),
                Url ="http://allrecipes.com/recipe/" } },

            {RecipesSource.Food,  new SourceData() {
                PageKeyword = "pn",
                Meals = new List<MealData> {
                    new MealData { Meal = MealType.Breakfast, Url = "http://breakfast.food.com", MealsLimit=100},
                    new MealData { Meal = MealType.Lunch, Url = "http://lunch.food.com", MealsLimit=100},
                },
                Parser = new FoodParser(),
                Url ="http://www.food.com/recipe/-" } }
        };
    }
}
