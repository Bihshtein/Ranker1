using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class NoMeatDairyMealFilter : MealFilter
    {
        public NoMeatDairyMealFilter()
        {
            Description = "The meal doesn't contain meat product and dairy products";
            Type = FilterType.NoMeatDairyMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return !(meal.Recipe.ProductTypes.Contains(ProductType.Dairy) && meal.Recipe.ProductTypes.Contains(ProductType.Meat));
        }
    }
}
