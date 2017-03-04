using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitProducts {
    class SnacksValidator : BasicValidator  {
        public SnacksValidator() {
            SecondParts = new List<string>() { "beef jerky" };
            ThirdParts = new List<string>() { "chopped and formed" };
        }
    }
}
