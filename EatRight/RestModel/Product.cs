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

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("HasSkin")]
        public bool HasSkin { get; set; }

        [BsonElement("_size")]
        public double UnitSize { get; set; }

        [BsonElement("Protein")]
        public double Protein { get; set; }

        [BsonElement("Fat")]
        public double Fat { get; set; }

        [BsonElement("Carbs")]
        public double Carbs { get; set; }

        [BsonElement("VitaminC")]
        public double VitaminC { get; set; }

        [BsonElement("Thiamin")]
        public double Thiamin { get; set; }

        [BsonElement("Riboflavin")]
        public double Riboflavin { get; set; }

        [BsonElement("Niacin")]
        public double Niacin { get; set; }

        [BsonElement("PantothenicAcid")]
        public double PantothenicAcid { get; set; }

        [BsonElement("VitaminB6")]
        public double VitaminB6 { get; set; }

        [BsonElement("Choline")]
        public double Choline { get; set; }

        [BsonElement("Betaine")]
        public double Betaine { get; set; }

        [BsonElement("Fiber")]
        public double Fiber { get; set; }

        [BsonElement("Animal")]
        public string Animal { get; set; }

        [BsonElement("ServingState")]
        public string ServingState{ get; set; }

        public byte[] Image { get; set; }

    }
}  

