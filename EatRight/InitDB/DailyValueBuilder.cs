using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using System.Text.RegularExpressions;

namespace InitDB
{
    /// <summary>
    /// You know how its like when you make shitty design decision just to make things work?
    /// Well here's a taste below:
    /// 
    /// nutrient (units),source of goala,child 1?3,female 4?8,Male 4?8,female 9?13,Male 9?13,female 14?18,Male 14?18,female 19?30,Male 19?30,female 31?50,Male 31?50,female 51+,Male 51+
    /// TODO [hen]: turn the way we read this data using a Nutrition Tables Sources Factory 
    ///             one of the instaces will be a NUTRITION_GOALS_CSV with Author and Date
    /// </summary>
    public class NUTRITION_GOALS_CSV
    {
        public const char DELIMITER             = ',';

        public const int AGE_GENDER_INDEX       = 2;
        public const string AGE_GENDER_PREFIX   = "nutrient (units)";
        public const int AGE_GENDER_FIRST_COL   = 2;
        public const int AGE_GENDER_COL_LENGTH  = 13;
        public const char AGE_GENDER_ALL_DELIM  = ' ';
        public const char AGE_GENDER_AGE_DELIM  = '?';
        public const char AGE_GENDER_AGE_TOP    = '+';

        public static Dictionary<string, GenderType> AGE_GENDER_CSV_MAP = new Dictionary<string, GenderType>
        {
            { "child", GenderType.Both },
            { "female", GenderType.Female },
            { "male", GenderType.Male }
        };
    }

    public class DailyValueBuilder
    {
        /// <summary>
        /// TODO: Move to data structures
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public class TupleList<T1, T2> : List<Tuple<T1, T2>>
        {
            public void Add(T1 item, T2 item2)
            {
                Add(new Tuple<T1, T2>(item, item2));
            }
        }

        private static Tuple<GenderParam, AgeParam> ExtractCSVGenderAgeTuple(string data)
        {
            if ((data == null) || (!data.Contains(NUTRITION_GOALS_CSV.AGE_GENDER_ALL_DELIM))) return null;

            var agegender = data.Split(NUTRITION_GOALS_CSV.AGE_GENDER_ALL_DELIM);
            if ((agegender == null) || (agegender.Length != 2)) return null;

            var gender = agegender[0].ToLower();
            var ageRange = agegender[1];
            var ageParam = AgeParam.FromString(ageRange,
                                               NUTRITION_GOALS_CSV.AGE_GENDER_AGE_DELIM,
                                               NUTRITION_GOALS_CSV.AGE_GENDER_AGE_TOP);
            if (ageParam == null) return null;

            var genderParam = GenderParam.FromString(gender, NUTRITION_GOALS_CSV.AGE_GENDER_CSV_MAP);
            if (genderParam == null) return null;

            return new Tuple<GenderParam, AgeParam>(genderParam, ageParam);
        }

        private static TupleList<GenderParam,AgeParam> GetCSVGenderAgeTupleList(string[] lines)
        {
            if ((lines == null) || (lines.Length <= NUTRITION_GOALS_CSV.AGE_GENDER_INDEX)) return null;

            var genderAgeColumns = 
                ExtractCSVLine(lines[NUTRITION_GOALS_CSV.AGE_GENDER_INDEX]).Skip(NUTRITION_GOALS_CSV.AGE_GENDER_FIRST_COL).ToList();

            if (genderAgeColumns.Count != NUTRITION_GOALS_CSV.AGE_GENDER_COL_LENGTH) return null;

            TupleList<GenderParam, AgeParam> genderAgeList = new TupleList<GenderParam, AgeParam>();

            genderAgeColumns.ForEach(x => genderAgeList.Add(ExtractCSVGenderAgeTuple(x)));

            return genderAgeList;
        }

