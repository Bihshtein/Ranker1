using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoFishMealFilter : MealFilter
    {
        public HasNoFishMealFilter()
        {
            Description = "The meal doesn't contain fish products";
            Type = FilterType.HasNoFishMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return !meal.Recipe.ProductTypes.Contains(ProductType.Fish);
        }
    }
}
