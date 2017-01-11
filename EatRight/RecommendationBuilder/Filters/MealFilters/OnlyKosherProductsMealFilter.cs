using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class OnlyKosherProductsMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.NonKosher);
        }

        public OnlyKosherProductsMealFilter()
        {
            hasProperty = false;

            Description = "The meal doesn't contain containt non-kosher products";
            Type = FilterType.OnlyKosherProductsMealFilter;
        }
    }
}
