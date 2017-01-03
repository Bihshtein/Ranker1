using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoFishMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.Fish);
        }

        public HasNoFishMealFilter()
        {
            hasProperty = false;

            Description = "The meal doesn't contain fish products";
            Type = FilterType.HasNoFishMealFilter;
        }
    }
}
