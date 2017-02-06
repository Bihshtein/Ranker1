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
            rec.range = new MealSuggestionRange() { Length = 30, MealType = MealType.Dinner };

            var generator = new RecommendationGenerator(new RestDBInterface(), rec, true);
            return generator.GetRecommendation().MealsSet.ToList();
        }
    }
}
