using Logic;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestModel
{
    public enum UserRestrictions
    {
        None = 0,
            
        Meat,
        Dairy,
        Fish,
        Seafood,
        NoMeat,
        NoDairy,
        NoFish,
        NoSeafood,
        Vegetarian,
        Pescetarian,
        Vegan,
        Kosher
    }


    public enum UserPriorities {
        Nutrition,
        Simplicity,
        Budget,
    }

    /// <summary>
    /// Different types of graders for Menu, DailyMenu and Meal objects.
    /// </summary>
    public enum GraderType
    {
        GraderMenuStart = 0,

        CaloriesCountGrader,
        CostGrader,
        FoodCategoryGrader,
        MaxNutValuesGrader,
        MinNutValuesGrader,
        ProductsTasteGrader,
        VarietyGrader,
        PrepTimeGrader,
        ServingsNumGrader,

        GraderMenuEnd,

        DailyMenuGraderStart,

        CaloriesCountDailyGrader,
        FoodCategoryDailyGrader,
        MaxNutValuesDailyGrader,
        MinNutValuesDailyGrader,
        ProductsTasteDailyGrader,
        VarietyDailyGrader,
        PrepTimeDailyGrader,
        ServingsNumDailyGrader,

        DailyMenuGraderEnd,

        MealGraderStart,

        CaloriesCountMealGrader,
        MinNutValuesMealGrader,
        MaxNutValuesMealGrader,
        FoodCategoryMealGrader,
        ProductsTasteMealGrader,
        PrepTimeMealGrader,
        ServingsNumMealGrader,
        StepsNumMealGrader,

        MealGraderEnd,

        RecommendationGraderStart,

        VarietyRecoGrader,
        GradeAverageRecoGrader,

        RecommendationGraderEnd
    }

    /// <summary>
    /// Different types of filters for Menu, DailyMenu and Meal objects.
    /// </summary>
    public enum FilterType
    {
        FilterMenuStart = 0,

        FilterMenuEnd,

        DailyMenuFilterStart,

        DailyMenuFilterEnd,

        MealFilterStart,

        ForbiddenProductsMealFilter,
        MealTypeMealFilter,
        HasMeatMealFilter,
        HasNoMeatMealFilter,
        HasDairyMealFilter,
        HasNoDairyMealFilter,
        HasFishMealFilter,
        HasNoFishMealFilter,
        HasSeafoodMealFilter,
        HasNoSeafoodMealFilter,
        VeganMealFilter,
        OnlyKosherProductsMealFilter,
        NoMeatDairyMealFilter,

        MealFilterEnd
    }

    public class GraderTypeHelper
    {
        public static List<GraderType> GetPlaceHolders()
        {
            return new List<GraderType>()
            {
                GraderType.GraderMenuStart,
                GraderType.GraderMenuEnd,
                GraderType.DailyMenuGraderStart,
                GraderType.DailyMenuGraderEnd,
                GraderType.MealGraderStart,
                GraderType.MealGraderEnd
            };
        }
    }
    public class UserProfile : BodyProfile , IQueryable{
        public UserProfile() {
            RecommendedRecipes = new List<int>();
            Priorities = new HashSet<UserPriorities>();
            Meals = new HashSet<MealType>();
            Restrictions = new HashSet<UserRestrictions>();
        }
        [BsonElement("_id")]
        public int ID { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        public HashSet<UserRestrictions> Restrictions { get; set; }
        public HashSet<UserPriorities> Priorities{ get; set; }
        public string Name { get; set; }
        public int Frequency{ get; set; }
        public HashSet<MealType> Meals { get; set; }
        public List<int> RecommendedRecipes{ get; set; }
    }

  
}
