using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoMeatMealFilter : MealFilter
    {
        public HasNoMeatMealFilter()
        {
            Description = "The meal doesn't contain meat products";
            Type = FilterType.HasNoMeatMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return !meal.Recipe.ProductTypes.Contains(ProductType.Meat);
        }
    }
}
