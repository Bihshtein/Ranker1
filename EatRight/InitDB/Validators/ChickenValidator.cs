using System;
using System.Collections.Generic;
using System.Linq;
using InitDB.Validators;

namespace InitDB.Validators {
    internal class ChickenValidator : BasicValidator {
        public ChickenValidator() {
            MainParts = new List<string>()
            { "broilers or fryers", "broiler or fryers", "broiler", "capons",
                "cornish game hens","stewing", "roasting"};
            SecondParts = new List<string>()
            {"feet","leg","thigh","breast","neck","back", "wing", "drumstick","giblets","gizzard",
            "heart","liver","skin (drumsticks and thighs)","skin",
            "and giblets and neck","light meat"};
            Cuts = new List<string>() {"skinless", "meat and skin", "meat only", "skin only"};
        }
      

        public static List<string> NutritionDetails = 
            new List<string>() {"original seasoning","all classes", "dark meat","with added solution" , "with broth", "no broth"};

        public override bool IsValidPart(string part) {
            part = part.Trim();
            var aleg =  (NutritionDetails.Contains(part) || base.IsValidPart(part));
            return aleg;
        }
    }
}