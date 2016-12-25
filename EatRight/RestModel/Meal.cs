using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MongoDB.Bson.Serialization.Attributes;

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

    public enum MealCategory
    {
        Indian,
        Chinese
    }
    
    public class Meal : IQueryable
    {
        public override int GetHashCode() {
            return ID;
        }
        [BsonElement("_id")]
        public int ID { get; set; }
        [BsonElement("_types")]
        public HashSet<MealType> Types { get; set;  }
        [BsonElement("_categories")]
        public HashSet<MealCategory> Categories { get; set; }
        [BsonElement("_name")]
        public string Name { get; set; }
        [BsonElement("MealType")]
        public string MealType { get; set; }
        public Dictionary<string, double> ProductsWeight;
        public List<string> Ingredients;
        public double PrepTime { get; set; } // In minutes

        private static RestDBInterface Unit = new RestDBInterface();
        [BsonElement("Servings")]
        public int Servings { get; set; } = 1;

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Meal m = (Meal)obj;
            return ID.Equals(m.ID);
        }

        public KeyValuePair<Product, double> GetProductWeight(string prodName)
        {
            var product = Queries<Product>.GetMatchingProductsForIngredient(prodName)[0];
            return new KeyValuePair<Product, double>(product, ProductsWeight[prodName]/Servings);
        }

        public Boolean HasType(MealType type)
        {
            return Types != null && Types.Contains(type);
        }

        public Boolean HasCategory(MealCategory category)
        {
            return Categories != null && Categories.Contains(category);
        }
    }
}
