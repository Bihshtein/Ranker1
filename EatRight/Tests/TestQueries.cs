using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;

namespace Tests {
    [TestClass]
    public class TestQueries {
        [TestMethod]
        public void TestAllProductCounts() {
            var unit = new RestDBInterface();
            var count = unit.Products.GetAllList().Count;
            Assert.IsTrue(count == 3737);
        }

        [TestMethod]
        public void ChickenBreast() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("breast", "chicken","VitaminB12");
            Assert.IsTrue(res.Count == 16);
            Assert.IsTrue((res[res.Count-1].VitaminB12 / res[0].VitaminB12) >2 );
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12) < 3);
        }


        [TestMethod]
        public void Tendterloin() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("tenderloin","Beef", "VitaminB12");
            Assert.IsTrue((res[res.Count-1].VitaminB12 / res[0].VitaminB12) >5);
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12)< 6);
            Assert.IsTrue(res.Count == 46);
        }


        [TestMethod]
        public void Ham() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("ham", "Pork", "VitaminB12");
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12) > 3);
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12) <4);
            Assert.IsTrue(res.Count == 87);
        }


        [TestMethod]
        public void Tomato() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("Tomato", "Vegs", "Magnesium");
            Assert.IsTrue((res[res.Count - 1].Magnesium / res[0].Magnesium) >23);
            Assert.IsTrue((res[res.Count - 1].Magnesium / res[0].Magnesium) < 24);
            Assert.IsTrue(res.Count == 16);
        }


        [TestMethod]
        public void Apple() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("Apples", "Fruits", "Carbs");
            Assert.IsTrue((res[res.Count - 1].Carbs / res[0].Carbs) > 7);
            Assert.IsTrue((res[res.Count - 1].Carbs / res[0].Carbs) < 8);
            Assert.IsTrue(res.Count == 18);
        }

        [TestMethod]
        public void Egg() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("Egg", "Dairy", "Protein");
            Assert.IsTrue((res[res.Count - 1].Protein / res[0].Protein) > 8);
            Assert.IsTrue((res[res.Count - 1].Protein / res[0].Protein) < 9);
            Assert.IsTrue(res.Count == 25);
        }


        [TestMethod]
        public void Wine() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("wine", "Beverages", "Sugar", true);
            Assert.IsTrue((res[res.Count - 1].Sugar / res[0].Sugar) > 12);
            Assert.IsTrue(res.Count == 10);
        }


        [TestMethod]
        public void Salmon() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("salmon", "Fish", "VitaminD");
            Assert.IsTrue((res[res.Count - 1].VitaminD / res[0].VitaminD) > 2);
            Assert.IsTrue((res[res.Count - 1].VitaminD / res[0].VitaminD) < 3);
            Assert.IsTrue(res.Count == 14);
        }


        [TestMethod]
        public void Bread() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("Bread", "Baked", "Fiber");
            Assert.IsTrue((res[res.Count - 1].Fiber / res[0].Fiber) > 19);
            Assert.IsTrue((res[res.Count - 1].Fiber / res[0].Fiber) < 20);
            Assert.IsTrue(res.Count == 64);
        }

        [TestMethod]
        public void GetProductBySearchQuery()
        {
            var product = ProductQueryBuilder.FromString("cheese,cake");

            Assert.IsNotNull(product);
        }
    }
}
