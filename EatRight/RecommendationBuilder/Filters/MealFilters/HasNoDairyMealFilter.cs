using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoDairyMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.Dairy);
        }

        public HasNoDairyMealFilter()
        {
            hasProperty = false;

            Description = "The meal doesn't contain dairy products";
            Type = FilterType.HasNoDairyMealFilter;
        }
    }
}
