using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasSeafoodMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.Seafood);
        }

        public HasSeafoodMealFilter()
        {
            hasProperty = true;

            Description = "The meal contains seafood products";
            Type = FilterType.HasSeafoodMealFilter;
        }
    }
}
