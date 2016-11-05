using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestModel;
using MongoDB.Bson.Serialization.Attributes;

namespace RestModel  {
    public class Meal {
        [BsonElement("_id")]
        public int ID { get; set; }
        public Dictionary<string, double> productsWeight;
        private static RestDBInterface Unit = new RestDBInterface();
        public Meal(int id, Dictionary<string, double> productsWeight) {
            ID = id;
            this.productsWeight = productsWeight;
            Products = productsWeight.Keys.ToList();
        }
        public List<string> Products { get; private set; }
        public KeyValuePair<Product, double> GetProductWeight(string prodName) {
            var product = Unit.Products.GetByName(prodName)[0];
            return new KeyValuePair<Product, double>(product, productsWeight[prodName]);
        }
    }
}
