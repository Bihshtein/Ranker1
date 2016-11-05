using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder {
    class Meal {
        private Dictionary<string, double> productsWeight;
        private RestDBInterface Unit;
        public Meal(Dictionary<string, double> productsWeight, RestDBInterface unit) {
            this.productsWeight = productsWeight;
            Products = productsWeight.Keys.ToList();
            this.Unit = unit;
        }
        public List<string> Products { get; private set; }
        public int GetProductsNumber() { return productsWeight.Count; }
        public KeyValuePair<Product, double> GetProductWeight(string prodName) {
            var product = Unit.Products.GetByName(prodName)[0];
            return new KeyValuePair<Product, double>(product, productsWeight[prodName]);
        }
    }
}
