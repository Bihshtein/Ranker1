using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoDairyMealFilter : MealFilter
    {
        public HasNoDairyMealFilter()
        {
            Description = "The meal doesn't contain dairy products";
            Type = FilterType.HasNoDairyMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return !meal.Recipe.ProductTypes.Contains(ProductType.Dairy);
        }
    }
}
