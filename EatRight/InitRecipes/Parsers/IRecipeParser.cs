using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes {
    public interface IRecipeParser  {
        string[] ServingSplitter { get; set; }
        string GetRecipeName(string page);
        List<Tuple<string, double, string>> GetIngredients(string page);
        int GetServings(string page);
        TimeSpan GetPrepTime(string page);
        string GetImageUrl(string page);
    }
}
