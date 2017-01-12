using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasMeatMealFilter : MealFilter
    {
        public HasMeatMealFilter()
        {
            Description = "The meal contains meat products";
            Type = FilterType.HasMeatMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return meal.Recipe.ProductTypes.Contains(ProductType.Meat);
        }
    }
}
