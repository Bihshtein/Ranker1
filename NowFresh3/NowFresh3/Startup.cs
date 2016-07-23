using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

[assembly: OwinStartup(typeof(NowFresh3.Startup))]

namespace NowFresh3
{

    public partial class Startup
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            _client = new MongoClient();
            _database = _client.GetDatabase("test");

            var collection = _database.GetCollection<BsonDocument>("restaurants");
            var filter = Builders<BsonDocument>.Filter.Eq("borough", "Manhattan");
            var result = collection.Find(filter).ToList();
        }
    }
}
