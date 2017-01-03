using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasDairyMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.Dairy);
        }

        public HasDairyMealFilter()
        {
            hasProperty = true;

            Description = "The meal contains dairy products";
            Type = FilterType.HasDairyMealFilter;
        }
    }
}
