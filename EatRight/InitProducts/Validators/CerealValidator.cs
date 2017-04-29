using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitProducts {
    public class CerealValidator : BasicValidator {
        public CerealValidator() {
            SecondParts = new List<string> { "RALSTON Corn Flakes", "KELLOGG'S Corn Flakes", "KELLOGG'S HONEY CRUNCH CORN FLAKES" };
            ThirdParts = new List<string> { "KELLOGG"};
        }

        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item.ToLower().Contains("corn flakes"))
                item = "corn flakes";
            return base.SimpleSecondPart(item);
        }
    }
}
