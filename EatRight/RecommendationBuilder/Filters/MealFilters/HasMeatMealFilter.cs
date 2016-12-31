using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasMeatMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.IsMeatProduct;
        }

        public HasMeatMealFilter()
        {
            hasProperty = true;

            Description = "The meal contains meat products";
            Type = FilterType.HasMeatMealFilter;
        }
    }
}
