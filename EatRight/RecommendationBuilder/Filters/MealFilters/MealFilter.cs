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
            if (!(obj is MealWrapper))
            {
                throw new ArgumentException();
            }

            return InternalIsValid((MealWrapper)obj);
        }

        abstract protected Boolean InternalIsValid(MealWrapper meal);
    }
}
