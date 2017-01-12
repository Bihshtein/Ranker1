using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasNoSeafoodMealFilter : MealFilter
    {
        public HasNoSeafoodMealFilter()
        {
            Description = "The meal doesn't contain seafood products";
            Type = FilterType.HasNoSeafoodMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return !meal.Recipe.ProductTypes.Contains(ProductType.Seafood);
        }
    }
}
