using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TestTheMongo {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void AddSomeProducts() {
            var unit = new RestUnitOfWork();
            var path =Assembly.GetExecutingAssembly().Location + @"\..\..\..\..\" + "Products_Table.csv";
            var lines = File.ReadAllLines(path).ToList();
            lines.ForEach((line) => unit.Products.Add(GetProduct(line)));
        }

        public static Product GetProduct(string dataLine) {
            var parts = dataLine.Split(',');
            var main = (MainCategoryTypes)Enum.Parse(typeof(MainCategoryTypes), parts[0]);
            var second = (SecondaryCategoryTypes)Enum.Parse(typeof(SecondaryCategoryTypes), parts[1]);
            return new Product() { MainCategory = main, SecondaryCategory = second, SpeciesName = parts[2] };
        }
    }
}
