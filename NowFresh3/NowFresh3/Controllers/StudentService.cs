using RestModel;
using System.Collections.Generic;
using System.Linq;

namespace Students.Services {
    public class ProductsService  {
        private readonly RestDBInterface dbLayer;
        public ProductsService() {
            dbLayer = new RestDBInterface();
        }
        public Product Get(string i) {
            return dbLayer.Products.Get(i);
        }

        public List<Product> GetProtein(double min) {
            return dbLayer.Products.GetByProtein(min).Where(IsEmpty).ToList();
        }
        public static bool IsEmpty(Product p) {
            return p.Image != null;

        }
        public IQueryable<Product> GetAll() {
            return dbLayer.Products.GetAll().ToList().Where(IsEmpty).AsQueryable();
        }
        public void Delete(string id) {
            dbLayer.Products.Delete(s => s.Name, id);
        }
        public void Insert(Product student) {
            dbLayer.Products.Add(student);
        }
        public void Update(Product student) {
            dbLayer.Products.Update(s => s.Name, student.Name, student);
        }
    }
}