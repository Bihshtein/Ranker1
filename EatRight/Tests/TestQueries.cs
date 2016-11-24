﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;

namespace Tests {
    [TestClass]
    public class TestQueries {
        [TestMethod]
        public void TestAllProductCounts() {
            var unit = new RestDBInterface();
            Assert.IsTrue(unit.Products.GetAllList().Count == 2725);
        }

        [TestMethod]
        public void ChickenBreast() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("breast", "Chicken","VitaminB12");
            Assert.IsTrue(res.Count == 14);
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
            var res = unit.Products.Queries.QueryByNameAndValue("Tomatoes", "Vegs", "Magnesium");
            Assert.IsTrue((res[res.Count - 1].Magnesium / res[0].Magnesium) >23);
            Assert.IsTrue((res[res.Count - 1].Magnesium / res[0].Magnesium) < 24);
            Assert.IsTrue(res.Count == 12);
        }


        [TestMethod]
        public void Apple() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("Apples", "Fruits", "Carbs");
            Assert.IsTrue((res[res.Count - 1].Carbs / res[0].Carbs) > 7);
            Assert.IsTrue((res[res.Count - 1].Carbs / res[0].Carbs) < 8);
            Assert.IsTrue(res.Count == 14);
        }

        [TestMethod]
        public void Egg() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue("Egg", "Dairy", "Protein");
            Assert.IsTrue((res[res.Count - 1].Protein / res[0].Protein) > 8);
            Assert.IsTrue((res[res.Count - 1].Protein / res[0].Protein) < 9);
            Assert.IsTrue(res.Count == 25);
        }
    }
}
