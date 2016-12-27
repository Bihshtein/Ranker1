using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using RecommendationBuilder.Filters.MealFilters;

namespace RecommendationBuilder.Filters
{
    abstract class Filter
    {
        /*
         * A filter is a method for filtering unwanted objects.
         * IsValid says if this object is valid according to this filter.
         */
        public abstract Boolean IsValid(RecommendationObject obj);

        public string Description { protected set; get; }

        public FilterType Type { protected set; get; }

        public static Filter GetFilterByType(FilterType type)
        {
            switch (type)
            {
                // Menu filters

                // DailyMenu filters

                // Meal filters
                case FilterType.ForbiddenProductsMealFilter:
                    return new ForbiddenProductsMealFilter();
                case FilterType.MealTypeMealFilter:
                    return new MealTypeMealFilter();

                default:
                    // TODO: better handle errors
                    System.Console.WriteLine("***ERROR*** Filter type " + type.ToString() + " is an unknown type!");
                    System.Environment.Exit(1);
                    return null;
            }
        }
    }
}
