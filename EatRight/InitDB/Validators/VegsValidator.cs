using InitDB.Validators;

namespace InitDB.Validators {
    public class VegsValidator : BasicValidator {
        public override bool IsValidPart(string part) {
            part = part.Trim();
            var aleg = ((part != part.ToLower()) || base.IsValidPart(part));
            return aleg;
        }
    }
}