using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder.Graders
{
    public class DailyValues
    {
        protected Dictionary<string, double> resetDailyValues;
        protected Dictionary<string, double> dailyValues;

        public DailyValues()
        {
            dailyValues = new Dictionary<string, double>();
            resetDailyValues = new Dictionary<string, double>(dailyValues);
        }

        public DailyValues(Dictionary<string, double> dValues)
        {
            dailyValues = new Dictionary<string, double>(dValues);
            resetDailyValues = new Dictionary<string, double>(dailyValues);
        }

        public static DailyValues Default()
        {
            return new DailyValues(
                new Dictionary<string, double>()
                {
            { "Protein", 56 },
            { "Fiber",25},
            { "VitaminC",90},
            { "Fat",65 },
            { "Thiamin",1.2 },
            { "Riboflavin",1.3 },
            { "Niacin",18 },
            { "PantothenicAcid",5 },
            { "VitaminB6",1.3 },
            { "VitaminB12",2.4 },
            { "VitaminD",600 },
            { "VitaminA",5000 },
            { "Folate",400 },
            { "VitaminE",15 },
            { "VitaminK",80 },
            { "Calcium",1000},
            { "Iron",11 },
            { "Magnesium",400 },
            { "Phosphorus",1000 },
            { "Potassium",3500 },
            { "Sodium",2400 },
            { "Zinc",15 }

                });
        }
        
        public Dictionary<string, double> DuplicateDictionary()
        {
            return new Dictionary<string, double>(dailyValues);
        }

        public static DailyValues DefaultByDryParams(double age, int sex)
        {
            return Default();
        }

        public bool Increase(string dValue, double precentage)
        {
            if (!(dailyValues.ContainsKey(dValue))) return false;
            dailyValues[dValue] += (dailyValues[dValue] * precentage / 100);
            return true;
        }
        
        public bool Decrease(string dValue, double precentage)
        {
            if (!(dailyValues.ContainsKey(dValue))) return false;
            dailyValues[dValue] -= (dailyValues[dValue] * precentage / 100);
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
            resetDailyValues = new Dictionary<string, double>(dailyValues);
        }
    }
    /// <summary>
    /// Calculates daily values according to user profile
    /// </summary>
    public class DailyValuesGenerator
    {
        public static DailyValues FromUserProfile(UserProfile userProfile)
        {
            if (userProfile == null) return null;
            var startingDV = 
                DailyValues.DefaultByDryParams(userProfile.Age, userProfile.Sex);

            // TODO: logic modifications according to user profile

            return startingDV;
        }
    }

    public class DailyCaloriesGenerator
    {
        public static readonly int INVALID_DAILY_CALORIES = -1;
        public static int FromUserProfile(UserProfile userProfile)
        {
            if (userProfile == null) return -1;



            return 3000;
        }
    }

    public class SuggestionRangeGenerator
    {
        public static SuggestionRange FromUserProfile(UserProfile userProfile)
        {
            if (userProfile == null) return null;
            return new SuggestionRange() { Type = SuggestionRangeType.Days, Length = 1 };
        }
    }

    /// <summary>
    /// A grader db configurator / generator according to UserProfiles
    /// </summary>
    public class GraderDBGenerator
    {
        public static GraderDB FromUserProfile(UserProfile userProfile)
        {
            var graderDB = new GraderDB();

            graderDB.dailyValues = 
                DailyValuesGenerator.FromUserProfile(userProfile).DuplicateDictionary();
            graderDB.dailyCaloriesNum = 
                DailyCaloriesGenerator.FromUserProfile(userProfile);
            graderDB.range =
               SuggestionRangeGenerator.FromUserProfile(userProfile);
            
            return null;
        }
    }
}
