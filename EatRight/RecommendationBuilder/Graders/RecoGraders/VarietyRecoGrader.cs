using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder.Graders.RecoGraders
{
    class VarietyRecoGrader : RecoGrader
    {
        public VarietyRecoGrader()
        {
            Description = "Variety in products";
            Type = RestModel.GraderType.VarietyRecoGrader;
        }

        protected override double InternalGrade(Recommendation reco)
        {
            var allProducts = GetAllProducts(reco);
            int totalProductsNum = allProducts.Count;
            int repeatedProductsNum = totalProductsNum - allProducts.Distinct().Count();


            return GradeRatio((double)repeatedProductsNum / totalProductsNum);
        }

        private List<string> GetAllProducts(Recommendation reco)
        {
            var res = new List<string>();

            foreach (var meal in reco.MealsSet)
            {
                res.AddRange(meal.Recipe.ProductsWeight.Select(x => x.Key));
            }

            return res;
        }

        private double GradeRatio(double ratio)
        {
            return 1 - ratio;
        }
    }
}
