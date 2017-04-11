using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MongoDB.Bson.Serialization.Attributes;
using Logic;

namespace RestModel  {

    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner
    }


    public enum FoodType {
       Meat,
       VegAndFruit,
    }

    public enum RecipeCategory
    {
        Indian,
        Chinese
    }
    
    public class Recipe : IQueryable
    {
        public override int GetHashCode() {
            return ID;
        }
        [BsonElement("_id")]
        public int ID { get; set; }
        public int OriginalID { get; set; }
        public string Urn{ get; set; }
        [BsonElement("_types")]
        public HashSet<MealType> Types { get; set;  }
        [BsonElement("_categories")]
        public HashSet<RecipeCategory> Categories { get; set; }
        [BsonElement("_productTypes")]
        public HashSet<ProductType> ProductTypes { get; set; } = new HashSet<ProductType>();
        [BsonElement("_name")]
        public string Name { get; set; }
        [BsonElement("ImageUrl")]
        public string ImageUrl { get; set; }
        public Dictionary<string, double> ProductsWeight;
        public List<Tuple<string, double,string>> Ingredients;
        public TimeSpan PrepTime { get; set; } // In minutes
        public Dictionary<string, double> TotalNutValues { get; set; }
        public double TotalCaloriesNum { get; set; }
        private static RestDBInterface Unit = new RestDBInterface();
        [BsonElement("Servings")]
        public int Servings { get; set; } = 1;
        [BsonElement("StepsNum")]
        public int StepsNum { get; set; } = 0;

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Recipe m = (Recipe)obj;
            return ID.Equals(m.ID);
        }
        
        private KeyValuePair<List<Product>, double> GetProductWeight(string prodName)
        {
            var product = Queries<Product>.GetMatchingProductsForIngredient(prodName);
            return new KeyValuePair<List<Product>, double>(product, ProductsWeight[prodName]);
        }

        public Boolean HasType(MealType type)
        {
            return Types != null && Types.Contains(type);
        }

        public Boolean HasCategory(RecipeCategory category)
        {
            return Categories != null && Categories.Contains(category);
        }

        public void CalculateProperties()
        {
            CalculateProductTypes();
            CalculateNutValuesAndCalories();
        }

        public void CalculateProductTypes()
        {
            foreach (var pw in ProductsWeight)
            {
                var products = Queries<Product>.GetMatchingProductsForIngredient(pw.Key);
                if (products == null || products.Count == 0)
                {
                    continue;
                }
                var product = products[0];
                ProductTypes.UnionWith(product.Types);
            }
        }

        public bool CalculateNutValuesAndCalories()
        {
            if (this.TotalNutValues != null) return false;

            this.TotalNutValues = new Dictionary<string, double>();
            this.TotalCaloriesNum = 0;
            foreach (var pw in ProductsWeight)
            {
                var res = GetProductWeight(pw.Key).Key;
                var product = res[0];
                var weight = pw.Value;
                var prodNutValues = product.NutrientsList();

                var retProducts = new List<string>();
                foreach (var entry in prodNutValues)
                {
                    if (!TotalNutValues.ContainsKey(entry.Key))
                    {
                        TotalNutValues[entry.Key] = 0;
                    }

                    double curValue = TotalNutValues[entry.Key];
                    TotalNutValues[entry.Key] = curValue + (entry.Value * (weight / Formulas.DefaultGrams));
                }

                TotalCaloriesNum += Formulas.GetTotalCalories(weight,
                    product.Nutrients["Proximates"]["Protein"],
                    product.Nutrients["Proximates"]["Total lipid (fat)"],
                    product.Nutrients["Proximates"]["Carbohydrate, by difference"]);
                    
            }
            
            return true;
        }
    }
}
