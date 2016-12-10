using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using System.Text.RegularExpressions;
using System.IO;

namespace InitDB
{
    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 item, T2 item2)
        {
            Add(new Tuple<T1, T2>(item, item2));
        }
    }

    public interface IDailyValuesProvider
    {
        List<DailyValue> FromFile(string filePath);
    }

    /// <summary>
    /// You know how its like when you make shitty design decision just to make things work?
    /// Well here's a taste below:
    /// 
    /// nutrient (units),source of goala,child 1?3,female 4?8,Male 4?8,female 9?13,Male 9?13,female 14?18,Male 14?18,female 19?30,Male 19?30,female 31?50,Male 31?50,female 51+,Male 51+
    /// TODO [hen]: turn the way we read this data using a Nutrition Tables Sources Factory 
    ///             one of the instaces will be a NUTRITION_GOALS_CSV with Author and Date
    /// </summary>
    public class NutritionGoalsCSV : IDailyValuesProvider
    {
        private static string[] Split(string line)
        {
            return Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
        }
        
        public List<DailyValue> FromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var ageGenderList = AgeGenderRow.GetGenderAgeList(lines);
            var nameToProductDVMap = NutrientRow.NameToProductDV();

            var dailyValuesList = GetDefaultDailyValuesList(ageGenderList);
            if (!PopulateDailyValuesListFromLines(dailyValuesList, lines, 
                                                  ageGenderList, nameToProductDVMap)) return null;
            if (!ValidateDailyValuesList(dailyValuesList)) return null;

            return dailyValuesList;
        }

        private bool ValidateDailyValuesList(List<DailyValue> dailyValuesList)
        {
            for (var pIndex = 0; pIndex < dailyValuesList.Count; pIndex++)
            {
                dailyValuesList[pIndex].Set("Fat", 65);
                dailyValuesList[pIndex].Set("PantothenicAcid", 15);

                if (!dailyValuesList[pIndex].IsValid()) { return false; }
            }

            return true;
        }

        private bool PopulateDailyValuesListFromLines(List<DailyValue> dailyValuesList, string[] lines, 
                                                      TupleList<GenderParam, AgeParam> ageGenderList, 
                                                      Dictionary<string, Tuple<string, int>> nameToProductDVMap)
        {
            foreach (var line in lines)
            {
                string[] result = Split(line);

                if (nameToProductDVMap.ContainsKey(result[0]))
                {
                    if (nameToProductDVMap[result[0]].Item2 != 0) return false; 
                    nameToProductDVMap[result[0]] = new Tuple<string, int>(nameToProductDVMap[result[0]].Item1, 1);

                    for (var pIndex = 0; pIndex < ageGenderList.Count; pIndex++)
                    {
                        var tempStr = result[pIndex + 2];
                        tempStr = tempStr.Replace("<", "");
                        tempStr = tempStr.Replace(",", "");
                        tempStr = tempStr.Replace("\"", "");

                        double val = -1;

                        if (!double.TryParse(tempStr, out val)) return false;

                        if (!dailyValuesList[pIndex].Set(nameToProductDVMap[result[0]].Item1, val)) return false;
                    }
                }
            }

            return true;
        }

        private List<DailyValue> GetDefaultDailyValuesList(TupleList<GenderParam, AgeParam> ageGenderList)
        {
            var dvList = new List<DailyValue>(ageGenderList.Count);

            var i = 0;
            ageGenderList.ForEach(ageGender => dvList.Add(DailyValue.NullDefault(ageGender, i++)));

            return dvList;
        }

        public class NutrientRow
        {
            public static Dictionary<string, Tuple<string, int>> NameToProductDV()
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
        }

        public class AgeGenderRow
        {
            public const char Delimiter = ',';
            public const int Index = 2;
            public const string Prefix = "nutrient (units)";
            public const int FirstCol = 2;
            public const int ColLength = 13;
            public const char AllDelim = ' ';
            public const char AgeRangeDelim = '?';
            public const char AgeTopChar = '+';

            public static Dictionary<string, GenderType> StringToGenderType =
                new Dictionary<string, GenderType> {
                    { "child", GenderType.Any },
                    { "female", GenderType.Female },
                    { "male", GenderType.Male }
                };


            private static Tuple<GenderParam, AgeParam> GetGenderAge(string data)
            {
                if ((data == null) || (!data.Contains(AllDelim))) return null;

                var agegender = data.Split(AllDelim);
                if ((agegender == null) || (agegender.Length != 2)) return null;

                var gender = agegender[0].ToLower();
                var ageRange = agegender[1];
                var ageParam = AgeParam.FromString(ageRange,
                                                   AgeRangeDelim,
                                                   AgeTopChar);
                if (ageParam == null) return null;

                var genderParam = GenderParam.FromString(gender, StringToGenderType);
                if (genderParam == null) return null;

                return new Tuple<GenderParam, AgeParam>(genderParam, ageParam);
            }

            public static TupleList<GenderParam, AgeParam> GetGenderAgeList(string[] lines)
            {
                if ((lines == null) || (lines.Length <= Index)) return null;

                var genderAgeColumns = Split(lines[Index]).Skip(FirstCol).ToList();

                if (genderAgeColumns.Count != ColLength) return null;

                TupleList<GenderParam, AgeParam> genderAgeList = new TupleList<GenderParam, AgeParam>();

                genderAgeColumns.ForEach(x => genderAgeList.Add(GetGenderAge(x)));

                return genderAgeList;
            }
        }

    }

    public class DailyValueBuilder
    {
        public static List<DailyValue> FromFile(string filePath, IDailyValuesProvider dvProvider)
        {
            return dvProvider.FromFile(filePath);
        }
    }
}
