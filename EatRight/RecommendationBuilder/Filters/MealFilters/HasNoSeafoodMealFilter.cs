using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoSeafoodMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.Seafood);
        }

        public HasNoSeafoodMealFilter()
        {
            hasProperty = false;

            Description = "The meal doesn't contain seafood products";
            Type = FilterType.HasNoSeafoodMealFilter;
        }
    }
}
