using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;
using System.Web;

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




        [HttpGet]
        public HttpResponseMessage Get(string id) {
            var query = ActionContext.Request.RequestUri.Query;
            var _params = HttpUtility.ParseQueryString(query);
            string _id = string.Empty;
            string _filtersStr;
            List<FilterType> _filters;
            if (_params.AllKeys.Contains("id"))
                _id = _params["id"];
            if (_params.AllKeys.Contains("filters"))
                _filtersStr = _params["filters"];
            List<Recipe> productsList = new List<Recipe>() { new RestDBInterface().Recipes.Get(int.Parse(_id)) };
            return Request.CreateResponse(HttpStatusCode.OK, productsList);
        }
    }
}