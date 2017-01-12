using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasFishMealFilter : MealFilter
    {
        public HasFishMealFilter()
        {
            Description = "The meal contains fish products";
            Type = FilterType.HasFishMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return meal.Recipe.ProductTypes.Contains(ProductType.Fish);
        }
    }
}
