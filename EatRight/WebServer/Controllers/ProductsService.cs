using RestModel;
using System.Collections.Generic;
using System.Linq;

namespace Students.Services {
    public class ProductsService  {
        private readonly RestDBInterface dbLayer;
        public ProductsService() {
            dbLayer = new RestDBInterface();
        }
        public Product Get(int i) {
            return dbLayer.Products.Get(i);
        }

    
        public List<Product> GetNutrient(string name, int min, bool vegetarian)
        {
            return dbLayer.Products.GetByMeasure(name, min, vegetarian);
        }

        public List<Product> GetTopFoods(int min)
        {
            return dbLayer.Products.GetTopFoods(min);
        }

        public List<Product> GetFoodGroup(string group) {
            return dbLayer.Products.GetByGroupName(group.ToLower());
        }

       
        public IQueryable<Product> GetAll() {
            return dbLayer.Products.GetAll();
        }
        public void Delete(string id) {
            dbLayer.Products.Delete(s => s.Name1, id);
        }
        public void Insert(Product student) {
            dbLayer.Products.Add(student);
        }
        public void Update(Product student) {
            dbLayer.Products.Update(s => s.ID, student.ID, student);
        }
    }
}