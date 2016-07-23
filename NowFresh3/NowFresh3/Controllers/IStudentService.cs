using RestModel;
using System.Linq;
namespace Students.Services {  
    public interface IStudentService  
    {  
        void Insert(Rest student);
        Rest Get(int i);
    IQueryable<Rest> GetAll();
    void Delete(int id);
    void Update(Rest student);
}  
}  