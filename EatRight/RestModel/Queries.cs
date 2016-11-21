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


        public List<Product> QueryForVitaminB12(string name, string group) {
            IMongoQuery query;
            if (group == "Pork")

                query = Query<Product>.Where(x => x.Name1.Equals(name) && x.FoodGroup.Equals(group));
            else
                query = Query<Product>.Where(x => x.Name2.Equals(name) && x.FoodGroup.Equals(group));

            var res = collection.Find(query).ToList();

            var newRes = res.Cast<Product>().ToList();
            newRes.Sort((a, b) => a.VitaminB12 > b.VitaminB12 ? 1 : -1);
            newRes.RemoveAll(p => p.VitaminB12 == 0);
            return newRes;
        }

        
    }
}
