using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB.Validators {
    public abstract class BasicValidator {
        public List<string> MainParts { get; protected set; }
        public List<string> SecondParts { get; protected set; }
        public List<string> ThirdParts { get; protected set; }

        public virtual bool IsSecondPart(string part) {
            if (ThirdParts != null)
                return ThirdParts.Any((cut) => SecondParts.Contains(part.Replace(cut, string.Empty).Trim()));
            return ((SecondParts != null) && SecondParts.Contains(part));

        }

        public virtual bool IsMainPart(string part) {
            if (part == string.Empty)
                return true;
            else if (MainParts != null) 
                return MainParts.Contains(part);
            else 
                return Char.IsUpper(part[0]);
        }

        public virtual bool IsThirdPart(string part) {
            return ThirdParts != null && ThirdParts.Contains(part);
        }

        public virtual string GetPrettyName(string part){ return part; } 

        public virtual bool IsValidPart(string part) {
            part = part.Trim();
            return (CommonValidator.IsCommonParameter(part) ||
                    IsMainPart(part) || IsSecondPart(part)|| IsThirdPart(part));
        }

        public virtual Tuple<string, string> GetNameAndDescription(string item) {
            string hiddenCut = string.Empty;
            if (ThirdParts != null) { }
                hiddenCut = ThirdParts.FirstOrDefault((cut) => item.Contains(cut));
            if (hiddenCut != null && hiddenCut != string.Empty) {
                item = item.Replace(hiddenCut, string.Empty).Trim();
                hiddenCut = hiddenCut.Trim();
            }
            return new Tuple<string, string>(item, hiddenCut);
        }
    }

}
