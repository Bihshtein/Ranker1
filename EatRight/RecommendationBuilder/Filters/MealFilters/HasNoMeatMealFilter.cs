using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoMeatMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.IsMeatProduct;
        }

        public HasNoMeatMealFilter()
        {
            hasProperty = false;

            Description = "The meal doesn't contain meat products";
            Type = FilterType.HasNoMeatMealFilter;
        }
    }
}
