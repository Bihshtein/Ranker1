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
        private HashSet<MealType> types;
        private HashSet<MealCategory> categories;
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
            this.types = types;
        }

        public Meal(string name, Dictionary<string, double> productsWeight, HashSet<MealCategory> categories) :
            this(name, productsWeight)
        {
            this.categories = categories;
        }

        public Meal(string name, Dictionary<string, double> productsWeight, HashSet<MealType> types, HashSet<MealCategory> categories) :
            this(name, productsWeight)
        {
            this.types = types;
            this.categories = categories;
        }

        public KeyValuePair<Product, double> GetProductWeight(string prodName)
        {
            var product = Unit.Products.GetByName(prodName)[0];
            return new KeyValuePair<Product, double>(product, productsWeight[prodName]);
        }

        public Boolean HasType(MealType type)
        {
            return types.Contains(type);
        }

        public Boolean HasCategory(MealCategory category)
        {
            return categories.Contains(category);
        }
    }
}
