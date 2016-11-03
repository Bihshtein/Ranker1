using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;

namespace MenuBuilder
{
    class Breakfast : Meal
    {
        public Breakfast(List<KeyValuePair<Product, double>> productsWeight) : base(productsWeight) {}

        public override Boolean IsBreakfast() { return true; }
    }
}
