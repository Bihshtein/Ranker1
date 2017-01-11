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

        public MinMaxDouble(MinMaxDouble other)
        {
            if (other == null) throw new ArgumentNullException("MinMaxDouble::(MinMaxDouble other)");
            if (other.MinValue > other.MaxValue) throw new Exception("MinMaxDouble:: other.MinValue > other.MaxValue");

            this.MinValue = other.MinValue;
            this.MaxValue = other.MaxValue;
        }

        public MinMaxDouble(double min, double max)
        {
            if (min > max) throw new Exception("MinMaxDouble:: min > max");

            this.MinValue = min;
            this.MaxValue = max;
        }

        public MinMaxDouble(double minMaxValue)
        {
            this.MinValue = minMaxValue;
            this.MaxValue = minMaxValue;
        }
    }
    public class DailyValue : IQueryable
    {
        public Dictionary<string, object> ResetDailyValues;
        [BsonExtraElements]
        public Dictionary<string, object> DailyValues;

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
            ret.DailyValues = new Dictionary<string, object>(this.DailyValues);
            ret.ResetDailyValues = new Dictionary<string, object>(this.ResetDailyValues);
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
            ret.DailyValues = 
                new Dictionary<string, object>()
                {
            { "Protein", new MinMaxDouble(-1) }, //
            { "Fiber",new MinMaxDouble(-1)}, //
            { "VitaminC",new MinMaxDouble(-1)}, //
            { "Fat",new MinMaxDouble(-1) }, //
            { "Thiamin",new MinMaxDouble(-1) }, //
            { "Riboflavin",new MinMaxDouble(-1) }, //
            { "Niacin",new MinMaxDouble(-1) }, //
            { "PantothenicAcid",new MinMaxDouble(-1) }, // ?
            { "VitaminB6",new MinMaxDouble(-1) }, //
            { "VitaminB12",new MinMaxDouble(-1) }, //
            { "VitaminD",new MinMaxDouble(-1) }, //
            { "VitaminA",new MinMaxDouble(-1) }, //
            { "Folate",new MinMaxDouble(-1) }, //
            { "VitaminE",new MinMaxDouble(-1) }, //
            { "VitaminK",new MinMaxDouble(-1) }, //
            { "Calcium",new MinMaxDouble(-1)}, //
            { "Iron",new MinMaxDouble(-1) }, //
            { "Magnesium",new MinMaxDouble(-1) }, //
            { "Phosphorus",new MinMaxDouble(-1) }, //
            { "Potassium",new MinMaxDouble(-1) }, //
            { "Sodium",new MinMaxDouble(-1) }, //
            { "Zinc",new MinMaxDouble(-1) } //
                };
            ret.ResetDailyValues = ret.DailyValues;
            ret.Age = null;
            ret.Gender = null;
            return ret;
        }

        public bool IsValid()
        {
            var valid = (Age != null) && (Gender != null);
            if (!valid) return false;
            foreach (var nutrient in DailyValues)
            {
                if (!(nutrient.Value is double) || ((double)nutrient.Value == -1)) return false;
            }
            return true;
        }

        public static DailyValue Default()
        {
            var ret = new DailyValue();
            ret.DailyValues = 
                new Dictionary<string, object>()
                {
            { "Protein", new MinMaxDouble(56) }, //
            { "Fiber",new MinMaxDouble(25)}, //
            { "VitaminC",new MinMaxDouble(90)}, //
            { "Fat",new MinMaxDouble(65) }, //
            { "Thiamin",new MinMaxDouble(1.2) }, //
            { "Riboflavin",new MinMaxDouble(1.3) }, //
            { "Niacin",new MinMaxDouble(18) }, //
            { "PantothenicAcid",new MinMaxDouble(5) }, // ?
            { "VitaminB6",new MinMaxDouble(1.3) }, //
            { "VitaminB12",new MinMaxDouble(2.4) }, //
            { "VitaminD",new MinMaxDouble(600) }, //
            { "VitaminA",new MinMaxDouble(5000) }, //
            { "Folate",new MinMaxDouble(400) }, //
            { "VitaminE",new MinMaxDouble(15) }, //
            { "VitaminK",new MinMaxDouble(80) }, //
            { "Calcium",new MinMaxDouble(1000)}, //
            { "Iron",new MinMaxDouble(11) }, //
            { "Magnesium",new MinMaxDouble(400) }, //
            { "Phosphorus",new MinMaxDouble(1000) }, //
            { "Potassium",new MinMaxDouble(3500) }, //
            { "Sodium",new MinMaxDouble(2400) }, //
            { "Zinc",new MinMaxDouble(15) } //
                };

            ret.Age = new AgeParam(25, 30);
            ret.Gender = new GenderParam();
            ret.Gender.Type = GenderType.Any;
            return ret;
        }

        public Dictionary<string, MinMaxDouble> DuplicateDictionary()
        {
            return DailyValues.ToDictionary(k => k.Key, k => new MinMaxDouble((double)k.Value));
        }

        public static DailyValue DefaultByDryParams(double age, int sex)
        {
            return Default();
        }

        public bool Increase(string dValue, double precentage)
        {
            if (!(DailyValues.ContainsKey(dValue))) return false;
            DailyValues[dValue] = new MinMaxDouble(((MinMaxDouble)DailyValues[dValue]).MinValue + (((MinMaxDouble)DailyValues[dValue]).MinValue * precentage / 100));
            return true;
        }

        public bool Decrease(string dValue, double precentage)
        {
            if (!(DailyValues.ContainsKey(dValue))) return false;
            DailyValues[dValue] = new MinMaxDouble(((MinMaxDouble)DailyValues[dValue]).MinValue - (((MinMaxDouble)DailyValues[dValue]).MinValue * precentage / 100));
            return true;
        }

        public bool Set(string dValue, double value)
        {
            if (!(DailyValues.ContainsKey(dValue))) return false;
            DailyValues[dValue] = value;
            return true;
        }

        public bool Reset(string dValue, double value)
        {
            if (!(ResetDailyValues.ContainsKey(dValue) &&
                (DailyValues.ContainsKey(dValue)))) return false;
            DailyValues[dValue] = ResetDailyValues[dValue];
            return true;
        }

        public void Save()
        {
            ResetDailyValues = new Dictionary<string, object>(DailyValues);
        }
    }
}
