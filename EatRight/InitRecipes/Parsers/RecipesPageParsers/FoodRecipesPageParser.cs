using RestModel;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes.Parsers.RecipesPageParsers
{
    public class FoodRecipesPageParser : IRecipesPageParser
    {
        public void AddRecipesFromPage(string pageStr, RestDBInterface unit)
        {
            var parts = pageStr.Split(new string[1] { "var searchResults = " }, StringSplitOptions.None);
            parts = parts[1].Split(new string[1] { ";\r\n" }, StringSplitOptions.None);
            var json = parts[0];
            var res = JsonConvert.DeserializeObject<dynamic>(json);
            JArray results = res.response.results;
            results.ToList().ForEach(r => {
                if (r["record_type"].ToString() == "Recipe")
                {
                    var id = r["recipe_id"].ToString();
                    ParseRecipes.RecipeIds.Add(id);
                }
            }
            );
        }
    }
}
