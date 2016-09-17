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
            List<Product> productsList = null;
            var parts = id.Split('=');
            if (parts[0] == "Protein") 
                productsList = productsService.GetProtein(double.Parse(parts[1]));
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