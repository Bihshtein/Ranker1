using System;
using System.Collections.Generic;
using System.Linq;

namespace InitProducts {
    internal class PoultryValidator : BasicValidator {
        public PoultryValidator() {
            MainParts = new List<string> { "Chicken", "Turkey", "Duck" };
            SecondParts = new List<string>{
                "broilers or fryers", "broiler or fryers", "broiler", "capons",
                "cornish game hens","stewing", "roasting"};
            ThirdParts = new List<string>(){
            "feet","leg","thigh","breast","neck","back", "wing", "drumstick","giblets","gizzard",
            "heart","liver","skin (drumsticks and thighs)","skin",
            "and giblets and neck","light meat","all classes", "dark meat", "with added solution"};

        }
    }
}