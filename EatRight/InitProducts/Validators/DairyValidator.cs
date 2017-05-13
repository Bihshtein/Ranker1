﻿using System.Collections.Generic;
using System.Linq;

namespace InitProducts {
    public class DairyValidator : BasicValidator {
        public DairyValidator() {
        
            SecondParts = new List<string> {
               "human","producer","indian buffalo", "imitation","sheep","Mexican blend",
                "milkshake mix","instant breakfast powder", "acid","light","sour", "sweet","cottage",
                "turkey","duck","goose","quail","liquid", "port de salut",
                "pimento", "cheddar or American", "American", "powdered",
                "buttermilk", "filled","fluid","dry", "cheshire", "colby","gjetost",
                "edam","feta","fontina","cheddar", "blue","brick","brie","camembert","caraway",
                "cream","cotage","gouda","goat", "gruyere","limburger","mexican","monterey", "mozzarella",
                "muenster","neufchatel","parmesan","ricotta", "romano","roquefort","swiss","tilsit","white",
                "provolone", "fruit","strawberry","plain","vanilla","chocolate"
            };
            ThirdParts = new List<string> {
               "hard","queso blanco","soft serve","flakes", "queso fresco","queso seco","hard-boiled"
               ,"stabilized","stick or nugget","evaporated", "bar or stick","semi solid",
                "instant","flavored", "part skim milk",
                "grated", "part-skim","queso cotija","queso chihuahua","queso asadero",
                "queso anejo", "half and half", "soft type","powder",
                "semisoft type", "hard type","whole", "yolk","white","Greek",
                "shredded","sliced","creamed","uncreamed","omelet","hard boiled","scrambled",
                "heavy whipping","light whipping",
            };
        }


        public override bool IsSecondPart(string part) {
            return SecondParts.Any((item) => part.Contains(item));
        }

        public override string SimpleThirdPart(string part) {
            if (part.Contains("whipping"))
                return part.Replace("whipping","").Trim();
            return base.SimpleThirdPart(part);
        }
    }
}