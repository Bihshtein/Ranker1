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
        public static List<int> Nutrients = new List<int> { 203, 204, 291, 205, 401, 404, 405, 406, 410, 415, 454, 418, 417, 318, 323, 430, 301, 303, 304,/* 305, 306, 307,*/ 309 };
    }

    public class GroupData {
        public string Name { get; set; }
        public Func<string, bool> ValidationFunction { get; set; }
        public string IDNum { get; set; }
        public List<string> MainTypeOptions { get; set; }
        public List<string> SecondTypeOptions { get; set; }
        public List<string> CutTypeOptions { get; set; }
    }

    public class MongoData {
        public static string CollectionName = "products";
        public static MongoClient _client = new MongoClient();
        public static IMongoDatabase _database = _client.GetDatabase("test");
    }
}
