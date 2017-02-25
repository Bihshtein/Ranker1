using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InitProducts {
    public class QueryData {
        public static string ApiKey = "fo9SmQOhJvGXLkWqXbFUGFEb89SYgVgTA1CkEbua";
        public static string Format = "json";
        public static string SingleUrl = "http://api.nal.usda.gov/ndb/reports/?ndbno={0}&type=f&format={1}&api_key={2}";
        public static string GroupUrl = "http://api.nal.usda.gov/ndb/nutrients/?fg={0}&format={1}&api_key={2}&{3}&max=1500";
        
    }


    public class MongoData {
        public static string CollectionName = "products";
        public static MongoClient _client = new MongoClient();
        public static IMongoDatabase _database = _client.GetDatabase("test");
    }
}
