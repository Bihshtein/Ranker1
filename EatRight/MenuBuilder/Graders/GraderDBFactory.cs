using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder.Graders
{
    /// <summary>
    /// Calculates daily values according to user profile
    /// </summary>
    public class DailyValuesGenerator
    {
        public static DailyValue FromUserProfile(UserProfile userProfile, RestDBInterface unit)
        {
            if (userProfile == null) return null;
            var dValues = unit.DailyValues.GetByAgeAndGender(userProfile.Age, userProfile.Gender);
            if ((dValues == null) || (dValues.Count <= 0)) return null;
            // TODO: logic modifications according to user profile

            return dValues[0];
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
    

    public class DailyValuesRecord
    {
        public GenderType SexType { get; set; }
        public AgeParam AgeRange { get; set; }
        public DailyValue DailyValues { get; set; }

        public DailyValuesRecord(GenderType sexType, AgeParam ageRange, DailyValue dailyValues)
        {
            SexType = sexType;
            AgeRange = ageRange.Clone();
            DailyValues = dailyValues.Clone();
        }
    }

    public class DailyValuesDB
    {
        List<DailyValuesRecord> dailyValuesRecords { get; set; }

        public DailyValuesDB()
        {

        }

        public bool LoadFromCsv(string path)
        {
            return false;
        }
    }

    public class SuggestionRangeGenerator
    {
        public static SuggestionRange FromUserProfile(UserProfile userProfile)
        {
            if (userProfile == null) return null;
            
            // TODO: get suggetion range from user profile

            return SuggestionRange.SingleDay();
        }
    }

    /// <summary>
    /// A grader db configurator / generator according to UserProfiles
    /// </summary>
    public class GraderDBGenerator
    {
        public static GraderDB FromUserProfile(UserProfile userProfile, RestDBInterface unit)
        {
            // Looking up db for relevant daily values
            var dValues = DailyValuesGenerator.FromUserProfile(userProfile, unit);
            if (dValues == null) return null;

            var graderDB = new GraderDB();

            graderDB.dailyValues =
                dValues.DuplicateDictionary();
            graderDB.dailyCaloriesNum = 
                DailyCaloriesGenerator.FromUserProfile(userProfile);
            graderDB.range =
               SuggestionRangeGenerator.FromUserProfile(userProfile);
            
            return graderDB;
        }
    }
}
