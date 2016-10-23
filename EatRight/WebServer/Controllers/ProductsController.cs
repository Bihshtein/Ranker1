using System.Linq;
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
            bool vegetarian = false;
            if (parts.Length > 1)
            {
                id = parts[0];
                if ( parts[1] != "undefined,undefined") {
                    var _params = parts[1].Split(',');
                    min = int.Parse(_params[0]);

                    if (_params.Length > 1 && !_params.Contains("undefined"))
                        vegetarian = _params.Contains("Vegetarian");
                }
            }
            List<Product> productsList = null;
            if (RestRepository<Product>.DailyValues.Keys.ToList().Contains(id))
                productsList = productsService.GetMeasure(id,min,vegetarian);
            else if (RestRepository<Product>.Animals.Contains(id))
                productsList = productsService.GetAnimal(id);
            else if (id == "TopFoods")
                productsList = productsService.GetTopFoods(min);

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