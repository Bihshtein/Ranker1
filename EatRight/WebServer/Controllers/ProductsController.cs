using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;

namespace Students.Services
{
    public enum ProfileType
    {
        Normal = 0,
        Vegeterian,
        BudyBuilder,
        VitaminFreak
    }
    public class SearchQueryParser
    {
        protected const char QUERY_PARAMS_DELIMITER = '=';

        protected const int LEGAL_SEARCH_PARAMS_LENGTH = 2;
        protected const int SEARCH_WORDS_INDEX = 0;
        protected const int OTHER_PARAMS_INDEX = 1;

        protected const char OTHER_PARAMS_DELIMITER = ',';
        protected const int LEGAL_OTHER_PARAMS_LENGTH = 2;
        protected const int MIN_MEASURE_INDEX = 0;
        protected const int PROFILE_TYPE_INDEX = 1;

        protected const string ANGULAR_NO_VALUE_KEYWORD = "undefined";

        protected const int DEFAULT_MIN_MEASURE = 10;
        protected const bool DEFAULT_IS_VEGETERIAN = false;

        public List<string> SearchQuery { get; protected set; }
        public bool IsVegeterian { get; protected set; }
        public int MinMeasure { get; protected set; }
        public ProfileType ProfileType { get; protected set; }

    }

    public class ProductsController : ApiController
    {
        private readonly ProductsService productsService;
        public ProductsController()
        {
            productsService = new ProductsService();
        }
        public List<List<Product>> GetProducts(string searchKeyword)
        {
            var productLists = new List<List<Product>>();
            if (RestRepository<Product>.FoodGroups.Contains(searchKeyword))
                return new List<List<Product>>() { productsService.GetFoodGroup(searchKeyword) };
            else {
                var res = productsService.GetNutrient(searchKeyword, 0);
                if (res.Count == 0)
                    res = productsService.GetIngredient(searchKeyword);
                return new List<List<Product>>() { res };
            }

        }
        public HttpResponseMessage Get(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, GetProducts(id));
        }
        public HttpResponseMessage GetAll()
        {
            var products = productsService.GetAll();
            var x = new List<List<Product>>();
            x.Add(products.ToList());
            if (products.Any()) return Request.CreateResponse(HttpStatusCode.OK, x);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No products found.");
        }
        public void Post([FromBody] Product student)
        {
            productsService.Insert(student);
        }
        public void Delete(int id)
        {
            productsService.Delete(id);
        }
        public void Put([FromBody] Product student)
        {
            productsService.Update(student);
        }
    }
}