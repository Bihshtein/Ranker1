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
            Assert.IsTrue(unit.Products.Queries.GetChickenBreast().Count == 16);
        }
    }
}
