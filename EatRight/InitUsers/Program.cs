using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;
using System.Reflection;
using System.IO;

namespace InitUsers {
    class Program {
        static void Main(string[] args) {
            var unit = new RestDBInterface();
            unit.Users.Empty();
            var path = Assembly.GetExecutingAssembly().Location + @"..\..\..\..\..\LocalDB\EatRightRecepients.csv";
            var rows = File.ReadAllLines(path).ToList();
            var id = 0;
            rows.ForEach(r => {
                var user = GetUser(r.Split(','));
                user.ID = id++;
                unit.Users.Add(user);
            });
        }

        public static UserProfile GetUser(string[] props) {
            var profile = new UserProfile {
                Name = props[0],
                Email = props[1],
                Age = int.Parse(props[2]),
                Gender = (GenderType)Enum.Parse(typeof(GenderType), props[3]),
                ActivityLevel = (PhysicalActivityLevel)Enum.Parse(typeof(PhysicalActivityLevel), props[4]),
                Height = int.Parse(props[5]),
                Weight = 70,
                Priority = (UserPriorities)Enum.Parse(typeof(UserPriorities), props[7]),
                Frequency = int.Parse(props[8]),
                MealsNum = int.Parse(props[9])
            };
            if (props[6] != string.Empty)
                profile.Weight = int.Parse(props[6]);
            if (props[10] != string.Empty) 
                profile.Restrictions = GetChoices<UserRestrictions>(props[10]);
            if (props[11] != string.Empty)
                profile.Meals = GetChoices<MealType>(props[11]);
            return profile;
        }

        public static HashSet<T> GetChoices<T>(string choices) {
            var res = new HashSet<T>();
            var parts = choices.Split('|').ToList();
            parts.ForEach(p => res.Add((T)Enum.Parse(typeof(T), p)));
            return res;
        }
    }
}
