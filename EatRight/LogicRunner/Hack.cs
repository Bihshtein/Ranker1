using Logic;
using RecommendationBuilder;
using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicRunner {
    public class Hack {


        public static List<Meal> GetRecommendation(int age, GenderType gender) {
            var rec = RecommendationDBGenerator.FromBodyProfile(new BodyProfile() { Age = age, Gender = gender }, new RestDBInterface());
            rec.range = new MealSuggestionRange() { Length = 5, MealType = MealType.Dinner };

            var generator = new RecommendationGenerator(new RestDBInterface(), rec, true);
            return generator.GetRecommendation().MealsSet.ToList();
        }
        public static Dictionary<int, List<DayOfWeek>> DaysToRecommend = new Dictionary<int, List<DayOfWeek>> {
            {1, new List<DayOfWeek> {DayOfWeek.Monday } },
            {2, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Thursday } },
            {3, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday } },
            {4, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday, DayOfWeek.Saturday } },
            {5, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Thursday }  },
            {6, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Thursday } },
        };

        public static bool ShouldRecommend(UserProfile user, MealType mealType) {
            return (user.Frequency == 7 || DaysToRecommend[user.Frequency].Contains(DateTime.Now.DayOfWeek))&&
                user.Meals.Contains(mealType);
        }

        public static void RecommendToUsers(MealType mealType, bool debug) {
            var unit = new RestDBInterface();
            unit.Users.GetAllList().ForEach(u => {
                if (ShouldRecommend(u, mealType)) {
                    var rec = RecommendationDBGenerator.FromUserProfile(u, unit);
                    rec.range = new MealSuggestionRange() { Length = u.MealsNum, MealType = mealType };
                    var generator = new RecommendationGenerator(new RestDBInterface(), rec, true, false, u.RecommendedRecipes);
                    var meals = generator.GetRecommendation().MealsSet.ToList();
                    PersonalFeed.SendEmail(rec, meals, mealType.ToString(), debug);
                    if (!debug)
                        meals.ForEach(m => u.RecommendedRecipes.Add(m.Recipe.ID));
                    unit.Users.Update(s => s.ID, u.ID, u);
                }
            });
        }
    }
}
