using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;

namespace TestTheMongo {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void AddSomeProducts() {
            var a = new RestUnitOfWork();
            a.Products.Add(new Product(1, "Apple", 2.5));
            a.Products.Add(new Product(3, "Onion", 3.5));
            Assert.AreEqual(a.Products.Get(3).Name , "Onion");

            Assert.AreEqual(a.Products.Get(1).Name , "Apple");
        }
    }
}
