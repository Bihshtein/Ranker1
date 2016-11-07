using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitDB.Validators {
    public interface BasicValidator {
        List<string> MainParts { get; }
        List<string> SecondParts { get; }
        List<string> Cuts { get; }
        bool IsSecondPart(string part);
        string GetPrettyName(string part);
        Tuple<string, string> GetNameAndCut(string part);
        bool IsValidPart(string part);
    }
}
