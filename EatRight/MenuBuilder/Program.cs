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
            var bread = unit.Products.GetByName("Bread")[0];
            var cottage = unit.Products.GetByName("Cottage")[0];
            var carrot = unit.Products.GetByName("Carrot")[0];
            var tomato = unit.Products.GetByName("Tomato")[0];
            var avocado = unit.Products.GetByName("Avocado")[0];
            var bacon = unit.Products.GetByName("bacon")[0];
            var bProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(bread, 25),
                new KeyValuePair<Product, double>(cottage, 150),
                new KeyValuePair<Product, double>(carrot, 200),
                new KeyValuePair<Product, double>(tomato, 123),
                new KeyValuePair<Product, double>(avocado, 50),
                new KeyValuePair<Product, double>(bacon, 200)
            };
            Meal breakfast = new Meal(bProdList);

            // Lunch
            var salmon = unit.Products.GetByName("Salmon")[0];
            var rice = unit.Products.GetByName("Rice")[0];
            var orange = unit.Products.GetByName("Orange")[0];
            var Broccoli = unit.Products.GetByName("Broccoli")[0];
            var lProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(salmon, 300),
                new KeyValuePair<Product, double>(rice, 200),
                new KeyValuePair<Product, double>(orange, 200),
                new KeyValuePair<Product, double>(Broccoli, 100)
            };
            var lunch = new Meal(lProdList);

            // Dinner
            var tenderloin = unit.Products.GetByName("tenderloin")[0];
            var tomato2 = unit.Products.GetByName("Tomato")[0];
            var corn = unit.Products.GetByName("Corn")[0];
            var Garlic = unit.Products.GetByName("Garlic")[0];
            var egg = unit.Products.GetByName("Egg")[0];
            var almond = unit.Products.GetByName("Almond")[0];
            var  dProdList = new List<KeyValuePair<Product, double>>()
            {
                new KeyValuePair<Product, double>(tenderloin, 200),
                new KeyValuePair<Product, double>(tomato2, 123),
                new KeyValuePair<Product, double>(corn, 200),
                new KeyValuePair<Product, double>(Garlic, 50),
                new KeyValuePair<Product, double>(egg, 200),
                new KeyValuePair<Product, double>(almond, 100)
            };
            var dinner = new Meal(dProdList);

            var day1 = new DailyMenu(breakfast, lunch, dinner);

            var dayList = new List<DailyMenu>() { day1 };
            return new Menu(dayList);
        }
    }
}
