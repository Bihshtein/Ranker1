using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuBuilder.Graders.DailyMenuGraders;
using RestModel;

namespace MenuBuilder.Graders.MenuGraders
{
    class ProductsTasteGrader : PerDayGrader
    {
        public ProductsTasteGrader()
        {
            Description = "Compatibility of products to the user flavor";
            Type = GraderType.ProductsTasteGrader;

            dailyGrader = new ProductsTasteDailyGrader();
        }
    }
}
