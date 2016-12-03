using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders.MealGraders
{
    class ProductsTasteMealGrader : TasteMealGrader<string>
    {
        public ProductsTasteMealGrader()
        {
            Description = "Compatibility of products to the user flavor";
            Type = GraderType.ProductsTasteMealGrader;
        }

        protected override void InitDataStructures(MenuMeal meal)
        {
            flavorDict = graderDB.productFlavorGrade;
            objectList = meal.Meal.Products;
        }
    }
}
