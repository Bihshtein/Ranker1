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
        public Product Product { get; set; }
        protected string searchQuery;
        protected static bool expFlag = true;
        protected static object expLocker = new object();
        protected Expression<Func<Product, bool>> _expression;

        public ProductQuery(string searchQuery)
        {
            _expression = null;
            this.searchQuery = (string)searchQuery.Clone();
            RebuildProduct();
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

                return _expression;
            }
        }
        protected void RebuildProduct()
        {
            lock (expLocker)
            {
                Product = ProductBuilder.GetProductFromString(searchQuery);
                expFlag = true;
            }
        }

        public void RefreshExpression()
        {
            if (Product == null) { _expression = null; return; }
        }
    }
}
