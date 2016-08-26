using MongoDB.Driver;
using System.Configuration;
namespace RestModel {
    public class RestDBInterface {
        private MongoDatabase _database;
        protected RestRepository<Product> products;
        public RestDBInterface() {
            var client = new MongoClient();
            var server = client.GetServer();
            _database = server.GetDatabase("test");
        }
        public RestRepository<Product> Products {
            get {
                if (products == null) products = new RestRepository<Product>(_database, "products");
                return products;
            }
        }


    }
}