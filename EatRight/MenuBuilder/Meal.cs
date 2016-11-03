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

        public virtual Boolean IsBreakfast() { return false; }
        public virtual Boolean IsLunch() { return false; }
        public virtual Boolean IsDinner() { return false; }

        public int GetProductsNumber() { return productsWeight.Count; }
        public KeyValuePair<Product, double> GetProductWeight(int prodInx) { return productsWeight[prodInx]; }
    }
}
