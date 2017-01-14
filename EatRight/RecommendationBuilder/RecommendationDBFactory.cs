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

            // For adults, we apply the Mifflin - St. Jeor equation
            if (userProfile.Age >= 18)
            {
                var bmr = -1;
                // Equation for men
                if (userProfile.Gender == GenderType.Male)
                {
                    bmr = (int)(10 * userProfile.Weight + 6.25 * userProfile.Height - 5 * userProfile.Age + 5);
                }
                // Equation for women
                if (userProfile.Gender == GenderType.Female)
                {
                    bmr = (int)(10 * userProfile.Weight + 6.25 * userProfile.Height - 5 * userProfile.Age - 161);
                }
                if (bmr > 0)
                {
                    switch (userProfile.activityLevel)
                    {
                        case PhysicalActivityLevel.Sedentary:
                            return (int)(bmr * 1.2);
                        case PhysicalActivityLevel.LightlyActive:
                            return (int)(bmr * 1.375);
                        case PhysicalActivityLevel.ModeratelyActive:
                            return (int)(bmr * 1.55);
                        case PhysicalActivityLevel.VeriActive:
                            return (int)(bmr * 1.725);
                        case PhysicalActivityLevel.ExtraActive:
                            return (int)(bmr * 1.9);
                    }
                }
            }

            // For children, we follow known-heuristics
            else
            {
                if (userProfile.Age < 4)
                {
                    return 900 + 100 * (userProfile.Age - 1);
                }
                else if (userProfile.Age < 9)
                {
                    return 1200 + 50 * (userProfile.Age - 4);
                }
                else if (userProfile.Age < 14)
                {
                    if (userProfile.Gender == GenderType.Male)
                    {
                        return 1600 + 100 * (userProfile.Age - 9);
                    }
                    else if (userProfile.Gender == GenderType.Female)
                    {
                        return 1400 + 50 * (userProfile.Age - 9);
                    }
                }
                else // 14-17
                {
                    if (userProfile.Gender == GenderType.Male)
                    {
                        return 2000 + 100 * (userProfile.Age - 13);
                    }
                    else if (userProfile.Gender == GenderType.Female)
                    {
                        return 1800;
                    }
                }
            }

            // Default value
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

    public class UserRestrictionsGenerator
    {
        public static HashSet<UserRestrictions> FromUserProfile(UserProfile userProfile)
        {
            if (userProfile == null) return null;

            var ret = 
                (userProfile.Restrictions == null) ? new HashSet<UserRestrictions>() : 
                    new HashSet<UserRestrictions>(userProfile.Restrictions);
            
            // uri: for now i'm just adding this to support your UserRestrictions but there's gotta
            // be a different logic from the UI, there are duplicates in nutriention diet through out the filters.
            return ret;
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
            recommendationDB.preferences = UserRestrictionsGenerator.FromUserProfile(userProfile);
            
            return recommendationDB;
        }


    }
}
