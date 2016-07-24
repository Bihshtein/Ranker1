using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson.Serialization.Attributes;
namespace RestModel {  
    public class Product
    {
        public Product(string name, double price) {
            this.Name = name;
            this.Price = price;
        }
    [BsonElement("_id")]
    public string Name { get; set; }
    public double Price{get; set;}
}  
}  