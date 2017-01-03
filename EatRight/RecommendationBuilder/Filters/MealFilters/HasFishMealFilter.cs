using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasFishMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.Fish);
        }

        public HasFishMealFilter()
        {
            hasProperty = true;

            Description = "The meal contains fish products";
            Type = FilterType.HasFishMealFilter;
        }
    }
}
