using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestModel;
using System.Collections.Generic;

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
            CheckNutrientDiff("breast", "chicken", "Vitamins", "Vitamin B-12",2,3,16);
        }
        [TestMethod]
        public void Tendterloin() {
            CheckNutrientDiff("tenderloin","Beef", "Vitamins", "Vitamin B-12",5,6,46);
        }
        [TestMethod]
        public void Ham(){
            CheckNutrientDiff("ham", "Pork","Vitamins", "Vitamin B-12",3,4,87);
        }
        [TestMethod]
        public void Tomato() {
            CheckNutrientDiff("Tomato", "Vegs","Minerals", "Magnesium, Mg",23,25,16);
        }
        [TestMethod]
        public void Apple() {
            CheckNutrientDiff("Apples", "Fruits","Proximates", "Carbohydrate, by difference", 7,8,18);
        }

        [TestMethod]
        public void Egg() {
            CheckNutrientDiff("Egg", "Dairy", "Proximates", "Protein",8,9,25);
        }

        [TestMethod]
        public void Wine() {
            CheckNutrientDiff("wine", "Beverages", "Proximates", "Sugars, total",12,13,10, true);
        }

        [TestMethod]
        public void Salmon() {
            CheckNutrientDiff("salmon", "Fish", "Vitamins", "Vitamin D", 2, 3, 14);
        }

        [TestMethod]
        public void Bread() {
            CheckNutrientDiff("Bread", "Baked", "Proximates", "Fiber, total dietary", 19, 20, 64);
        }

        private void CheckNutrientDiff(string name, string group,string nutGroup, string nutName, int minRatio, int maxRatio, int total,bool partial=false) { 
            var unit = new RestDBInterface();
            var res = unit.Products.Queries.QueryByNameAndValue(name, group, nutGroup, nutName,partial);
            var max = res[res.Count - 1].Nutrients[nutGroup][nutName];
            var min = res[0].Nutrients[nutGroup][nutName];
            var ratio = max / min;
            Assert.IsTrue(ratio > minRatio);
            Assert.IsTrue(ratio < maxRatio);
            Assert.IsTrue(res.Count == total);
        }
     

        [TestMethod]
        public void GetProductBySearchQuery()
        {
            var testString = new List<string>
            {
                "cheese cake",
                "cookies chocolate chip commercially prepared, regular",
                "cookie chocolate, chip enriched"
            };

            foreach (var searchQuery in testString)
            {
                var product = ProductQueryBuilder.FromString(searchQuery);
                var str = product.Product.USDAString;
                Assert.IsNotNull(product);
            }
            
        }
    }
}
