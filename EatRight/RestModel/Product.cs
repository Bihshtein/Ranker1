using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson.Serialization.Attributes;
namespace RestModel {  
    public enum ServingStateName { Raw,Dried, Pickled, Fried , Smo}

    public class ServingStateInfo
    {
        [BsonElement("_protein")]
        public double Protein { get; set; }

        [BsonElement("_fat")]
        public double Fat { get; set; }

        [BsonElement("_carbs")]
        public double Carbs { get; set; }



        [BsonElement("_fiber")]
        public double Fiber { get; set; }

        public byte[] Image { get; set; }
    }


    public class Product
    {
        public Product()
        {
            ServingOptions = new Dictionary<ServingStateName, ServingStateInfo>();
        }
        [BsonElement("_id")]
        public string Name { get; set; }

        public Dictionary<ServingStateName, ServingStateInfo> ServingOptions { get; set; }

        [BsonElement("_serving")]
        public double Serving { get; set; }

        [BsonElement("_size")]
        public double UnitSize { get; set; }

        public byte[] Image { get; set; }

    }
    public class CompexProduct
    {
        public CompexProduct()
        {
            Products = new List<Product>();
        }
        List<Product> Products { get; set; }

    }
}  

