using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitProducts {
    public class CerealValidator : BasicValidator {
        public CerealValidator() {
            SecondParts = new List<string> {
                "RALSTON Corn Flakes",
            };
            ThirdParts = new List<string> { "KELLOGG"};
        }

        public override bool IsSecondPart(string part) {
            if (part.ToLower().Contains("kellogg's") || part.ToLower().Contains("cheerios"))
                return true;
            return base.IsSecondPart(part);
        }

        public override bool IsThirdPart(string part) {
            if (part.ToLower().Contains("general mills"))
                return true;
            return base.IsThirdPart(part);
        }

        public override Tuple<string, string> SimpleSecondPart(string item) {
            if (item.ToLower().Contains("corn flakes"))
                item = "corn flakes";
            else if (item.ToLower().Contains("all-bran"))
                item = "all-bran";
            else if (item.ToLower().Contains("rice krispies"))
                item = "rice krispies";
            return base.SimpleSecondPart(item);
        }
    }
}
