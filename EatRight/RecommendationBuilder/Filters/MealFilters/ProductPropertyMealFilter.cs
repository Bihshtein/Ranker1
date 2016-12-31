using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    abstract class ProductPropertyMealFilter : MealFilter
    {
        protected Boolean hasProperty;

        abstract protected Boolean HasProperty(Product product);

        protected override Boolean InternalIsValid(Meal meal)
        {
            foreach (var entry in meal.Recipe.ProductsWeight)
            {
                var prodName = entry.Key;
                var product = Queries<Product>.GetMatchingProductsForIngredient(prodName)[0];
                if (HasProperty(product))
                {
                    return hasProperty;
                }
            }
            return !hasProperty;
        }
    }
}
