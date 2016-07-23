using RestModel;
using System.Linq;
namespace Students.Services {  
    public interface IStudentService  
    {  
        void Insert(Product student);
        Product Get(int i);
    IQueryable<Product> GetAll();
    void Delete(int id);
    void Update(Product student);
}  
}  