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

   


    
}
