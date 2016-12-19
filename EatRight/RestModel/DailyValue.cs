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

    public enum GenderType
    {
        Undefined = 0,
        Female,
        Male,
        Any
    }

    public class GenderParam : DailyValueParam<GenderType>
    {
        public GenderType Type { get; set; }

        public GenderParam(GenderType param)
        {
            Type = param;
        }

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

            return new GenderParam(StringToGenderType[gender]);
        }
    }

    public class DailyValue : IQueryable
    {
        protected Dictionary<string, object> resetDailyValues;
        [BsonExtraElements]
        protected Dictionary<string, object> dailyValues;

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

        public DailyValue()
        {
            dailyValues = new Dictionary<string, object>();
            resetDailyValues = new Dictionary<string, object>(dailyValues);
            
        }

        public DailyValue(Dictionary<string, object> dValues)
        {
            dailyValues = new Dictionary<string, object>(dValues);
            resetDailyValues = new Dictionary<string, object>(dailyValues);
        }

        protected DailyValue(DailyValue dup)
        {
            if (dup == null) throw new ArgumentNullException("dup");
            resetDailyValues = new Dictionary<string, object>(dup.resetDailyValues);
            dailyValues = new Dictionary<string, object>(dup.dailyValues);
        }

        public DailyValue Clone()
        {
            return new DailyValue(this);
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
            var ret = new DailyValue(
                new Dictionary<string, object>()
                {
            { "Protein", (double)-1 }, //
            { "Fiber",(double)-1}, //
            { "VitaminC",(double)-1}, //
            { "Fat",(double)-1 }, //
            { "Thiamin",(double)-1 }, //
            { "Riboflavin",(double)-1 }, //
            { "Niacin",(double)-1 }, //
            { "PantothenicAcid",(double)-1 }, // ?
            { "VitaminB6",(double)-1 }, //
            { "VitaminB12",(double)-1 }, //
            { "VitaminD",(double)-1 }, //
            { "VitaminA",(double)-1 }, //
            { "Folate",(double)-1 }, //
            { "VitaminE",(double)-1 }, //
            { "VitaminK",(double)-1 }, //
            { "Calcium",(double)-1}, //
            { "Iron",(double)-1 }, //
            { "Magnesium",(double)-1 }, //
            { "Phosphorus",(double)-1 }, //
            { "Potassium",(double)-1 }, //
            { "Sodium",(double)-1 }, //
            { "Zinc",(double)-1 } //
                });

            ret.Age = null;
            ret.Gender = null;
            return ret;
        }

        public bool IsValid()
        {
            var valid = (Age != null) && (Gender != null);
            if (!valid) return false;
            foreach (var nutrient in dailyValues)
            {
                if (!(nutrient.Value is double) || ((double)nutrient.Value == -1)) return false;
            }
            return true;
        }

        public static DailyValue Default()
        {
            var ret= new DailyValue(
                new Dictionary<string, object>()
                {
            { "Protein", (double)56 }, //
            { "Fiber",(double)25}, //
            { "VitaminC",(double)90}, //
            { "Fat",(double)65 }, //
            { "Thiamin",(double)1.2 }, //
            { "Riboflavin",(double)1.3 }, //
            { "Niacin",(double)18 }, //
            { "PantothenicAcid",(double)5 }, // ?
            { "VitaminB6",(double)1.3 }, //
            { "VitaminB12",(double)2.4 }, //
            { "VitaminD",(double)600 }, //
            { "VitaminA",(double)5000 }, //
            { "Folate",(double)400 }, //
            { "VitaminE",(double)15 }, //
            { "VitaminK",(double)80 }, //
            { "Calcium",(double)1000}, //
            { "Iron",(double)11 }, //
            { "Magnesium",(double)400 }, //
            { "Phosphorus",(double)1000 }, //
            { "Potassium",(double)3500 }, //
            { "Sodium",(double)2400 }, //
            { "Zinc",(double)15 } //
                });

            ret.Age = new AgeParam(25, 30);
            ret.Gender = new GenderParam(GenderType.Any);
            return ret;
        }

        public Dictionary<string, double> DuplicateDictionary()
        {
            return dailyValues.ToDictionary(k => k.Key, k => (double)k.Value);
        }

        public static DailyValue DefaultByDryParams(double age, int sex)
        {
            return Default();
        }

        public bool Increase(string dValue, double precentage)
        {
            if (!(dailyValues.ContainsKey(dValue))) return false;
            dailyValues[dValue] = (double)dailyValues[dValue] + ((double)dailyValues[dValue] * precentage / 100);
            return true;
        }

        public bool Decrease(string dValue, double precentage)
        {
            if (!(dailyValues.ContainsKey(dValue))) return false;
            dailyValues[dValue] = (double)dailyValues[dValue] - ((double)dailyValues[dValue] * precentage / 100);
            return true;
        }

        public bool Set(string dValue, double value)
        {
            if (!(dailyValues.ContainsKey(dValue))) return false;
            dailyValues[dValue] = value;
            return true;
        }

        public bool Reset(string dValue, double value)
        {
            if (!(resetDailyValues.ContainsKey(dValue) &&
                (dailyValues.ContainsKey(dValue)))) return false;
            dailyValues[dValue] = resetDailyValues[dValue];
            return true;
        }

        public void Save()
        {
            resetDailyValues = new Dictionary<string, object>(dailyValues);
        }
    }
}
