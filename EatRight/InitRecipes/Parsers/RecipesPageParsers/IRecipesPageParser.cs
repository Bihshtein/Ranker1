using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes.Parsers.RecipesPageParsers
{
    public interface IRecipesPageParser
    {
        void AddRecipesFromPage(string pageStr, RestDBInterface unit);
    }
}
