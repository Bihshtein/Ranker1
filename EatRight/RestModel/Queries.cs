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


        public List<T> GetChickenBreast() {
            return collection.Find(Query<Product>.Where(x => x.Name2.Equals("breast") && x.FoodGroup.Equals("Chicken"))).ToList();

        }
    }
}
