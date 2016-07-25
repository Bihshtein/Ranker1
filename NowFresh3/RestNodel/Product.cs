using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson.Serialization.Attributes;
namespace RestModel {  
    public enum MainCategoryTypes { Fruit, Veg, Other}
    public enum SecondaryCategoryTypes { Citrus, Tree, Bush, Tropic, Nut, Root, Flower, Fruit, Seed, Leaf, Tuber,Fungus }
    public enum StateTypes { Fresh,Dried, Pickled, Fried }
    public enum StoreAmountType{ Exists, NotInStore}


    public class Product
    {
        [BsonElement("_id")]
        public string SpeciesName { get; set; }
        public MainCategoryTypes MainCategory { get; set; }
        public SecondaryCategoryTypes SecondaryCategory { get; set; }
        public byte[] Image { get; set; }
        
    }

    public class ProductInStore : Product {
        public StateTypes State { get; set; }
        public double Price { get; set; }
        public string SpecificName { get; set; }
        public StoreAmountType StoreAmount { get; set; }

    }
}  