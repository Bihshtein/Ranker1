using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using System.Text.RegularExpressions;
using System.IO;

namespace InitDailyValuesDB
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
            return dailyValuesList.All(val => val.IsMinValid());
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
                        tempStr = tempStr.Replace("?", "");
                        tempStr = tempStr.Replace("%", "");

                        double val = -1;

                        if (!double.TryParse(tempStr, out val)) return false;

                        if (!dailyValuesList[pIndex].SetMin(nameToProductDVMap[result[0]].Item1, val)) return false;
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
                var nameToProduct = new Dictionary<string, Tuple<string, int>>();
                DailyValue.Map.ToList().ForEach(item => nameToProduct.Add(item.Key, new Tuple<string, int>(item.Value, 0)));
                return nameToProduct;
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
