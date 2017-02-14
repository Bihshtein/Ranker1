using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public enum GenderType {
        Undefined = 0,
        Female,
        Male,
        Any
    }

    public class BodyProfile {
        public int Age { get; set; } = 30;
        public GenderType Gender { get; set; }
        public bool Pregnant { get; set; }
        public int Weight { get; set; } = 70;
        public int Height { get; set; } = 175;
        public PhysicalActivityLevel ActivityLevel{ get; set; } = PhysicalActivityLevel.Sedentary;
    }

    public enum PhysicalActivityLevel {
        Sedentary, // Little to no exercise
        LightlyActive, // Light exercise, 1-3 days a week
        ModeratelyActive, // Moderate exercise, 3-5 days a week
        VeriActive, // Hard exercise, 6-7 days a week
        ExtraActive // Very hard exercise, twice a day
    }

    public class Formulas
    {

            public static readonly int INVALID_DAILY_CALORIES = -1;
            public static int GetCalories(BodyProfile userProfile) {
                if (userProfile == null) return -1;

                // For adults, we apply the Mifflin - St. Jeor equation
                if (userProfile.Age >= 18) {
                    var bmr = -1;
                    // Equation for men
                    if (userProfile.Gender == GenderType.Male) {
                        bmr = (int)(10 * userProfile.Weight + 6.25 * userProfile.Height - 5 * userProfile.Age + 5);
                    }
                    // Equation for women
                    if (userProfile.Gender == GenderType.Female) {
                        bmr = (int)(10 * userProfile.Weight + 6.25 * userProfile.Height - 5 * userProfile.Age - 161);
                    }
                    if (bmr > 0) {
                        switch (userProfile.ActivityLevel) {
                            case PhysicalActivityLevel.Sedentary:
                                return (int)(bmr * 1.2);
                            case PhysicalActivityLevel.LightlyActive:
                                return (int)(bmr * 1.375);
                            case PhysicalActivityLevel.ModeratelyActive:
                                return (int)(bmr * 1.55);
                            case PhysicalActivityLevel.VeriActive:
                                return (int)(bmr * 1.725);
                            case PhysicalActivityLevel.ExtraActive:
                                return (int)(bmr * 1.9);
                        }
                    }
                }

                // For children, we follow known-heuristics
                else {
                    if (userProfile.Age < 4) {
                        return 900 + 100 * (userProfile.Age - 1);
                    }
                    else if (userProfile.Age < 9) {
                        return 1200 + 50 * (userProfile.Age - 4);
                    }
                    else if (userProfile.Age < 14) {
                        if (userProfile.Gender == GenderType.Male) {
                            return 1600 + 100 * (userProfile.Age - 9);
                        }
                        else if (userProfile.Gender == GenderType.Female) {
                            return 1400 + 50 * (userProfile.Age - 9);
                        }
                    }
                    else // 14-17
                    {
                        if (userProfile.Gender == GenderType.Male) {
                            return 2000 + 100 * (userProfile.Age - 13);
                        }
                        else if (userProfile.Gender == GenderType.Female) {
                            return 1800;
                        }
                    }
                }

                // Default value
                return 3000;
        }

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
            {"dash ",5/16 },
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

