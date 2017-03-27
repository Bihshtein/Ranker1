using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    public enum RecipesSource { AllRecipes=1, Cookpad=2, Food=3 }

    class Sources {

        public static Dictionary<RecipesSource, string> RecipesURNs = new Dictionary<RecipesSource, string>() {
            {RecipesSource.Cookpad,  "https://cookpad.com/us/recipes/" },
            {RecipesSource.AllRecipes,  "http://allrecipes.com/recipe/" },
            {RecipesSource.Food, "http://www.food.com/recipe/-" }
        };
        public static Dictionary<RecipesSource, Dictionary<MealType, Tuple<string, string, int>>> MealTypesURNs = new Dictionary<RecipesSource, Dictionary<MealType, Tuple<string, string, int>>>() {
        /*    {RecipesSource.AllRecipes, new Dictionary<MealType, Tuple<string,string, int>>() {
                {MealType.Dinner, new Tuple<string,string,int>( "page","http://allrecipes.com/recipes/17562/dinner",1) },
                {MealType.Breakfast, new Tuple<string,string,int>( "page","http://allrecipes.com/recipes/78/breakfast-and-brunch",1) } }
            },*/
               {RecipesSource.Food, new Dictionary<MealType, Tuple<string,string, int>>() {
                {MealType.Breakfast, new Tuple<string,string,int>( "pn","http://breakfast.food.com",20) } ,
               {MealType.Dinner, new Tuple<string,string,int>( "pn","http://www.food.com/topic/main-dish",20) } }
            },
            {RecipesSource.Cookpad, new Dictionary<MealType, Tuple<string,string, int>>() {
               {MealType.Breakfast, new Tuple<string,string,int>("page","https://cookpad.com/us/search/breakfast",20)},
                {MealType.Lunch,  new Tuple<string,string,int>("page","https://cookpad.com/us/search/lunch",20 )},
               {MealType.Dinner, new Tuple<string,string,int>( "page","https://cookpad.com/us/search/dinner" ,20)} }
            }
        };

    }
}
