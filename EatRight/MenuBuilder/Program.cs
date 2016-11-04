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
            var list = unit.Products.GetByProtein(0);
            foreach (Product prod in list)
            {
                System.Console.WriteLine(prod.Name);
            }

            // Breakfast
            var bread = unit.Products.GetByName("Bread")[0];
            var cottage = unit.Products.GetByName("Cottage")[0];
            var cucumber = unit.Products.GetByName("Cucumber")[0];
            var tomato = unit.Products.GetByName("Tomato")[0];
            var apple = unit.Products.GetByName("Apple")[0];
            var almond = unit.Products.GetByName("Almond")[0];
            var bProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(bread, 25),
                new KeyValuePair<Product, double>(cottage, 15),
                new KeyValuePair<Product, double>(cucumber, 130),
                new KeyValuePair<Product, double>(tomato, 123),
                new KeyValuePair<Product, double>(apple, 100),
                new KeyValuePair<Product, double>(almond, 200)
            };
            Meal breakfast = new Meal(bProdList);

            // Lunch
            var salmon = unit.Products.GetByName("Salmon")[0];
            var rice = unit.Products.GetByName("Rice")[0];
            var pasta = unit.Products.GetByName("Pasta")[0];
            var lProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(salmon, 200),
                new KeyValuePair<Product, double>(rice, 100),
                new KeyValuePair<Product, double>(pasta, 100)
            };
            var lunch = new Meal(lProdList);

            // Dinner
            var cucumber2 = unit.Products.GetByName("Cucumber")[0];
            var tomato2 = unit.Products.GetByName("Tomato")[0];
            var corn = unit.Products.GetByName("Corn")[0];
            var cottage2 = unit.Products.GetByName("Cottage")[0];
            var egg = unit.Products.GetByName("Egg")[0];
            var  dProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(cucumber2, 200),
                new KeyValuePair<Product, double>(tomato2, 123),
                new KeyValuePair<Product, double>(corn, 50),
                new KeyValuePair<Product, double>(cottage2, 15),
                new KeyValuePair<Product, double>(egg, 20)
            };
            var dinner = new Meal(dProdList);

            var day1 = new DailyMenu(breakfast, lunch, dinner);

            var dayList = new List<DailyMenu>() { day1 };
            return new Menu(dayList);
        }
    }
}
