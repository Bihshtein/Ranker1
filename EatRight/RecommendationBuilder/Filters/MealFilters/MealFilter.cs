using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    abstract class MealFilter : Filter
    {
        public override Boolean IsValid(RecommendationObject obj)
        {
            if (!(obj is Meal))
            {
                throw new ArgumentException();
            }

            return InternalIsValid((Meal)obj);
        }

        abstract protected Boolean InternalIsValid(Meal meal);
    }
}
