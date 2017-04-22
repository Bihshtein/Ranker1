using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitProducts {
    public class FastFoodsValidator : BasicValidator {
        public FastFoodsValidator() {
            MainParts = new List<string> {"potatoes" };
            SecondParts = new List<string> { "hash browns" };
            ThirdParts = new List<string> { "round pieces or patty" };
        }
    }
}
