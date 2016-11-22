using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace RestModel {

    public class RestDBInterface {
        private MongoDatabase _database;
        protected RestRepository<Product> products;
        protected RestRepository<Meal> meals;
        public RestDBInterface() {
            var client = new MongoClient();
            var server = client.GetServer();
            _database = server.GetDatabase("test");
        }
        public RestRepository<Product> Products {
            get {
                if (products == null) products = new RestRepository<Product>(_database, "products");
                return products;
            }
        }

        public RestRepository<Meal> Meals {
            get {
                if (meals == null) meals = new RestRepository<Meal>(_database, "meals");
                return meals;
            }
        }
    }

    public class RestRepository<T> where T : class {
        private MongoDatabase _database;
        public Queries<T> Queries { get; private set; }

        private string _tableName;
        private MongoCollection<T> _collection;
        public RestRepository(MongoDatabase db, string tblName) {
            _database = db;
            _tableName = tblName;
            _collection = _database.GetCollection<T>(tblName);
            Queries = new Queries<T>(_collection);

        }
        public T Get(int i) {
            return _collection.FindOneById(i);
        }

        public List<T> GetByProtein(double min) {
            return _collection.Find(Query<Product>.Where(x => x.Protein > min)).ToList();
        }



        public List<T> GetByName(string name)
        {
            return _collection.Find(Query<Product>.Where(x =>
                (Product.Name2FoodGroups.Contains(x.FoodGroup) && x.Name2.Equals(name)) ||
                (!Product.Name2FoodGroups.Contains(x.FoodGroup) && x.Name1.Equals(name))))
                .ToList();
        }


        public List<T> GetByGroupName(string name) {
            return _collection.Find(Query<Product>.Where(x => x.FoodGroup == name)).ToList();
        }
        public static List<string> FoodGroups = new List<string>() {"Carbs", "Chicken", "Beef", "Pork" ,"Vegs","Fruits","Dairy"};
        public static Dictionary<string, double> DailyValues = new Dictionary<string, double>() {
            {"Protein",56},
            {"Fiber",25},
            {"VitaminC",90},
            {"Fat",65 },
            {"Thiamin",1.2 },
            {"Riboflavin",1.3 },
            {"Niacin",18 },
            {"PantothenicAcid",5 },
            {"VitaminB6",1.3 },
            {"VitaminB12",2.4 },
            {"VitaminD",600 },
            {"VitaminA",5000 },
            {"Folate",400 },
            {"VitaminE",15 },
            {"VitaminK",80 },
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
            IMongoQuery query;
            if (isVegetarian)
                query = Query.And(Query.GT(name, DailyValues[name] * (min / 100.0)), Query.EQ("Animal", string.Empty));
            else
                query = Query.GT(name, DailyValues[name] * (min / 100.0));
            return _collection.Find(query).ToList();
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
            MongoCursor<T> cursor = _collection.FindAll();
            return cursor.AsQueryable<T>();
        }
        public List<T> GetAllList() {
            return  _collection.FindAll().ToList();
        }
        public void Add(T entity) {
            _collection.Insert(entity);
        }
        public void Delete(Expression<Func<T, string>> queryExpression, string id) {
            var query = Query<T>.EQ(queryExpression, id);
            _collection.Remove(query);
        }

        public void Empty() {
            _collection.RemoveAll();
        }
        public void Update(Expression<Func<T, string>> queryExpression, string id, T entity) {
            var query = Query<T>.EQ(queryExpression, id);
            _collection.Update(query, Update<T>.Replace(entity));
        }
    }
}