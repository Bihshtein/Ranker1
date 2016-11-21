using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;

namespace Tests {
    [TestClass]
    public class TestQueries {
        [TestMethod]
        public void TestAllProductCounts() {
            var unit = new RestDBInterface();
            Assert.IsTrue(unit.Products.GetAllList().Count == 2670);
        }

        [TestMethod]
        public void TestAllChickenBreastsCounts() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryForVitaminB12("breast", "Chicken");
            Assert.IsTrue(res.Count == 14);
            Assert.IsTrue((res[res.Count-1].VitaminB12 / res[0].VitaminB12) >2 );
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12) < 3);
        }


        [TestMethod]
        public void TestAllTenderloinsCounts() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryForVitaminB12("tenderloin","Beef");
            Assert.IsTrue((res[res.Count-1].VitaminB12 / res[0].VitaminB12) >5);
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12)< 6);
            Assert.IsTrue(res.Count == 46);
        }


        [TestMethod]
        public void TestAllHamCounts() {
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryForVitaminB12("ham", "Pork");
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12) > 3);
            Assert.IsTrue((res[res.Count - 1].VitaminB12 / res[0].VitaminB12) <4);
            Assert.IsTrue(res.Count == 87);
        }
    }
}
