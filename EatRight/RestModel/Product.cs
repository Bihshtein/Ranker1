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
        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            return ID.Equals(((Product)obj).ID);
        }
        [BsonElement("_id")]
        public int ID { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("MinorName")]
        public string MinorName { get; set; }

        public string MeatCut { get; set; }
        [BsonElement("HasSkin")]
        public bool HasSkin { get; set; }

        [BsonElement("_size")]
        public double Weight { get; set; }

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

        [BsonElement("VitaminB12")]
        public double VitaminB12 { get; set; }

        [BsonElement("Folate")]
        public double Folate { get; set; }

        [BsonElement("VitaminD")]
        public double VitaminD { get; set; }

        [BsonElement("VitaminA")]
        public double VitaminA { get; set; }

        [BsonElement("VitaminE")]
        public double VitaminE { get; set; }

        [BsonElement("VitaminK")]
        public double VitaminK { get; set; }

        [BsonElement("Calcium")]
        public double Calcium { get; set; }

        [BsonElement("Iron")]
        public double Iron { get; set; }

        [BsonElement("Phosphorus")]
        public double Phosphorus { get; set; }

        [BsonElement("Zinc")]
        public double Zinc { get; set; }

        [BsonElement("Potassium")]
        public double Potassium { get; set; }

        [BsonElement("Sodium")]
        public double Sodium { get; set; }

        [BsonElement("Magnesium")]
        public double Magnesium { get; set; }

        [BsonElement("Fiber")]
        public double Fiber { get; set; }

        [BsonElement("Animal")]
        public string Animal { get; set; }

        [BsonElement("ServingState")]
        public string ServingState{ get; set; }

        public byte[] Image { get; set; }

        public Dictionary<string, double> GetNutritionValues()
        {
            Dictionary<string, double> res = new Dictionary<string, double>();

            res["Protein"]          = Protein;
            res["Fat"]              = Fat;
            res["Carbs"]            = Carbs;
            res["VitaminC"]         = VitaminC;
            res["Thiamin"]          = Thiamin;
            res["Riboflavin"]       = Riboflavin;
            res["Niacin"]           = Niacin;
            res["PantothenicAcid"]  = PantothenicAcid;
            res["VitaminB6"]        = VitaminB6;
            res["VitaminB12"]       = VitaminB12;
            res["Folate"]           = Folate;
            res["VitaminD"]         = VitaminD;
            res["VitaminA"]         = VitaminA;
            res["VitaminE"]         = VitaminE;
            res["VitaminK"]         = VitaminK;
            res["Calcium"]          = Calcium;
            res["Iron"]             = Iron;
            res["Phosphorus"]       = Phosphorus; 
            res["Zinc"]             = Zinc;
            res["Potassium"]        = Potassium;
            res["Sodium"]           = Sodium;
            res["Magnesium"]        = Magnesium;
            res["Fiber"]            = Fiber;

            return res;
        }

    }
}  

