using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
namespace Students.Services {
    public class StudentsController : ApiController {
        private readonly IStudentService _studentService;
        public StudentsController() {
            _studentService = new StudentService();
        }
        // GET api/student/id  
        public HttpResponseMessage Get(int id) {
            var student = _studentService.Get(id);
            if (student != null) return Request.CreateResponse(HttpStatusCode.OK, student);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found for provided id.");
        }
        public HttpResponseMessage GetAll() {
            var students = _studentService.GetAll();
            if (students.Any()) return Request.CreateResponse(HttpStatusCode.OK, students);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No students found.");
        }
        public void Post([FromBody] Rest student) {
            _studentService.Insert(student);
        }
        public void Delete(int id) {
            _studentService.Delete(id);
        }
        public void Put([FromBody] Rest student) {
            _studentService.Update(student);
        }
    }
}