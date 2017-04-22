using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    public interface IRecipeParser  {
        string[] IngredientSplitter { get; }
        string[] ServingSplitter { get;  }
        string GetRecipeName(string page);
        IngredientInfo ParseWeightAndName(string ingredient);
        int GetServings(string page);
        int GetStepsNum(string page);
        TimeSpan GetPrepTime(string page);
        string GetImageUrl(string page);
    }

   

    public class GeneralRecipeParser {
        public IRecipeParser Parser;
        public GeneralRecipeParser(IRecipeParser parser) {
            Parser = parser;
        }
        public List<IngredientInfo> GetIngredients(string page) {
            var ingredients = new List<IngredientInfo>();
            var ingredientParts = page.Split(Parser.IngredientSplitter, StringSplitOptions.None);
            for (int i = 1; i < ingredientParts.Length; i++) {
                var res = Parser.ParseWeightAndName(ingredientParts[i]);
                if (res != null )
                    ingredients.Add(res);
            }
            return ingredients;
        }
    }
}
