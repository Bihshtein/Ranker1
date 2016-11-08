﻿using RestModel;
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

        public List<Product> GetProtein(double min, double minProteinForDay, double maxCaloriesForDay) {
            var products = dbLayer.Products.GetByProtein(min).Where(IsEmpty).ToList();
            var filteredProducts = new List<Product>();
            foreach (var product in products) {
                var totalCalories = product.Fat*7 + product.Carbs*4 + product.Protein * 4;
                var ratio = maxCaloriesForDay / totalCalories;
                var proteinForDay = ratio * product.Protein;

                if (proteinForDay > minProteinForDay)
                    filteredProducts.Add(product);
            }
            return filteredProducts;
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
            return dbLayer.Products.GetByGroupName(group);
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