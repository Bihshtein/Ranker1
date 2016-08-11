using RestModel;
using System.Collections.Generic;
using System.Linq;
namespace Students.Services {  
    public interface IStudentService  
    {  
        void Insert(Product student);
        Product Get(string i);
    IQueryable<Product> GetAll();
    void Delete(string id);
    void Update(Product student);
        List<Product> GetMain(MainCategoryTypes type);
    }  
}  