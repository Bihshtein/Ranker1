using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using RecommendationBuilder.Graders.MenuGraders;
using RecommendationBuilder.Graders.DailyMenuGraders;
using RecommendationBuilder.Graders.MealGraders;
using RecommendationBuilder.Graders.RecoGraders;

namespace RecommendationBuilder.Graders
{
    abstract class Grader
    {
        /**
         * Returns a double between 0 and 1.
         */
        public abstract double Grade(RecommendationObject obj);

        public string Description { protected set; get; }

        public GraderType Type { protected set; get; }

        public static Grader GetGraderByType(GraderType type)
        {
            switch (type)
            {
                // Menu graders
                case GraderType.CaloriesCountGrader:
                    return new CaloriesCountGrader();
                case GraderType.CostGrader:
                    return new CostGrader();
                case GraderType.FoodCategoryGrader:
                    return new FoodCategoryGrader();
                case GraderType.MinNutValuesGrader:
                    return new MinNutValuesGrader();
                case GraderType.MaxNutValuesGrader:
                    return new MaxNutValuesGrader();
                case GraderType.ProductsTasteGrader:
                    return new ProductsTasteGrader();
                case GraderType.VarietyGrader:
                    return new VarietyGrader();
                case GraderType.PrepTimeGrader:
                    return new PrepTimeGrader();
                case GraderType.ServingsNumGrader:
                    return new ServingsNumGrader();

                // DailyMenu graders
                case GraderType.CaloriesCountDailyGrader:
                    return new CaloriesCountDailyGrader();
                case GraderType.FoodCategoryDailyGrader:
                    return new FoodCategoryDailyGrader();
                case GraderType.MinNutValuesDailyGrader:
                    return new MinNutValuesDailyGrader();
                case GraderType.MaxNutValuesDailyGrader:
                    return new MaxNutValuesDailyGrader();
                case GraderType.ProductsTasteDailyGrader:
                    return new ProductsTasteDailyGrader();
                case GraderType.VarietyDailyGrader:
                    return new VarietyDailyGrader();
                case GraderType.PrepTimeDailyGrader:
                    return new PrepTimeDailyGrader();
                case GraderType.ServingsNumDailyGrader:
                    return new ServingsNumDailyGrader();

                // Meal graders
                case GraderType.CaloriesCountMealGrader:
                    return new CaloriesCountMealGrader();
                case GraderType.MinNutValuesMealGrader:
                    return new MinNutValuesMealGrader();
                case GraderType.MaxNutValuesMealGrader:
                    return new MaxNutValuesMealGrader();
                case GraderType.FoodCategoryMealGrader:
                    return new FoodCategoryMealGrader();
                case GraderType.ProductsTasteMealGrader:
                    return new ProductsTasteMealGrader();
                case GraderType.PrepTimeMealGrader:
                    return new PrepTimeMealGrader();
                case GraderType.ServingsNumMealGrader:
                    return new ServingsNumMealGrader();

                // Recommendation graders
                case GraderType.VarietyRecoGrader:
                    return new VarietyRecoGrader();
                case GraderType.GradeAverageRecoGrader:
                    return new GradeAverageRecoGrader();

                default:
                    // TODO: better handle errors
                    System.Console.WriteLine("***ERROR*** Grader type " + type.ToString() + " is an unknown type!");
                    System.Environment.Exit(1);
                    return null;
            }
        }
    }
}
