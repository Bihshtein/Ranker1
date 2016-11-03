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

            Menu testMenu1 = GenerateTestMenu1();
            double eval = MenuGenerator.EvaluateMenu(testMenu1, RestRepository<Product>.DailyValues);
            System.Console.WriteLine("Menu eavluated to " + eval);
        }

        private static Menu GenerateTestMenu1()
        {
            List<Product> list = unit.Products.GetByProtein(0);
            foreach (Product prod in list)
            {
                System.Console.WriteLine(prod.Name);
            }

            // Breakfast
            Product bread = unit.Products.GetByName("Bread")[0];
            Product cottage = unit.Products.GetByName("Cottage")[0];
            Product cucumber = unit.Products.GetByName("Cucumber")[0];
            Product tomato = unit.Products.GetByName("Tomato")[0];
            Product apple = unit.Products.GetByName("Apple")[0];
            Product almond = unit.Products.GetByName("Almond")[0];
            List<KeyValuePair<Product, double>> bProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(bread, 25),
                new KeyValuePair<Product, double>(cottage, 15),
                new KeyValuePair<Product, double>(cucumber, 130),
                new KeyValuePair<Product, double>(tomato, 123),
                new KeyValuePair<Product, double>(apple, 100),
                new KeyValuePair<Product, double>(almond, 10)
            };
            Breakfast breakfast = new Breakfast(bProdList);

            // Lunch
            Product salmon = unit.Products.GetByName("Salmon")[0];
            Product rice = unit.Products.GetByName("Rice")[0];
            Product pasta = unit.Products.GetByName("Pasta")[0];
            List<KeyValuePair<Product, double>> lProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(salmon, 200),
                new KeyValuePair<Product, double>(rice, 100),
                new KeyValuePair<Product, double>(pasta, 100)
            };
            Lunch lunch = new Lunch(lProdList);

            // Dinner
            Product cucumber2 = unit.Products.GetByName("Cucumber")[0];
            Product tomato2 = unit.Products.GetByName("Tomato")[0];
            Product corn = unit.Products.GetByName("Corn")[0];
            Product cottage2 = unit.Products.GetByName("Cottage")[0];
            Product egg = unit.Products.GetByName("Egg")[0];
            List<KeyValuePair<Product, double>> dProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(cucumber2, 200),
                new KeyValuePair<Product, double>(tomato2, 123),
                new KeyValuePair<Product, double>(corn, 50),
                new KeyValuePair<Product, double>(cottage2, 15),
                new KeyValuePair<Product, double>(egg, 20)
            };
            Dinner dinner = new Dinner(dProdList);

            Day day1 = new Day(breakfast, lunch, dinner);

            List<Day> dayList = new List<Day>() { day1 };
            return new Menu(dayList);
        }
    }
}
