using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB.Validators {
    public abstract class BasicValidator {
        public List<string> MainParts { get; protected set; }
        public List<string> SecondParts { get; protected set; }
        public List<string> Cuts { get; protected set; }

        public bool IsSecondPart(string part) {
            if (Cuts != null)
                return Cuts.Any((cut) => SecondParts.Contains(part.Replace(cut, string.Empty).Trim()));
            else
                return
                    SecondParts.Contains(part);
        }

        public virtual string GetPrettyName(string part){ return part; } 
        public virtual bool IsValidPart(string part) {
            part = part.Trim();
            var aleg = (CommonValidator.IsCommonParameter(part) ||
                    SecondParts!=null && SecondParts.Contains(part) ||
                    MainParts != null && MainParts.Contains(part) ||
                    SecondParts != null && IsSecondPart(part)||
                    Cuts != null &&  Cuts.Contains(part)
                    );
            return aleg;
        }

        public virtual Tuple<string, string> GetNameAndCut(string item) {
            var hiddenCut = Cuts.FirstOrDefault((cut) => item.Contains(cut));
            if (hiddenCut == null)
                hiddenCut = string.Empty;
            else
                item = item.Replace(hiddenCut, string.Empty);
            return new Tuple<string, string>(item, hiddenCut);
        }
    }

}
