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
        NoSeafood
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

    /// <summary>
    /// Different types of nutrition diets
    /// TODO: Need to come up with exception system - for example, what about a 'vegan' that eats fish   
    /// </summary>
    public enum NutritionDietType
    {
        None = 0,
        Pregnant, // (no raw meat, no sushi, etc)
        Vegetarian, // (no animal products, including no fish or seafood)
        Pescetarian, // (like Vegetarian, but eats fish and seafood)
        Vegan, // (no animal, fish, dairy, or other foods derived from or made by animals incl. insects)
        RawFood, // (definitions may vary but often uncooked vegan)
        Diabetic, // (diet subject to management of the medical condition diabetes - which can have different levels of severity)
        GlutenFree, // (diet restricted by the constraints of coeliac disease, an autoimmune disorder of the small intestine - need to avoid foods that contain gluten)
        Kosher, // (diet meets Jewish religious requirements)
        Halal // (food meets Muslim religious requirements)
    }

    /// <summary>
    /// Different types of menu restrictions
    /// TODO: come up with a system to generate custom menu restrictions by user
    ///       meaning, to be able to divide a menu restriction to specific rules
    ///       - Kosher
    ///             - Does not eat mixture of diary/milk in a 6 hour window
    /// </summary>
    public enum MenuRestrictionTypes
    {
        Kosher, // (does not eat mixture of diary/milk in a 6 hour window)
        Halal // (menu that meets Muslim religious requirements) 
    }

    /// <summary>
    /// Represents a single user's profile
    /// </summary>
    public class UserDietProfile
    {
        public double Weight { get; protected set; }
        public double TargetWeight { get; protected set; }

        // Types of diet restrictions (vegan, gluten-free, etc.)
        public List<NutritionDietType> DietRestrictions { get; protected set; }

        // Types of menu restrictions (kosher: no dairy/meat mixture, 6-hour restrictions)
        public List<MenuRestrictionTypes> MenuRestrictions { get; protected set; }
        

        public bool HasMenuRestrictions()
        {
            return (((MenuRestrictions != null) && (MenuRestrictions.Any())));
        }

        public bool HasDietRestrictions()
        {
            return (((DietRestrictions != null) && (DietRestrictions.Any())));
        }
    }

    /// <summary>
    ///  TODO: fill this once figured out
    /// </summary>
    public class UserSocialProfile
    {
        // Email address
        // Image
        // Nickname
        // User Id
        // Social User Id
        // Social credentials
        // Social link
    }

    public class UserProfile
    {
        public string Name { get; set; }
        public int Age { get; set; }

        // todo: those two going to be removed
        public GenderType Gender { get; set; }
        public int Pregnant { get; set; }

        // TODO: temp support for uri's restriction code
        public HashSet<UserRestrictions> Restrictions { get; set;}

        public UserDietProfile DietProfile { get; protected set; }
        public UserSocialProfile SocialProfile { get; protected set; }

        // TODO: Add user credentials / Hash keys
        public int Id { get; set; }
    }
        
}
