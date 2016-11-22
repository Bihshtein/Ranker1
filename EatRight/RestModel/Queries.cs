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


        public List<Product> QueryByNameAndValue(string name, string group, string value) {
            IMongoQuery query;
            if (Product.Name2FoodGroups.Contains(group) )
                query = Query<Product>.Where(x => x.Name2.Equals(name) && x.FoodGroup.Equals(group));
            else
                query = Query<Product>.Where(x => x.Name1.Equals(name) && x.FoodGroup.Equals(group));


            var res = collection.Find(query).ToList();

            var newRes = res.Cast<Product>().ToList();
            newRes.Sort((a, b) => a.Nutrients()[value] > b.Nutrients()[value] ? 1 : -1);
            newRes.RemoveAll(p => p.Nutrients()[value] == 0);
            return newRes;
        }

        
    }
}
