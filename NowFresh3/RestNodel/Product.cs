using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson.Serialization.Attributes;
namespace RestModel {  
    public enum StateTypes { Fresh,Dried, Pickled, Fried }


    public class Product
    {
        [BsonElement("_id")]
        public string Name { get; set; }
        [BsonElement("_protein")]
        public double Protein { get; set; }
        public byte[] Image { get; set; }
        
    }

}  