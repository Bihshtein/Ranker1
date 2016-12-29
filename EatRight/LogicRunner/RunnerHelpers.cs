using RecommendationBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicRunner
{
    class MyViewModel
    {
        public string Calories {
           
            get {
                return _obj.CaloriesNum.ToString("N2");
            }
            
        }
        public string Name { get; set; }
        public string Grade {
          
            get {
                return _obj.Grade.ToString("N2");
            }
            
        }
        public int Id { get; set; }
        public string NutValues { get; set; }
        public string GradersResult { get; set; }
        public string Products {
            get {
                string res = string.Empty;
                _obj.Meal.ProductsWeight.Keys.ToList().ForEach(value => res += value +" : " + _obj.Meal.ProductsWeight[value] + " gm\n");
                return res;
            }
            }




        private MealWrapper _obj;

        public MyViewModel(MealWrapper obj)
        {
            _obj = obj;
        }

        public MealWrapper GetModel()
        {
            return _obj;
        }
    }

}
