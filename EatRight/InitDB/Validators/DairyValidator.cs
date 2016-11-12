using InitDB.Validators;
using System.Collections.Generic;
using System.Linq;

namespace InitDB.Validators {
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
                "provolone", "fruit","strawberry","Greek","plain","vanilla","chocolate"
            };
            Cuts = new List<string> {
               "hard","queso blanco","soft serve","flakes", "queso fresco","queso seco","hard-boiled"
               ,"stabilized","stick or nugget","evaporated", "bar or stick","semi solid",
                "instant","flavored", "part skim milk",
                "grated", "part-skim","queso cotija","queso chihuahua","queso asadero",
                "queso anejo", "half and half", "soft type","powder",
                "semisoft type", "hard type","whole", "yolk","white",
                "shredded","sliced","creamed","uncreamed","omelet","hard boiled","scrambled"
            };
        }

        public override bool IsMainPart(string part) {
            return part != part.ToLower();
        }

        public override bool IsSecondPart(string part) {
            return SecondParts.Any((item) => part.Contains(item));
        }
    }
}