﻿using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using Logic;

namespace RecommendationBuilder
{
    /// <summary>
    /// Calculates daily values according to user profile
    /// </summary>
    public class DailyValuesGenerator
    {
        public static DailyValue FromUserProfile(BodyProfile userProfile, RestDBInterface unit)
        {
            if (userProfile == null) return null;
            var dValues = unit.DailyValues.GetByAgeAndGender(userProfile.Age, userProfile.Gender);
            if ((dValues == null) || (dValues.Count <= 0)) return null;
            // TODO: logic modifications according to user profile

            return dValues[0];
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
        public static Dictionary<UserPriorities,List<GraderType>> PriorityToGradersMap = new Dictionary<UserPriorities, List<GraderType>>() {
            { UserPriorities.Diet, new List<GraderType>() {GraderType.CaloriesCountMealGrader } },
            { UserPriorities.Nutrition, new List<GraderType>() { GraderType.MinNutValuesMealGrader, GraderType.MaxNutValuesMealGrader, } },
            { UserPriorities.Convenience, new List<GraderType>() { GraderType.PrepTimeMealGrader } }
        };

        public static Dictionary<GraderType, double> GraderDefaults = new Dictionary<GraderType, double>() {
            { GraderType.CaloriesCountMealGrader, 1},
            { GraderType.MinNutValuesMealGrader,  1},
            { GraderType.MaxNutValuesMealGrader,  1},
            { GraderType.PrepTimeMealGrader, 1}
        };
        public static RecommendationDB FromUserProfile(UserProfile userProfile, RestDBInterface unit)
        {
            var gradersMap = GradersMapGenerator.FromUserProfile(userProfile);
            if (gradersMap == null) return null;
           
            var recommendationDB = FromBodyProfile(userProfile, unit);
            recommendationDB.GradersWeight = new Dictionary<GraderType, double>();
            var priorities = new Stack<UserPriorities>(userProfile.Priorities);
            while (priorities.Count > 0) {
                var graders = PriorityToGradersMap[priorities.Pop()];
                graders.ForEach(g => recommendationDB.GradersWeight.Add(g, GraderDefaults[g] * priorities.Count));
            }
            recommendationDB.range = SuggestionRangeGenerator.FromUserProfile(userProfile);
            recommendationDB.preferences = UserRestrictionsGenerator.FromUserProfile(userProfile);
            recommendationDB.UserProfile = userProfile;
            return recommendationDB;
        }


        public static RecommendationDB FromBodyProfile(BodyProfile userProfile, RestDBInterface unit) {
            // Looking up db for relevant daily values
            var dValues = DailyValuesGenerator.FromUserProfile(userProfile, unit);
            if (dValues == null) return null;

            var recommendationDB = new RecommendationDB();

            recommendationDB.dailyValues = dValues.DuplicateDictionary();
            recommendationDB.dailyCaloriesNum = Logic.Formulas.GetCalories(userProfile);
            return recommendationDB;
            
        }


    }
}
