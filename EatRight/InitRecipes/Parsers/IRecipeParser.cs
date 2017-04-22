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
        public string GetImageUrl(string page) {
            try {
                return Parser.GetImageUrl(page);
            }
            catch {
                return null;
            }
        }

        public int GetServings(string page) {
            try {
                return Parser.GetServings(page);
            }
            catch {
                return 1;
            }
        }
        public List<IngredientInfo> GetIngredients(string page) {
            var ingredients = new List<IngredientInfo>();
            var ingredientParts = page.Split(Parser.IngredientSplitter, StringSplitOptions.None);
            for (int i = 1; i < ingredientParts.Length; i++) {
                var res = Parser.ParseWeightAndName(ingredientParts[i].ToLower());
                if (res != null )
                    ingredients.Add(res);
            }
            return ingredients;
        }
    }
}
