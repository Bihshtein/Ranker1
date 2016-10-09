using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

    public class RestRepository<T> where T : class {
        private MongoDatabase _database;
        private string _tableName;
        private MongoCollection<T> _collection;
        public RestRepository(MongoDatabase db, string tblName) {
            _database = db;
            _tableName = tblName;
            _collection = _database.GetCollection<T>(tblName);
        }
        public T Get(int i) {
            return _collection.FindOneById(i);
        }

        public List<T> GetByProtein(double min) {
            return _collection.Find(Query<Product>.Where(x => x.Protein > min)).ToList();
        }

        public List<T> GetByAnimal(string name)
        {
            return _collection.Find(Query<Product>.Where(x => x.Animal == name)).ToList();
        }


        public IQueryable<T> GetAll() {
            MongoCursor<T> cursor = _collection.FindAll();
            return cursor.AsQueryable<T>();
        }
        public void Add(T entity) {
            _collection.Insert(entity);
        }
        public void Delete(Expression<Func<T, string>> queryExpression, string id) {
            var query = Query<T>.EQ(queryExpression, id);
            _collection.Remove(query);
        }
        public void Update(Expression<Func<T, string>> queryExpression, string id, T entity) {
            var query = Query<T>.EQ(queryExpression, id);
            _collection.Update(query, Update<T>.Replace(entity));
        }
    }
}