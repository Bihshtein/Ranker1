using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBuilder
{
    class MenuGenerator
    {
        private class MenuComparer : IComparer<KeyValuePair<Menu, double>>
        {
            public int Compare(KeyValuePair<Menu, double> x, KeyValuePair<Menu, double> y)
            {
                return (-1) * x.Value.CompareTo(y.Value); // Sort in descending order
            }
        }

        public static double EvaluateMenu(Menu menu, Dictionary<Grader, double> graderMap)
        {
            double graderWeightSum = 0;
            double grade = 0;

            foreach (var entry in graderMap)
            {
                graderWeightSum += entry.Value;

                var grader = entry.Key;
                grade += (grader.Grade(menu) * entry.Value);
            }

            // Return the scaled grade
            return (grade / graderWeightSum) * 100;
        }

        public static List<KeyValuePair<Menu, double>> GenerateMenuList(GraderDB graderDB)
        {
            Grader.graderDB = graderDB; // For the initialization of graders map

            // Initialize graders map
            var graderMap = new Dictionary<Grader, double>()
            {
                {new NutValuesGrader(), 0.5},
                {new CaloriesCountGrader(), 0.5}
            };

            Grader.graderDB = null;

            var menuList = GetMenuList();
            var menuGradeList = new List<KeyValuePair<Menu, double>>();

            foreach (var menu in menuList)
            {
                menuGradeList.Add(new KeyValuePair<Menu, double>(menu, EvaluateMenu(menu, graderMap)));
            }

            var comparer = new MenuComparer();
            menuGradeList.Sort(comparer);
            return menuGradeList;
        }

        private static List<Menu> GetMenuList()
        {
            return new List<Menu>()
            {
                Program.GenerateTestMenu1()
            };
        }
    }
}
