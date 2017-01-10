﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RestModel.Validators {
    public abstract class BasicValidator {
        public List<string> MainParts { get; protected set; }
        public List<string> SecondParts { get; protected set; }
        public List<string> ThirdParts { get; protected set; }

        public virtual bool IsSecondPart(string part) {
            if (ThirdParts != null)
                return ThirdParts.Any((cut) => 
                SecondParts.Any(s => s.Equals(Regex.Replace(part, cut, string.Empty, RegexOptions.IgnoreCase).Trim(),
                StringComparison.OrdinalIgnoreCase)));
            return ((SecondParts != null) && SecondParts.Contains(part));

        }

        public virtual bool IsMainPart(string part) {
            if (part == string.Empty)
                return true;
            else if (MainParts != null)
                return MainParts.Any(s => s.Equals(part, StringComparison.OrdinalIgnoreCase));
            else
                return IsName(part) && !IsSecondPart(part) && !IsThirdPart(part);
        }

        public bool IsName(string part) {
            if (Char.IsLower(part[0]))
                return false;
            for (int i = 1; i < part.Length; i++) {
                if (Char.IsUpper(part[i])) {
                    return false;
                }
            }
            return true;
        }

        public virtual bool IsThirdPart(string part) {
            return ThirdParts != null && ThirdParts.Any(s => s.Equals(part, StringComparison.OrdinalIgnoreCase));
        }

        public virtual string GetPrettyName(string part){ return part; } 

        public virtual bool IsValidPart(string part) {
            part = part.Trim();
            return (CommonValidator.IsCommonParameter(part) ||
                    IsMainPart(part) || IsSecondPart(part)|| IsThirdPart(part));
        }

        public virtual Tuple<string, string> GetNameAndDescription(string item) {
            string hiddenCut = string.Empty;
            if (ThirdParts != null) 
                hiddenCut = ThirdParts.FirstOrDefault((cut) => item.Contains(cut));
            if (hiddenCut != null && hiddenCut != string.Empty) {
                item = item.Replace(hiddenCut, string.Empty).Trim();
                hiddenCut = hiddenCut.Trim();
            }
            return new Tuple<string, string>(item, hiddenCut);
        }
    }

}
