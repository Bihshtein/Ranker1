using MongoDB.Driver;
using System.Configuration;
namespace RestModel {
    public class StudentsUnitOfWork {
        private MongoDatabase _database;
        protected RestRepository<Rest> _students;
        public StudentsUnitOfWork() {
            var client = new MongoClient();
            var server = client.GetServer();
            _database = server.GetDatabase("test");
        }
        public RestRepository<Rest> Students {
            get {
                if (_students == null) _students = new RestRepository<Rest>(_database, "students");
                return _students;
            }
        }
    }
}