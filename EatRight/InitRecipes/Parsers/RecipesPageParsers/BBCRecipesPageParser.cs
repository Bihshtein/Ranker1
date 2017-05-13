using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes.Parsers.RecipesPageParsers
{
    public class BBCRecipesPageParser : IRecipesPageParser
    {
        public void AddRecipesFromPage(string pageStr, RestDBInterface unit)
        {
            // Split the whole page str by recipe URLs. We assume that each recipe URL on this page is relevant
            string[] parts = pageStr.Split(new string[] { Sources.RecipesURNs[RecipesSource.BBC].PageSplitterStr }, StringSplitOptions.None);
            for (int i = 1; i < parts.Length; i++)
            {

                string[] splittedPart = parts[i].Split(new string[] { "/" }, StringSplitOptions.None);
                if (splittedPart.Length == 0)
                {
                    continue;
                }
                var idStr = splittedPart[0];
                ParseRecipes.RecipeIds.Add(idStr);
            }
        }
    }
}
