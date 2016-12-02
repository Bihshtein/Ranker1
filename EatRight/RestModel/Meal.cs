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

    public enum MealCategory
    {
        Indian,
        Chinese
    }

    public class Meal
    {
        [BsonElement("_id")]
        public int ID { get; set; }
        [BsonElement("_types")]
        public HashSet<MealType> Types { get; private set;  }
        [BsonElement("_categories")]
        public HashSet<MealCategory> Categories { get; private set; }
        [BsonElement("_name")]
        public string Name { get; private set; }
        public Dictionary<string, double> productsWeight;
        public List<string> Products { get; private set; }

        private static RestDBInterface Unit = new RestDBInterface();
        private static int idCounter = 0;

        public Meal(string name, Dictionary<string, double> productsWeight)
        {
            this.Name = name;
            this.productsWeight = productsWeight;
            Products = productsWeight.Keys.ToList();
            ID = idCounter++;
        }

        public Meal(string name, Dictionary<string, double> productsWeight, HashSet<MealType> types) :
            this(name, productsWeight)
        {
            this.Types = types;
        }

        public Meal(string name, Dictionary<string, double> productsWeight, HashSet<MealCategory> categories) :
            this(name, productsWeight)
        {
            this.Categories = categories;
        }

        public Meal(string name, Dictionary<string, double> productsWeight, HashSet<MealType> types, HashSet<MealCategory> categories) :
            this(name, productsWeight)
        {
            this.Types = types;
            this.Categories = categories;
        }

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
            var product = Unit.Products.GetByName(prodName)[0];
            return new KeyValuePair<Product, double>(product, productsWeight[prodName]);
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
