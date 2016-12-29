using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class MealTypeMealFilter : MealFilter
    {
        private MealType mealWantedType;

        public MealTypeMealFilter()
        {
            Description = "The meal is of type requested by the user";
            Type = RestModel.FilterType.MealTypeMealFilter;

            mealWantedType = ((MealSuggestionRange)RecommendationObject.recommendationDB.range).MealType;
        }

        protected override Boolean InternalIsValid(Meal mealWrapper)
        {
            return mealWrapper.Recipe.Types.Contains(mealWantedType);
        }
    }
}
