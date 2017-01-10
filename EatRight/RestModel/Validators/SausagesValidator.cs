using RestModel.Validators;
using System.Collections.Generic;

namespace RestModel {
    internal class SausagesValidator : BasicValidator {
        public SausagesValidator() {
            SecondParts = new List<string> {
                "turkey", "beef" , "beef and pork", "pork and beef", "chicken","meat", "meat and poultry",
                "chicken liver","goose liver","liver", "beef","pork", "Berliner","Polish","Vienna",
                "smoked link sausage", 
            };
            ThirdParts = new List<string> { "sliced" ,"Italian"};
        }
    }
}