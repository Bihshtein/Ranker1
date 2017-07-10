using RecommendationBuilder;
using RestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicRunner
{
    class MyViewModel
    {
        public int Serv {
            get {
                return _obj.Recipe.Servings;
            }
        }
        public int ID {
            get {
                return _obj.Recipe.ID;
            }
        }
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

        public static string GetNutInfo(Recipe recipe) {
            var unit = new RestDBInterface();
            var dv = unit.DailyValues.GetAllList();
            string res = string.Empty;
            var scores = new Dictionary<string, double>();
            recipe.TotalNutValues.ToList().ForEach(i => {
                if (dv[8].DailyValues.ContainsKey(i.Key))
                    scores.Add(i.Key, i.Value/ recipe.Servings / dv[8].DailyValues[i.Key].MinValue/0.333);
            });
            var ordered = scores.OrderByDescending(i => i.Value).ToList();
            ordered.ForEach(i => {
                res += i.Key + " : " + string.Format("{0:0.##}", i.Value) + "\n";
            });
            return res;
        }

        public static string GetNutInfo2(Recipe recipe, Recipe recipe2) {
            var unit = new RestDBInterface();
            var dv = unit.DailyValues.GetAllList();
            string res = string.Empty;
            var scores = new Dictionary<string, double>();
            recipe.TotalNutValues.ToList().ForEach(i => {
                if (dv[8].DailyValues.ContainsKey(i.Key))
                    scores.Add(i.Key, (i.Value / recipe2.TotalNutValues[i.Key]));
            });
            var ordered = scores.OrderByDescending(i => i.Value).ToList();
            ordered.ForEach(i => {
                res += i.Key + " : " + string.Format("{0:0.##}", i.Value) + "\n";
            });
            return res;
        }
        public string URL { get; set; }
        public RecipesSource Source { get; set; }
        public string NutValues { get {
                return GetNutInfo(_obj.Recipe);
            }
            
        }
        public string GradersResult { get; set; }
        public string Products {
            get {
                string res = string.Empty;
                if (_obj.Recipe.ProductsWeight != null)
                    _obj.Recipe.ProductsWeight.Keys.ToList().ForEach(value => res += value + " : " + _obj.Recipe.ProductsWeight[value] + " gm\n");
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
