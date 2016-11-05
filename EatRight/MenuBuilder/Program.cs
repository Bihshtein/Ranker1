using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestModel;


namespace MenuBuilder
{
    class Program
    {
        static RestDBInterface unit;

        static void Main(string[] args)
        {
            unit = new RestDBInterface();

            var testMenu1 = GenerateTestMenu1();
            var eval = MenuGenerator.EvaluateMenu(testMenu1, RestRepository<Product>.DailyValues, 3000);
            System.Console.WriteLine("Menu eavluated to " + eval);
        }

        private static Menu GenerateTestMenu1()
        {
            var list = unit.Products.GetTopFoods(15,4);
            list.RemoveAll((p) => p.Animal != string.Empty);
            foreach (Product prod in list)
            {
                System.Console.WriteLine(prod.Name);
            }

            // Breakfast
            var bProdList = new Dictionary<string, double>()
            {
                { "Bread", 25},
                { "Cottage", 150},
                { "Carrot", 200},
                { "Tomato", 123},
                { "Avocado", 50},
                { "bacon", 200}
            };
            Meal breakfast = new Meal(bProdList, unit);

            // Lunch
            var lProdList = new Dictionary<string, double>()
            {
                {"Salmon", 300 },
                {"Rice", 200},
                {"Orange", 200},
                {"Broccoli", 100}
            };
            var lunch = new Meal(lProdList, unit);

            // Dinner
            var  dProdList = new Dictionary<string, double>()
            {
                {"tenderloin", 200 },
                { "Tomato", 123},
                { "Corn", 200},
                { "Garlic", 50},
                { "Egg", 200},
                { "Almond", 100}
            };
            var dinner = new Meal(dProdList,unit);

            var day1 = new DailyMenu(breakfast, lunch, dinner);

            var dayList = new List<DailyMenu>() { day1 };
            return new Menu(dayList);
        }
    }
}
