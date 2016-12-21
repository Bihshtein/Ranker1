using MenuBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicRunner
{
    class MyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Grade { get; set; }
        public string NutValues { get; set; }
        public string GradersResult { get; set; }
        public string Products {
            get {
                string res = string.Empty;
                _obj.Meal.ProductsWeight.Keys.ToList().ForEach(value => res += value + '\n');
                return res;
            }
            }

        private MenuMeal _obj;

        public MyViewModel(MenuMeal obj)
        {
            _obj = obj;
        }

        public MenuMeal GetModel()
        {
            return _obj;
        }
    }

}
