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
        [BsonElement("Weights")]
        public Dictionary<string, double> Weights;

        
        public List<KeyValuePair<string, double>> NutrientsList() {
            var prodNutValues = this.Nutrients["Proximates"].ToList();
            if (this.Nutrients.ContainsKey("Minerals"))
            prodNutValues.AddRange(this.Nutrients["Minerals"].ToList());
            if (this.Nutrients.ContainsKey("Vitamins"))
                prodNutValues.AddRange(this.Nutrients["Vitamins"].ToList());
            if (this.Nutrients.ContainsKey("Lipids"))
                prodNutValues.AddRange(this.Nutrients["Lipids"].ToList());
            return prodNutValues;
        }
        [BsonElement("Types")]
        public HashSet<ProductType> Types { get; set; }

        [BsonElement("Nutrients")]
        public Dictionary<string,Dictionary<string, double>> Nutrients { get; set; }

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

        public Boolean NameContains(string str)
        {
            return (Name1 != null && Name1.Contains(str)) ||
                (Name2 != null && Name2.Contains(str)) ||
                (Name3 != null && Name3.Contains(str));
        }

        public String PrettyPrint()
        {
            return (Name1 + "," + Name2 + "," + Name3 + " (" + FoodGroup + ")");
        }
    }

    public enum ProductType
    {
        None = 0,

        Meat,
        Dairy,
        Fish,
        Seafood,
        FromAnimal,
        NonKosher
    }
}  

