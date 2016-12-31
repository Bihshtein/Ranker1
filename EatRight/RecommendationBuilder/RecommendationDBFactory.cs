using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace RecommendationBuilder
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


    public class GradersMap
    {
        protected Dictionary<GraderType, double> gradersMap;

        public GradersMap()
        {
            gradersMap = new Dictionary<GraderType, double>();
        }

        public Dictionary<GraderType, double> DuplicateDictionary()
        {
            return gradersMap.ToDictionary(k => k.Key, k => (double)k.Value);
        }

        public static GradersMap EquallySpread()
        {
            var gMap = new GradersMap();
            var gTypes = (GraderType[])Enum.GetValues(typeof(GraderType));
            var gPlaceholders = GraderTypeHelper.GetPlaceHolders();

            foreach (var gType in gTypes)
            {
                if (gPlaceholders.Contains(gType)) continue;
                gMap.gradersMap.Add(gType, 
                                    100.0 / (gTypes.Length - gPlaceholders.Count));
            }

            return gMap;
        }

        public static GradersMap NutGradersOnly()
        {
            var gMap = new GradersMap();
            var gradersValues = new Dictionary<GraderType, double> {
                { GraderType.CaloriesCountDailyGrader, 25},
                { GraderType.MinNutValuesDailyGrader, 25},
                { GraderType.CaloriesCountGrader, 25},
                { GraderType.MinNutValuesGrader, 25} };

            gMap.gradersMap = gradersValues;

            return gMap;
        }

    }
    public class GradersMapGenerator
    {
        public static GradersMap FromUserProfile(UserProfile userProfile)
        {
            // TODO: Get graders from user
            return GradersMap.EquallySpread();
        }
    }
    /// <summary>
    /// A grader db configurator / generator according to UserProfiles
    /// </summary>
    public class RecommendationDBGenerator
    {
        public static RecommendationDB FromUserProfile(UserProfile userProfile, RestDBInterface unit)
        {
            // Looking up db for relevant daily values
            var dValues = DailyValuesGenerator.FromUserProfile(userProfile, unit);
            if (dValues == null) return null;

            var gradersMap = GradersMapGenerator.FromUserProfile(userProfile);
            if (gradersMap == null) return null;

            var recommendationDB = new RecommendationDB();

            recommendationDB.GradersWeight = null;
            recommendationDB.dailyValues = dValues.DuplicateDictionary();
            recommendationDB.dailyCaloriesNum = DailyCaloriesGenerator.FromUserProfile(userProfile);
            recommendationDB.range = SuggestionRangeGenerator.FromUserProfile(userProfile);

            if (userProfile.DietProfile != null)
            {
                recommendationDB.dietRestrictions = userProfile.DietProfile.DietRestrictions;
            }
            
            return recommendationDB;
        }


    }
}
