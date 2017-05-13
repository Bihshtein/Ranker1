using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitProducts {
    class SnacksValidator : BasicValidator  {
        public SnacksValidator() {
            SecondParts = new List<string>() {
                "beef jerky", "granola bars"
            };
            ThirdParts = new List<string>() {
               "microwave","air-popped","cheese-flavor", "chopped and formed" ,
                "hard","soft", "plain","chocolate","almond", "peanut"
            };
        }
    }
}
