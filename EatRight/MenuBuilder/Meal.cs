using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder
{
    class Meal
    {
        private List<KeyValuePair<Product, double>> productsWeight;

        public Meal(List<KeyValuePair<Product, double>> productsWeight)
        {
            this.productsWeight = productsWeight;
        }

        public int GetProductsNumber() { return productsWeight.Count; }
        public KeyValuePair<Product, double> GetProductWeight(int prodInx) { return productsWeight[prodInx]; }
    }
}
