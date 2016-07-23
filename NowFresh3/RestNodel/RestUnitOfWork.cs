using MongoDB.Driver;
using System.Configuration;
namespace RestModel {
    public class RestUnitOfWork {
        private MongoDatabase _database;
        protected RestRepository<Product> products;
        public RestUnitOfWork() {
            var client = new MongoClient();
            var server = client.GetServer();
            _database = server.GetDatabase("test");
            _database.GetCollection("products").Drop();
        }
        public RestRepository<Product> Products {
            get {
                if (products == null) products = new RestRepository<Product>(_database, "products");
                return products;
            }
        }


    }
}