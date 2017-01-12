using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class OnlyKosherProductsMealFilter : MealFilter
    {
        public OnlyKosherProductsMealFilter()
        {
            Description = "The meal doesn't contain containt non-kosher products";
            Type = FilterType.OnlyKosherProductsMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return !meal.Recipe.ProductTypes.Contains(ProductType.NonKosher);
        }
    }
}
