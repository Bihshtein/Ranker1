using RestModel;
using System.Linq;

namespace Students.Services {
    public class StudentService : IStudentService {
        private readonly RestUnitOfWork _sUnitOfwork;
        public StudentService() {
            _sUnitOfwork = new RestUnitOfWork();
        }
        public Product Get(int i) {
            return _sUnitOfwork.Products.Get(i);
        }
        public IQueryable<Product> GetAll() {
            return _sUnitOfwork.Products.GetAll();
        }
        public void Delete(int id) {
            _sUnitOfwork.Products.Delete(s => s.ID, id);
        }
        public void Insert(Product student) {
            _sUnitOfwork.Products.Add(student);
        }
        public void Update(Product student) {
            _sUnitOfwork.Products.Update(s => s.ID, student.ID, student);
        }
    }
}