﻿using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;

namespace Students.Services {
    public class ProductsController : ApiController {
        private readonly ProductsService productsService;
        public ProductsController() {
            productsService = new ProductsService();
        }
        public HttpResponseMessage Get(string id) {
            var parts = id.Split('=');
            int min = 10;
            int products =1;
            if (parts.Length > 1)
            {
                id = parts[0];
                var _params = parts[1].Split(',');
                if (_params.Length > 1)
                {
                    min = int.Parse(_params[0]);
                    products = int.Parse(_params[1]);
                }
                else
                    min = int.Parse(parts[1]);
            }
            List<Product> productsList = null;
            if (RestRepository<Product>.DailyValues.Keys.ToList().Contains(id))
                productsList = productsService.GetMeasure(id,min);
            else if (RestRepository<Product>.Animals.Contains(id))
                productsList = productsService.GetAnimal(id);
            else if (id == "TopFoods")
                productsList = productsService.GetTopFoods(min,products);

                return Request.CreateResponse(HttpStatusCode.OK, productsList);
        }
        public HttpResponseMessage GetAll() {
            var products = productsService.GetAll();
            if (products.Any()) return Request.CreateResponse(HttpStatusCode.OK, products);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No products found.");
        }
        public void Post([FromBody] Product student) {
            productsService.Insert(student);
        }
        public void Delete(string id) {
            productsService.Delete(id);
        }
        public void Put([FromBody] Product student) {
            productsService.Update(student);
        }
    }
}