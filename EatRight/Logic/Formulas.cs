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
        public static double GetTotalCalories(double proteinWeight=DefaultGrams, double fatWeight= DefaultGrams, double carbsWeight= DefaultGrams) {
            return (proteinWeight * ProteinCarbsGramCalories) + (carbsWeight * ProteinCarbsGramCalories) + (FatGramCalories * 9);
        }
    }
}
