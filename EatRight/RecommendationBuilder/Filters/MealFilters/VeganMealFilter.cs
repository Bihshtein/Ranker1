using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationBuilder.Filters.MealFilters
{
    class VeganMealFilter : ProductPropertyMealFilter
    {
        protected override bool HasProperty(Product product)
        {
            return product.Types.Contains(ProductType.FromAnimal);
        }

        public VeganMealFilter()
        {
            hasProperty = false;

            Description = "The meal contain only vegan products";
            Type = FilterType.VeganMealFilter;
        }
    }
}
