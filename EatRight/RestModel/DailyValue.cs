using Logic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestModel
{

    public abstract class DailyValueParam<T>
    {
        public abstract bool Within(T param);
    }

    public class AgeParam : DailyValueParam<int>
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public AgeParam()
        {
            MinAge = int.MaxValue;
            MaxAge = int.MaxValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is int) return Equals((int)obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return MinAge^MaxAge;
        }

        public bool Equals(int param)
        {
            return Within(param);
        }

        public AgeParam(int min, int max)
        {
            if (min <= max)
            {
                MinAge = min;
                MaxAge = max;
                return;
            }

            throw new ArgumentException("Min age > Max Age");
        }

        protected AgeParam(AgeParam dup)
        {
            if (dup != null)
            {
                this.MinAge = dup.MinAge;
                this.MaxAge = dup.MaxAge;
            }

            throw new ArgumentNullException("dup");
        }

        public override bool Within(int param)
        {
            return ((param >= MinAge) && (param <= MaxAge));
        }

        public AgeParam Clone()
        {
            return new AgeParam(this);
        }

        public override string ToString()
        {
            return this.MinAge + "-" + this.MaxAge;
        }

        public static AgeParam FromString(string ageRange, char rangeDelim, char rangeTop)
        {
            if ( (ageRange == null) || (!ageRange.Contains(rangeDelim) && !ageRange.Contains(rangeTop)) )   return null;

            int minAge, maxAge;
            minAge = maxAge = -1;

            var top = ageRange.Contains(rangeTop);
            if (top) maxAge = 130;
           
            var range = ageRange.Split(top ? rangeTop : rangeDelim);
            if ((range == null) || (range.Length != 2)) return null;

            int.TryParse(range[0], out minAge);
            if (!top) int.TryParse(range[1], out maxAge);

            if ((minAge == -1) || (maxAge == -1) || (minAge > maxAge)) return null;
            return new AgeParam(minAge, maxAge);
        }
    }

  

    public class GenderParam : DailyValueParam<GenderType>
    {
        public GenderType Type { get; set; }

    

        public override string ToString()
        {
            return Type.ToString();
        }

        public override bool Within(GenderType param)
        {
            if ((Type == GenderType.Undefined) || (param == GenderType.Undefined)) return false;
            return ((param == Type) || (Type == GenderType.Any));
        }

        public static GenderParam FromString(string gender, Dictionary<string, GenderType> StringToGenderType)
        {
            if ((StringToGenderType == null) || (!StringToGenderType.ContainsKey(gender))) return null;

            var ret = new GenderParam();
            ret.Type = StringToGenderType[gender];
            return ret;
        }
    }

    public class MinMaxDouble
    {
        [BsonElement("MinValue")]
        public double MinValue { get; set; }
        [BsonElement("MaxValue")]
        public double MaxValue { get; set; }
    }
    public class DailyValue : IQueryable
    {

        public static Dictionary<string, string> Map = new Dictionary<string, string> {
                //Macronutrients
                { "Protein (g)" ,"Protein"},
                {"Carbohydrate (g)" , "Carbohydrate, by difference" },
                { "Total fiber (g)" , "Fiber, total dietary"},
                { "Total fat (% of calories)" , "Total lipid (fat)" },
               {"Saturated fat (% of calories)" , "Fatty acids, total saturated" },
                {"Sugars, total","Sugars, total" },
              /*  { "Linoleic acid (g)" , new Tuple<string, int>("Protein", 0) },
                { "alpha-Linolenic acid (g)" , new Tuple<string, int>("Protein", 0) },*/
                { "Cholesterol (mg)" , "Cholesterol"},
                //Minerals
                { "Calcium (mg)" , "Calcium, Ca"},
                { "Iron (mg)" , "Iron, Fe"},
                { "Magnesium (mg)" ,"Magnesium, Mg"},
                { "Phosphorus (mg)", "Phosphorus, P"},
                { "Potassium (mg)" , "Potassium, K"},
                { "Sodium (mg)" , "Sodium, Na"},
                { "Zinc (mg)" , "Zinc, Zn"},
                { "Copper (mcg)" , "Copper, Cu"},
                { "Selenium (mcg)" ,"Selenium, Se"},
                // vitamins
                { "Vitamin A (mcg RAE)" , "Vitamin A, IU"},
                { "Vitamin Dh (mcg)" ,"Vitamin D"},
                { "Vitamin E (mg AT)" , "Vitamin E (alpha-tocopherol)" },
                { "Vitamin C (mg)" , "Vitamin C, total ascorbic acid"},
                { "Thiamin (mg)" , "Thiamin"},
                { "Riboflavin (mg)" , "Riboflavin"},
                { "Niacin (mg)" , "Niacin"},
                { "Folate (mcg)" , "Folate, total"},
                { "Vitamin B6 (mg)" , "Vitamin B-6"},
                { "Vitamin B12 (mcg)" , "Vitamin B-12"},
                { "Choline (mg)" , "Choline, total"},
                { "Vitamin K (mcg)" , "Vitamin K (phylloquinone)"},
        };
        public Dictionary<string, MinMaxDouble> ResetDailyValues;
        public Dictionary<string, MinMaxDouble> DailyValues;

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            if (obj is System.DBNull) return false;
            return ID.Equals(((DailyValue)obj).ID);
        }

        [BsonElement("_id")]
        public int ID { get; set; }

        [BsonElement("Age")]
        public AgeParam Age { get; set; }

        [BsonElement("Gender")]
        public GenderParam Gender { get; set; }

        public override string ToString()
        {
            return Gender.ToString() + " " + Age.ToString();
        }


        public DailyValue Clone()
        {
            var ret = new DailyValue();
            ret.DailyValues = new Dictionary<string, MinMaxDouble>(this.DailyValues);
            ret.ResetDailyValues = new Dictionary<string, MinMaxDouble>(this.ResetDailyValues);
            return ret;
        }

        public static DailyValue NullDefault(Tuple<GenderParam, AgeParam> ageGender, int id)
        {
            var nullDefault = NullDefault();
            nullDefault.Gender = ageGender.Item1;
            nullDefault.Age = ageGender.Item2;
            nullDefault.ID = id;
            return nullDefault;
        }

        public static DailyValue NullDefault()
        {
            var ret = new DailyValue();
            ret.DailyValues = new Dictionary<string, MinMaxDouble>();
            Map.Values.ToList().ForEach(key => ret.DailyValues.Add(key, new MinMaxDouble() ));
            ret.ResetDailyValues = ret.DailyValues;
            ret.Age = null;
            ret.Gender = null;
            return ret;
        }

      
        public Dictionary<string, MinMaxDouble> DuplicateDictionary()
        {
            return DailyValues.ToDictionary(k => k.Key, k => new MinMaxDouble() { MinValue = k.Value.MinValue, MaxValue = k.Value.MaxValue });
        }

        public void Save()
        {
            ResetDailyValues = new Dictionary<string, MinMaxDouble>(DailyValues);
        }
    }
}
