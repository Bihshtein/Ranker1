using Logic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace RestModel {

    public class RestDBInterface {
        private IMongoDatabase _database;
        protected RestRepository<Product> products;
        protected RestRepository<Recipe> recipes;
        protected RestRepository<Recipe> testsRecipes;
        protected RestRepository<DailyValue> dailyvalues;
        public RestDBInterface() {
            var client = new MongoClient();

            _database = client.GetDatabase("test");
        }
        public RestRepository<Product> Products {
            get {
                if (products == null) products = new RestRepository<Product>(_database, "products");
                return products;
            }
        }

        public RestRepository<Recipe> Recipes {
            get {
                if (recipes == null) recipes = new RestRepository<Recipe>(_database, "recipes");
                return recipes;
            }
        }

        public RestRepository<Recipe> TestsRecipes {
            get
            {
                if (testsRecipes == null) testsRecipes = new RestRepository<Recipe>(_database, "testsRecipes");
                return testsRecipes;
            }
        }

        public RestRepository<DailyValue> DailyValues
        {
            get
            {
                if (dailyvalues == null) dailyvalues = new RestRepository<DailyValue>(_database, "dailyvalues");
                return dailyvalues;
            }
        }

        public List<Recipe> GetAllRecipes()
        {
            var resList = Recipes.GetAllList();
            resList.AddRange(TestsRecipes.GetAllList());
            return resList;
        }
    }

    public class RestRepository<T> where T : class, IQueryable {
        private IMongoDatabase _database;
        public Queries<T> Queries { get; private set; }

        private string _tableName;
        private IMongoCollection<T> _collection;
        public RestRepository(IMongoDatabase db, string tblName) {
            _database = db;
            _tableName = tblName;
            _collection = _database.GetCollection<T>(tblName);
            Queries = new Queries<T>(_collection);

        }
        public T Get(int i) {
            var ret = _collection.Find(x => x.ID == i).ToList();

            if ( (ret == null) || (ret.Count < 1)) return null;


            return ret[0];
        }


        public List<T> GetByGroupName(string name) {
            Expression<Func<Product, bool>> query = x => x.FoodGroup == name;
            return _collection.Find(query as Expression<Func<T, bool>>).ToList();
        }

        public List<T> GetByAgeAndGender(int age, GenderType gender)
        {
            //This is such a bad code- will fix on next commit- till then- ALEX DONT HURT ME.
            List<DailyValue> x = _collection.Find(_ => true).ToList() as List<DailyValue>;
            x = x.Where(y => y.Age.Within(age) && y.Gender.Within(gender)).ToList();
            return x as List<T>;
        }

        public List<T> GetRecipeById(int id)
        {
            Expression<Func<Recipe, bool>> query = x => x.ID == id;
            return _collection.Find(query as Expression<Func<T, bool>>).ToList();
        }

        public static List<string> FoodGroups = new List<string>() {"Sausages","Legumes","SoupAndSauce","Nuts","Sweets", "SpicesAndHerbs","OilsAndFats","Beverages","Baked","Fish","Carbs", "Chicken", "Beef", "Pork" ,"Vegs","Fruits","Dairy"};
        public static Dictionary<string, double> DailyValues = new Dictionary<string, double>() {
            {"Protein",56},
            {"Fiber",25},
            {"Vitamin C",90},
            {"Total lipid (fat)",65 },
            {"Thiamin",1.2 },
            {"Riboflavin",1.3 },
            {"Niacin",18 },
            //{"Pantothenic Acid",5 },
            {"Vitamin B-6",1.3 },
            {"Vitamin B-12",2.4 },
            {"Vitamin D",600 },
            {"Vitamin A",5000 },
            {"Folate",400 },
            {"Vitamin E",15 },
            {"Vitamin K (phylloquinone)",80 },
            {"Calcium",1000},
            {"Iron",11 },
            {"Magnesium",400 },
            {"Phosphorus",1000 },
            {"Potassium",3500 },
            {"Sodium",2400 },
            {"Zinc",15 }

        };

        

        public List<T> GetByMeasure(string name, int min,bool isVegetarian=false)
        {
            var builder = Builders<Product>.Filter;
            FilterDefinition<Product> filter;

            if (isVegetarian)
                filter = builder.And(builder.Gt(name, DailyValues[name] * (min / 100.0)), builder.Eq("Animal", string.Empty));
            else
                filter = builder.Gt(name, DailyValues[name] * (min / 100.0));
            return _collection.Find(filter as FilterDefinition<T>).ToList();
        }
        public T GetKey(KeyValuePair<T, int> pair)
        {
            return pair.Key;
        }
        public List<T> GetTopFoods(int minOfDailyValue) {
            return GetTopFoods(minOfDailyValue, 3);
        }

       
        public List<T> GetTopFoods(int minOfDailyValue, int minMeasures) 
        {
            Dictionary<T, int> productsCount = new Dictionary<T, int>();
            foreach (var item in DailyValues.Keys)
            {
                var results = GetByMeasure(item, minOfDailyValue);
                foreach (var product in results)
                {   
                    if (!productsCount.ContainsKey(product))
                        productsCount.Add(product, 1);
                    else
                        productsCount[product]++;
                }
            }
            var superFoods = productsCount.Where((pair) => pair.Value > minMeasures);


            var res = superFoods.ToDictionary(GetKey).Keys.ToList();
            return res;
        }

        public IQueryable<T> GetAll() {
            return _collection.Find(_ => true).ToEnumerable().AsQueryable();
        }

        public List<T> GetAllList() {
            return _collection.Find(_ => true).ToList();
        }
        public void Add(T entity) {
            _collection.InsertOne(entity);
        }
        public void Delete(Expression<Func<T, int>> queryExpression, int id) {
            var query = new FilterDefinitionBuilder<T>();
            
            _collection.DeleteMany(query.Eq(queryExpression, id));
        }

        public void Empty() {
            _collection.DeleteMany(_ => true);
        }
        public void Update(Expression<Func<T, int>> queryExpression, int id, T entity) {
            var query = new FilterDefinitionBuilder<T>();
            _collection.ReplaceOne(query.Eq(queryExpression, id), entity);
        }
    }
}