using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class VeganMealFilter : MealFilter
    {
        public VeganMealFilter()
        {
            Description = "The meal contain only vegan products";
            Type = FilterType.VeganMealFilter;
        }

        protected override Boolean InternalIsValid(Meal meal)
        {
            return !meal.Recipe.ProductTypes.Contains(ProductType.FromAnimal);
        }
    }
}
