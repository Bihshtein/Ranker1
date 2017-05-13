using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitRecipes.Parsers.RecipesPageParsers
{
    public class RecipesPageParserFactory
    {
        public static IRecipesPageParser CreateRecipesPageParser(RecipesSource source)
        {
            switch (source)
            {
                case RecipesSource.Food:
                    return new FoodRecipesPageParser();
                case RecipesSource.Epicurious:
                    return new EpicuriousRecipesPageParser();
                default:
                    return new GeneralRecipesPageParser() { source = source };
            }
        }
    }
}
