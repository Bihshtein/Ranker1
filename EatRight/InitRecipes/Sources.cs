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
       /*       {RecipesSource.AllRecipes, new Dictionary<MealType, Tuple<string,string, int>>() {
                {MealType.Lunch, new Tuple<string,string,int>( "page","http://allrecipes.com/recipes/17561/lunch/",99999) } ,
                {MealType.Breakfast, new Tuple<string,string,int>( "page","http://allrecipes.com/recipes/78/breakfast-and-brunch",99999) } ,
                {MealType.Dinner, new Tuple<string,string,int>( "page","http://allrecipes.com/recipes/17562/dinner",99999) }
              }
            },*/
               {RecipesSource.Food, new Dictionary<MealType, Tuple<string,string, int>>() {
                  //  {MealType.Dinner, new Tuple<string,string,int>( "pn","http://www.food.com/topic/main-dish",10) },
                    {MealType.Breakfast, new Tuple<string,string,int>( "pn","http://breakfast.food.com",99999) } ,
                    {MealType.Lunch, new Tuple<string,string,int>( "pn","http://lunch.food.com",99999) }
                }
            },
       /*     {RecipesSource.Cookpad, new Dictionary<MealType, Tuple<string,string, int>>() {
                {MealType.Breakfast, new Tuple<string,string,int>("page","https://cookpad.com/us/search/breakfast",10)},
                {MealType.Lunch,  new Tuple<string,string,int>("page","https://cookpad.com/us/search/lunch",10 )},
                {MealType.Dinner, new Tuple<string,string,int>( "page","https://cookpad.com/us/search/dinner" ,10)} }
            },*/

          
        };

    }
}
