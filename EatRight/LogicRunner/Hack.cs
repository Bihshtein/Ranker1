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


        public static void RecommendToUsers(MealType mealType) {
            var unit = new RestDBInterface();
            unit.Users.GetAllList().ForEach(u => {
                var rec = RecommendationDBGenerator.FromUserProfile(u, unit);
                rec.range = new MealSuggestionRange() { Length = 7, MealType = mealType };                
                var generator = new RecommendationGenerator(new RestDBInterface(), rec, true, false, u.RecommendedRecipes);
                var meals = generator.GetRecommendation().MealsSet.ToList();
                PersonalFeed.SendEmail(rec, meals, mealType.ToString());
                meals.ForEach(m => u.RecommendedRecipes.Add(m.Recipe.ID));
                unit.Users.Update(s => s.ID, u.ID, u);
            });
        }
    }
}
