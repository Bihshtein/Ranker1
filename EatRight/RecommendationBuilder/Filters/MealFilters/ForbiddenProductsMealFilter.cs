using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class ForbiddenProductsMealFilter : MealFilter
    {
        private HashSet<string> forbiddenProducts;

        public ForbiddenProductsMealFilter()
        {
            Description = "Existence of products that were declared as forbidden";
            Type = RestModel.FilterType.ForbiddenProductsMealFilter;

            forbiddenProducts = RecommendationObject.recommendationDB.forbiddenProducts;
        }

        protected override Boolean InternalIsValid(Meal mealWrapper)
        {
            if (forbiddenProducts == null || forbiddenProducts.Count == 0)
            {
                return true;
            }

            foreach (var prodName in mealWrapper.Recipe.ProductsWeight.Keys)
            {
                if (forbiddenProducts.Contains(prodName))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
