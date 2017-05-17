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
                Weight = int.Parse(props[6]),
                Priorities = new HashSet<UserPriorities>() {
                    (UserPriorities)Enum.Parse(typeof(UserPriorities), props[7]),
                    (UserPriorities)Enum.Parse(typeof(UserPriorities), props[8]),
                },
                Frequency = int.Parse(props[9])
            };
            if (props[10] != "None")
                profile.Restrictions.Add((UserRestrictions)Enum.Parse(typeof(UserRestrictions), props[10]));
            if (props[11] != "None")
                profile.Restrictions.Add((UserRestrictions)Enum.Parse(typeof(UserRestrictions), props[11]));
            if (props[12] != "None")
                profile.Meals.Add((MealType)Enum.Parse(typeof(MealType), props[12]));
            if (props[13] != "None")
                profile.Meals.Add((MealType)Enum.Parse(typeof(MealType), props[13]));
            if (props[14] != "None")
                profile.Meals.Add((MealType)Enum.Parse(typeof(MealType), props[14]));
            return profile;

        }
    }
}
