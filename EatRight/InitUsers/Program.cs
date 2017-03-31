using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace InitUsers {
    class Program {
        static void Main(string[] args) {
            var unit = new RestDBInterface();
            unit.Users.Empty();

            var user = new UserProfile() {
                ID = 1,
                Name = "Alex Bihshtein",
                Email = "alexbihsh@gmail.com",
                Age = 28,
                Gender = GenderType.Male,
                ActivityLevel = PhysicalActivityLevel.VeriActive,
                Height = 170,
                Weight = 64,
                Priorities = new List<UserPriorities>() {UserPriorities.Simplicity, UserPriorities.Nutrition }
            };
            unit.Users.Add(user);

         /*   user = new UserProfile() {
                ID = 2,
                Name = "Liran Madgar",
                Email = "liran.madjar@gmail.com",
                Age = 29,
                Gender = GenderType.Male,
                ActivityLevel = PhysicalActivityLevel.LightlyActive,
                Height = 175,
                Weight = 70,
                Priorities = new List<UserPriorities>() { UserPriorities.Simplicity, UserPriorities.Nutrition, }
            };
            unit.Users.Add(user);


            user = new UserProfile() {
                ID = 3,
                Name = "Siukei Cheung",
                Email = "siukeicheung184@gmail.com",
                Age = 28,
                Gender = GenderType.Male,
                ActivityLevel = PhysicalActivityLevel.ModeratelyActive,
                Height = 167,
                Weight = 57,
                Priorities = new List<UserPriorities>() { UserPriorities.Simplicity, UserPriorities.Nutrition }
            };
            unit.Users.Add(user);


            user = new UserProfile() {
                ID = 4,
                Name = "Uri Berger",
                Email = "uriberger@mail.tau.ac.il",
                Age = 28,
                Gender = GenderType.Male,
                ActivityLevel = PhysicalActivityLevel.VeriActive,
                Height = 173,
                Weight = 67,
                Priorities = new List<UserPriorities>() { UserPriorities.Nutrition, UserPriorities.Simplicity }
            };
            unit.Users.Add(user);    */  
        }
    }
}
