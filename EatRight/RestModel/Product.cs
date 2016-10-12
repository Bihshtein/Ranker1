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
        [BsonElement("_id")]
        public int ID { get; set; }

        [BsonElement("_name")]
        public string Name { get; set; }

        [BsonElement("_hasSkin")]
        public bool HasSkin { get; set; }

        [BsonElement("_size")]
        public double UnitSize { get; set; }

        [BsonElement("_protein")]
        public double Protein { get; set; }

        [BsonElement("_fat")]
        public double Fat { get; set; }

        [BsonElement("_carbs")]
        public double Carbs { get; set; }

        [BsonElement("_vitaminC")]
        public double VitaminC { get; set; }

        [BsonElement("_fiber")]
        public double Fiber { get; set; }

        [BsonElement("_animal")]
        public string Animal { get; set; }

        [BsonElement("_servingState")]
        public string ServingState{ get; set; }

        public byte[] Image { get; set; }

    }
}  

