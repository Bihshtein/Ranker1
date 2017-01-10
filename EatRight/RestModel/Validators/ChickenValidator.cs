using System;
using System.Collections.Generic;
using System.Linq;
using RestModel.Validators;

namespace RestModel.Validators {
    internal class ChickenValidator : BasicValidator {
        public ChickenValidator() {
            MainParts = new List<string>()
            { "broilers or fryers", "broiler or fryers", "broiler", "capons",
                "cornish game hens","stewing", "roasting"};
            SecondParts = new List<string>()
            {"feet","leg","thigh","breast","neck","back", "wing", "drumstick","giblets","gizzard",
            "heart","liver","skin (drumsticks and thighs)","skin",
            "and giblets and neck","light meat"};
            ThirdParts = new List<string>() { "all classes", "dark meat", "with added solution" };

        }
    }
}