using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InitDB {
    public class QueryData {
        public static string ApiKey = "fo9SmQOhJvGXLkWqXbFUGFEb89SYgVgTA1CkEbua";
        public static string Format = "json";
        public static string SingleUrl = "http://api.nal.usda.gov/ndb/nutrients/?{0}ndbno={1}&type=f&format={2}&api_key={3}";
        public static string GroupUrl = "http://api.nal.usda.gov/ndb/nutrients/?fg={0}&format={1}&api_key={2}&{3}&max=1500";
        public static List<int> Nutrients1 = new List<int> { 203, 204, 291, 205, 401, 404, 405, 406, 410, 415, 324, 418, 417, 318, 323, 269, 301, 303, 304, 307};
        public static List<int> Nutrients2 = new List<int> { 305, 306, 309, 430 };
    }


    public class MongoData {
        public static string CollectionName = "products";
        public static MongoClient _client = new MongoClient();
        public static IMongoDatabase _database = _client.GetDatabase("test");
    }
}
