﻿using RestModel;
using System.Collections.Generic;
using System.Linq;
namespace Students.Services {  
    public interface IProductsService  
    {  
        void Insert(Product student);
        Product Get(string i);
        IQueryable<Product> GetAll();
        void Delete(string id);
        void Update(Product student);
        List<Product> GetMain(MainCategoryTypes type);
        List<Product> GetSecond(SecondaryCategoryTypes type);
    }  
}  