using RestModel;
using System.Collections.Generic;
using System.Linq;

namespace Students.Services {
    public class ProductsService : IProductsService {
        private readonly RestDBInterface dbLayer;
        public ProductsService() {
            dbLayer = new RestDBInterface();
        }
        public Product Get(string i) {
            return dbLayer.Products.Get(i);
        }

        public List<Product> GetMain(MainCategoryTypes type) {
            return dbLayer.Products.GetByMainCategory(type).Where(IsEmpty).ToList();
        }
        public static bool IsEmpty(Product p) {
            return p.Image != null;

        }
        public List<Product> GetSecond(SecondaryCategoryTypes type) {
            return dbLayer.Products.GetBySecondCategory(type).Where(IsEmpty).ToList(); 
        }
        public IQueryable<Product> GetAll() {
            return dbLayer.Products.GetAll().ToList().Where(IsEmpty).AsQueryable();
        }
        public void Delete(string id) {
            dbLayer.Products.Delete(s => s.SpeciesName, id);
        }
        public void Insert(Product student) {
            dbLayer.Products.Add(student);
        }
        public void Update(Product student) {
            dbLayer.Products.Update(s => s.SpeciesName, student.SpeciesName, student);
        }
    }
}