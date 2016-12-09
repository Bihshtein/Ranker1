using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestModel {
    public class Queries<T> {
        public MongoCollection<T> collection { get; private set; }
        public Queries(MongoCollection<T> collection) {
            this.collection = collection;
        }


        public List<Product> QueryByNameAndValue(string name, string group, string value,bool partial=false) {
            IMongoQuery query;
            if (Product.Name2FoodGroups.Contains(group) )
                query = Query<Product>.Where(x => x.Name2.Equals(name) && x.FoodGroup.Equals(group));
            else
                if (partial)
                    query = Query<Product>.Where(x => x.Name1.Contains(name) && x.FoodGroup.Equals(group));
                else
                    query = Query<Product>.Where(x => x.Name1.Equals(name) && x.FoodGroup.Equals(group));

            var res = collection.Find(query).ToList();

            var newRes = res.Cast<Product>().ToList();
            newRes.RemoveAll(p => p.Nutrients()[value] == 0);
            newRes.Sort((a, b) => a.Nutrients()[value] > b.Nutrients()[value] ? 1 : -1);
            return newRes;
        }

        public List<DailyValue> GetByGender(GenderType gender) {
            IMongoQuery query;
            query = Query<DailyValue>.Where(x => x.Gender.Type.Equals(gender));
            var res = collection.Find(query).ToList();
            var newRes = res.Cast<DailyValue>().ToList();
            newRes.Sort((a, b) => a.Age.MinAge > b.Age.MinAge ? 1 : -1);
            return newRes;
        }
        public List<T> GetByName(string name) {
            string lowerCasedName = name.ToLower();
            return collection.Find(Query<Product>.Where(x =>
                (Product.Name2FoodGroups.Contains(x.FoodGroup) && (x.Name2.Equals(name) || x.Name2.Equals(lowerCasedName))) ||
                (!Product.Name2FoodGroups.Contains(x.FoodGroup) && (x.Name1.Equals(name) || x.Name1.Equals(lowerCasedName)))))
                .ToList();
        }




    }
}
