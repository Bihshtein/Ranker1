using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    
    public class Formulas
    {
        public const double DefaultGrams=100;
        public const double ProteinCarbsGramCalories = 4;
        public const double FatGramCalories = 9;
        public static double GetTotalCalories(double weight, double proteinWeight, double fatWeight, double carbsWeight)
        {
            // Given a product weight, and the protein, fat and carbs weight per 100 garms, return the calories number
            double weightRatio = weight / DefaultGrams;
            return ((proteinWeight * weightRatio) * ProteinCarbsGramCalories) +
                ((carbsWeight * weightRatio) * ProteinCarbsGramCalories) +
                ((FatGramCalories * weightRatio) * 9);
        }

        public static List<string> RelativeSizes = new List<string> {
            // relative to the weight stated by the USDA for the product
            // order matter keep the more inclusicve delimiters first
            "slices",
            "slice ",
            "large",
            "small",
            "medium",
            "cup packed",
            "cup unpacked",
            "cups",
            "cup",
            "cup sliced",
            "cup small",

        };


        public static Dictionary<string, double> MeasuresWeights = new Dictionary<string, double>(){
            {"bunch",60 },
            {"cloves",3 },//garlic
            {"clove",3 },//garlic
            {"pinch",0.25 },//green onion or whatever
            {"teaspoon ",5 },
            {"teaspoons",5 },
            {"tablespoons",14 },
            {"tablespoon ",14 },
            {"pound ",453.5},
            {"pound)",453.5},
            {"pounds",453.5},
            {"ounces",28.3 },
            {"ounce ",28.3 },
            {"ounce)",28.3 },
            {"fluid ounce)",28.3 },
            {"fluid ounce ",28.3 },
            {"fluid ounces",28.3 },
            {"ounce",28.3 },
        };
    }
}