        private static Dictionary<string, Tuple<string, int>> GetCSVDVTable()
        {
            return new Dictionary<string, Tuple<string, int>>()
            {
                //Macronutrients
                { "Protein (g)" , new Tuple<string, int>("Protein", 0) },
             //   { "Carbohydrate (g)" , new Tuple<string, int>("Protein", 0) },
                { "Total fiber (g)" , new Tuple<string, int>("Fiber", 0) },
           //     { "Total fat (% of calories)" , new Tuple<string, int>("Fat", 0) },
         //       { "Saturated fat (% of calories)" , new Tuple<string, int>("Protein", 0) },
            //    { "Linoleic acid (g)" , new Tuple<string, int>("Protein", 0) },
           //     { "alpha-Linolenic acid (g)" , new Tuple<string, int>("Protein", 0) },
          //      { "Cholesterol (mg)" , new Tuple<string, int>("Protein", 0) },

                //Minerals
                { "Calcium (mg)" , new Tuple<string, int>("Calcium", 0) },
                { "Iron (mg)" , new Tuple<string, int>("Iron", 0) },
                { "Magnesium (mg)" , new Tuple<string, int>("Magnesium", 0) },
                { "Phosphorus (mg)" , new Tuple<string, int>("Phosphorus", 0) },
                { "Potassium (mg)" , new Tuple<string, int>("Potassium", 0) },
                { "Sodium (mg)" , new Tuple<string, int>("Sodium", 0) },
                { "Zinc (mg)" , new Tuple<string, int>("Zinc", 0) },
           //     { "Copper (mcg)" , new Tuple<string, int>("Protein", 0) },
            //    { "Selenium (mcg)" , new Tuple<string, int>("Protein", 0) },

                // vitamins
                { "Vitamin A (mcg RAE)" , new Tuple<string, int>("VitaminA", 0) },
                { "Vitamin Dh (mcg)" , new Tuple<string, int>("VitaminD", 0) },
                { "Vitamin E (mg AT)" , new Tuple<string, int>("VitaminE", 0) },
                { "Vitamin C (mg)" , new Tuple<string, int>("VitaminC", 0) },
                { "Thiamin (mg)" , new Tuple<string, int>("Thiamin", 0) },
                { "Riboflavin (mg)" , new Tuple<string, int>("Riboflavin", 0) },
                { "Niacin (mg)" , new Tuple<string, int>("Niacin", 0) },
                { "Folate (mcg)" , new Tuple<string, int>("Folate", 0) },
                { "Vitamin B6 (mg)" , new Tuple<string, int>("VitaminB6", 0) },
                { "Vitamin B12 (mcg)" , new Tuple<string, int>("VitaminB12", 0) },
           //     { "Choline (mg)" , new Tuple<string, int>("Protein", 0) },
                { "Vitamin K (mcg)" , new Tuple<string, int>("VitaminK", 0) },

                // Missing to daily values (from excel) - all the commented out values

                // Missing from daily values:
                // PantothenicAcid - Supposed to be VitmainB5 but it's not in the table
                // Fat - total fat and total saturated fat is in precetnage,
                //       need to calc it to whatever fat is considered in the code
            };
        }

        private static string[] ExtractCSVLine(string line)
        {
            return Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
        }

        public static List<DailyValue> FromCSVLine(string[] lines)
        {
            var dvAgeGenderList = GetCSVGenderAgeTupleList(lines);
            var dvsTable = GetCSVDVTable();
            bool thrown = false;

            var dvList = new List<DailyValue>(dvAgeGenderList.Count);

            for (int i =0; i < dvAgeGenderList.Count; i++)
            {
                var curr_dv = DailyValue.NullDefault();
                curr_dv.ID = i;
                curr_dv.Gender = dvAgeGenderList[i].Item1;
                curr_dv.Age = dvAgeGenderList[i].Item2;

                dvList.Add(curr_dv);
            }
            foreach (var line in lines)
            {
                string[] result = ExtractCSVLine(line);

                if (dvsTable.ContainsKey(result[0]))
                {
                    if (dvsTable[result[0]].Item2 != 0) { thrown = true; break; }
                    dvsTable[result[0]] = new Tuple<string, int>(dvsTable[result[0]].Item1, 1);

                    for (var pIndex = 0; pIndex < dvAgeGenderList.Count; pIndex++)
                    {
                        var tempStr = result[pIndex + 2];
                        tempStr = tempStr.Replace("<", "");
                        tempStr = tempStr.Replace(",", "");
                        tempStr = tempStr.Replace("\"", "");

                        double val = -1;

                        if (!double.TryParse(tempStr, out val))
                        {
                            thrown = true; break;
                        }

                        if (!dvList[pIndex].Set(dvsTable[result[0]].Item1, val)) { thrown = true;  break; }
                    }
                }
            }

            if (!thrown)
            {
                for (var pIndex = 0; pIndex < dvList.Count; pIndex++)
                {
                    dvList[pIndex].Set("Fat", 65);
                    dvList[pIndex].Set("PantothenicAcid", 15);

                    if (!dvList[pIndex].IsValid()) { thrown = true; break; }
                }
            }
            
            if (thrown) return null;

            return dvList;
        }
    }
}
