using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;
namespace WebServer.Controllers
{
    public class RecipesController : ApiController
    {
        public HttpResponseMessage GetAll() {
            var meals = new RestDBInterface().Recipes.GetAll();
            var x = new List<List<Recipe>>();
            x.Add(meals.ToList());
            if (meals.Any()) return Request.CreateResponse(HttpStatusCode.OK, x);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No products found.");
        }

        public HttpResponseMessage Get(string id) {
            List<Recipe> productsList = new List<Recipe>() { new RestDBInterface().Recipes.Get(int.Parse(id)) };


            return Request.CreateResponse(HttpStatusCode.OK, productsList);
        }
    }
}