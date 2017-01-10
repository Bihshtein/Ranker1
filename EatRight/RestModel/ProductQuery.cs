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
            var pq = new ProductQuery(queryString);
            return pq;
        }
    }


    public class ProductQuery
    {
        protected Product product;
        protected string searchQuery;
        protected static bool expFlag = true;
        protected static object expLocker = new object();
        protected Expression<Func<Product, bool>> _query;

        public ProductQuery(string searchQuery)
        {
            _query = null;
            this.searchQuery = (string)searchQuery.Clone();
            RebuildProduct();
        }

        protected void RebuildProduct()
        {
            lock (expLocker)
            {
                product = ProductBuilder.GetProductFromString(searchQuery);
                expFlag = true;
            }
        }

        public Expression<Func<Product, bool>> Expression
        {
            get
            {
                lock (expLocker)
                {
                    if (expFlag)
                    {
                        RefreshExpression();
                        expFlag = false;
                    }
                }

                return _query;
            }

        }

        public void RefreshExpression()
        {
            if (product == null) { _query = null; return; }
        }
    }
}
