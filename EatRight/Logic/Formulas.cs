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
        public static List<string> RelativeProductSize = new List<string> {
            "large",
            "small head of",
            "small",
            "medium size of",
            "medium size",
            "medium bunch of",
            "medium",
            "handfuls",
            "handful",

        };
        public static List<string> RelativeSizes = new List<string> {
            // relative to the weight stated by the USDA for the product
            // order matter keep the more inclusicve delimiters first
            " cup sliced",
            " cup small",
            " cup packed",
            " cup unpacked",
            " cups",
            " Cups",
            " cup",
            " Cup",
            " thick slices",
            " thick slice ",
            " slices",
            " slice",
            " large",
            " small",
            " medium",
            " pinch",
            " leaves",
            " bags",
            " bag",
            " Bag",
            " block",
            " Box",
            " box",
            " packages",
            " package",
            " pack",
            " Pack",
            " pints",
            " pint",
            " boxes",
            " loaf",
            " inch",
            " Can",
            " can",
            " jar",
            " pieces",
            " piece",
            " pcs",
            " head",
            " handful",
            " each",
            " Bar",
            " bar",
            " to taste",
            " pkg",
            " bundle",
            " sheet",
            " container",
            " carton",
            " stalk",
            " envelope",
           
        };


        public static Dictionary<string, double> MeasuresWeights = new Dictionary<string, double>(){
            {" bunch",60 },           
            {" dash",5/16 },
            {" Teaspoons",5 },
            {" teaspoons",5 },
            {" tablespoons",14 },
            {" Tablespoon",14 },
            {" tablespoon",14 },
            {" Teaspoon",5 },
            {" teaspoon",5 },
            {" tsp",5 },
            {" Tsp",5 },
            {" TSP",5 },
            {" spoon",14 },
            {" tbsp",14 },
            {" tbls",14 },
            {" tbs",14 },
            {" TBS",14 },
            {" Tbsp",14 },
            {" lbs",453.5},
            {" pound)",453.5},
            {" pounds",453.5},
            {" Pounds",453.5},
            {" pound",453.5},
            {" Pound",453.5},
            {" fluid ounce)",28.3 },
            {" fl oz",28.3 },
            {" fluid ounce",28.3 },
            {" fluid ounces",28.3 },
            {" ounces",28.3 },
            {" ounce",28.3 },
            {" ounce)",28.3 },
            {" Oz",28.3 },        
            {" oz",28.3 },
            {" kg",1000 },
            {" liter",1000 },
            {" quart",1365 },
            {" gallon",3785 },
            {" dozen",12 },
            {" grams",1 },
            {" gram",1 },
            {" bottle",500 },
            {" lb",453.5},
            {" cloves",3 },//garlic
            {" Clove",3 },//garlic
            {" clove",3 },//garlic
          
        };
    }
}

