using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestModel
{
    public class ProductQueryBuilder
    {
        public static ProductQuery FromString(string queryString)
        {
            var pq = new ProductQuery();
            return pq;
        }
    }
    public class ProductQuery
    {
        public string HealthData { get; set; }
        public string PreperationMethod { get; set; }
        public string Name { get; set; }

        public Expression<Func<Product, bool>> Expression
        {
            get
            {
                Expression<Func<Product, bool>> query = x => false;
                return query;
            }

        }
    }
}
