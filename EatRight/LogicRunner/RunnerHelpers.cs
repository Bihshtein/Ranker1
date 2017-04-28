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
        public int OriginalID { get; set; }
        public string NutValues { get; set; }
        public string GradersResult { get; set; }
        public string Products {
            get {
                string res = string.Empty;
                if (_obj.Recipe.ProductsWeight != null)
                    _obj.Recipe.ProductsWeight.Keys.ToList().ForEach(value => res += value +" : " + _obj.Recipe.ProductsWeight[value] + " gm\n");
                return res;
            }
            }

        public string MinScores {
            get {
                string res = string.Empty;
                _obj.GradeInfo.MinNutrientGrades.ToList().ForEach(value => res += value.Key + " : " + value.Value.ToString("N2") + " \n");
                return res;
            }
        }

        public string MaxScores {
            get {
                string res = string.Empty;
                _obj.GradeInfo.MaxNutrientGrades.ToList().ForEach(value => res += value.Key + " : " + value.Value.ToString("N2") + " \n");
                return res;
            }
        }




        private Meal _obj;

        public MyViewModel(Meal obj)
        {
            _obj = obj;
        }

        public Meal GetModel()
        {
            return _obj;
        }
    }

}
