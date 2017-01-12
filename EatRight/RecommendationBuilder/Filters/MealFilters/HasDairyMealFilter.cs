using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasDairyMealFilter : MealFilter
    {
        public HasDairyMealFilter()
        {
            Description = "The meal contains dairy products";
            Type = FilterType.HasDairyMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return meal.Recipe.ProductTypes.Contains(ProductType.Dairy);
        }
    }
}
