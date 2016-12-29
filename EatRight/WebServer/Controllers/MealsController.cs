using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;
namespace WebServer.Controllers
{
    public class MealsController : ApiController
    {
        public HttpResponseMessage GetAll() {
            var meals = new RestDBInterface().Recipes.GetAll();
            var x = new List<List<Recipe>>();
            x.Add(meals.ToList());
            if (meals.Any()) return Request.CreateResponse(HttpStatusCode.OK, x);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No products found.");
        }
    }
}