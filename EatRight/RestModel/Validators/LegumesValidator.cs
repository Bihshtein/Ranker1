﻿using RestModel.Validators;
using System.Collections.Generic;

namespace RestModel {
    internal class LegumesValidator : BasicValidator {
        public LegumesValidator() {
            MainParts = new List<string> { "Soy sauce" };
            SecondParts = new List<string> { "made from hydrolyzed vegetable protein" };

        }
    }
}