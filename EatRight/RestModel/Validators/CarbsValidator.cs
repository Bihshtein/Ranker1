using RestModel.Validators;
using System.Collections.Generic;
using System.Linq;


namespace RestModel.Validators {
    internal class CarbsValidator : BasicValidator {
        public override bool IsMainPart(string part) {
            return part != part.ToLower();
        }
        public CarbsValidator() {
            SecondParts = new List<string> {
                "whole-groat","homemade",  "refined", "protein-fortified","pearl", "whole-wheat","whole-grain",
                "white","brown","egg","japanese","dark","light","medium","masa","yellow","hulled","pearled"

            };
            ThirdParts = new List<string> {
                 "corn and rice flour","corn", "all-purpose", "bread", "cake", "tortilla mix", "plain","spinach",
                "degermed","vegetable","long-grain","medium-grain","glutinous","self-rising","soba",
                "somen","crude","durum","hard red spring","hard red winter","soft white","hard white","soft red winter"
             };
           
        }
    }
}