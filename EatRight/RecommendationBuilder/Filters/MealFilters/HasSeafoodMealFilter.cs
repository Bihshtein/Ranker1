using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class HasSeafoodMealFilter : MealFilter
    {
        public HasSeafoodMealFilter()
        {
            Description = "The meal contains seafood products";
            Type = FilterType.HasSeafoodMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return meal.Recipe.ProductTypes.Contains(ProductType.Seafood);
        }
    }
}
