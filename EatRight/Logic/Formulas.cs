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
    }
}
