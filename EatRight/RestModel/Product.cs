using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson.Serialization.Attributes;
namespace RestModel {  

    public interface IQueryable
    {
        int ID { get; set; }
    }

    public class Product : IQueryable
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

        [BsonElement("Name1")]
        public string Name1 { get; set; }

        [BsonElement("FoodGroup")]
        public string FoodGroup { get; set; }

        [BsonElement("PreparationMethod")]
        public string PreparationMethod { get; set; }

        [BsonElement("StorageMethod")]
        public string StorageMethod { get; set; }

        [BsonElement("FatDetails")]
        public string FatDetails { get; set; }

        [BsonElement("BoneDetails")]
        public string BoneDetails { get; set; }

        [BsonElement("Name2")]
        public string Name2 { get; set; }

        [BsonElement("Name3")]
        public string Name3 { get; set; }

        [BsonElement("FoodsData")]
        public string FoodsData { get; set; }

        [BsonElement("HealthData")]
        public string HealthData { get; set; }

        [BsonElement("PeelDetails")]
        public string PeelDetails { get; set; }

        [BsonElement("PackDetails")]
        public string PackDetails { get; set; }

        public Dictionary<string, double> Weights;

        [BsonElement("Protein")]
        public double Protein { get; set; }

        [BsonElement("Fat")]
        public double Fat { get; set; }

        [BsonElement("Sugar")]
        public double Sugar { get; set; }

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

        public Dictionary<string, double> Nutrients() {
                var res = new Dictionary<string, double>();

                res["Protein"] = Protein;
                res["Fat"] = Fat;
                res["Carbs"] = Carbs;
                res["VitaminC"] = VitaminC;
                res["Thiamin"] = Thiamin;
                res["Riboflavin"] = Riboflavin;
                res["Niacin"] = Niacin;
                res["PantothenicAcid"] = PantothenicAcid;
                res["VitaminB6"] = VitaminB6;
                res["VitaminB12"] = VitaminB12;
                res["Folate"] = Folate;
                res["VitaminD"] = VitaminD;
                res["VitaminA"] = VitaminA;
                res["VitaminE"] = VitaminE;
                res["VitaminK"] = VitaminK;
                res["Calcium"] = Calcium;
                res["Iron"] = Iron;
                res["Phosphorus"] = Phosphorus;
                res["Zinc"] = Zinc;
                res["Potassium"] = Potassium;
                res["Sodium"] = Sodium;
                res["Magnesium"] = Magnesium;
                res["Fiber"] = Fiber;
                res["Sugar"] = Sugar;

            return res;
        }

        public string GetName()
        {
            if (Name2FoodGroups.Contains(FoodGroup))
            {
                return Name2;
            }
            else
            {
                return Name1;
            }
        }

        public static List<string> Name2FoodGroups = new List<string>{ "beef", "chicken" };
    }
}  

