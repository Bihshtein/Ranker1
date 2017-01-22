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

        public SearchQueryParser(string searchQuery)
        {
            MinMeasure = DEFAULT_MIN_MEASURE;
            IsVegeterian = DEFAULT_IS_VEGETERIAN;
            ProfileType = ProfileType.Normal;
            SearchQuery = new List<string>();

            if (String.IsNullOrEmpty(searchQuery)) return;

            // TODO: handle cases where '=' is part of the search query
            var spltQuery = searchQuery.Split(QUERY_PARAMS_DELIMITER);
            if (spltQuery.Length > LEGAL_SEARCH_PARAMS_LENGTH) return;

            SearchQuery = spltQuery[SEARCH_WORDS_INDEX].Split(',').ToList();
            if ((spltQuery.Length != LEGAL_SEARCH_PARAMS_LENGTH) ||
                (string.IsNullOrEmpty(spltQuery[OTHER_PARAMS_INDEX]))) return;

            var otherParamsSplt =
                spltQuery[OTHER_PARAMS_INDEX].Split(OTHER_PARAMS_DELIMITER);

            if (otherParamsSplt.Length != LEGAL_OTHER_PARAMS_LENGTH) return;

            int tempMinMeasure = 0;
            if (int.TryParse(otherParamsSplt[MIN_MEASURE_INDEX], out tempMinMeasure))
                MinMeasure = tempMinMeasure;

            ProfileType tempProfileType = ProfileType.Normal;
            if (ProfileType.TryParse(otherParamsSplt[PROFILE_TYPE_INDEX], out tempProfileType))
                ProfileType = tempProfileType;

            IsVegeterian = ProfileType == ProfileType.Vegeterian;
        }

    }

    public class ProductsController : ApiController
    {
        private readonly ProductsService productsService;
        public ProductsController()
        {
            productsService = new ProductsService();
        }
        public List<List<Product>> ProductListCreator(SearchQueryParser sqParser)
        {
            var productLists = new List<List<Product>>();


            foreach (var searchKeyword in sqParser.SearchQuery)
            {
                if (RestRepository<Product>.DailyValues.ContainsKey(searchKeyword))
                {
                    productLists.Add(productsService.GetNutrient(searchKeyword,
                                                                 sqParser.MinMeasure,
                                                                 sqParser.IsVegeterian));
                }
                

                else if (RestRepository<Product>.FoodGroups.Contains(searchKeyword)) {
                    productLists.Add(productsService.GetFoodGroup(searchKeyword));
                }

                else  {
                    productLists.Add(productsService.GetIngredient(searchKeyword));
                }


            }
            
            return productLists;
        }
        public HttpResponseMessage Get(string id)
        {
            var sqParser = new SearchQueryParser(id);
            List<List<Product>> productsList = ProductListCreator(sqParser);
           

            return Request.CreateResponse(HttpStatusCode.OK, productsList);
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