using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class NoMeatDairyMealFilter : MealFilter
    {
        public NoMeatDairyMealFilter()
        {
            Description = "The meal doesn't contain meat product and dairy products";
            Type = FilterType.NoMeatDairyMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            var hasMeat = false;
            var hasDairy = false;

            foreach (var entry in meal.Recipe.ProductsWeight)
            {
                var prodName = entry.Key;
                var product = Queries<Product>.GetMatchingProductsForIngredient(prodName)[0];
                if (product.Types.Contains(ProductType.Meat))
                {
                    if (hasDairy)
                    {
                        return false;
                    }
                    hasMeat = true;
                }
                if (product.Types.Contains(ProductType.Dairy))
                {
                    if (hasMeat)
                    {
                        return false;
                    }
                    hasDairy = true;
                }
            }
            return (!hasMeat) || (!hasDairy);
        }
    }
}
