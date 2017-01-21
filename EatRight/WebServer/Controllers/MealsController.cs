using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;
using RecommendationBuilder;
using LogicRunner;
using Logic;

namespace WebServer.Controllers {
    public class MealsController : ApiController {

        public HttpResponseMessage GetAll() {
            var recipes = new RestDBInterface().Recipes.GetAll().ToList();
            var meals = new List<Meal>();
            recipes.ForEach(a => meals.Add(new Meal() { Recipe = a }));
            var x = new List<List<Meal>>();
            x.Add(meals);
            if (meals.Any()) return Request.CreateResponse(HttpStatusCode.OK, x);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No products found.");
        }

        public HttpResponseMessage Get(string id) {
            var parts = id.Split('_');

            var mealsList = Hack.GetRecommendation(
                int.Parse(parts[0]),
                (GenderType)Enum.Parse(typeof(GenderType), parts[1])
                );
                


            return Request.CreateResponse(HttpStatusCode.OK, mealsList);
        }
    }
}