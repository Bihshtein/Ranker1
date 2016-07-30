using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;

namespace Students.Services {
    public class StudentsController : ApiController {
        private readonly IStudentService _studentService;
        public StudentsController() {
            _studentService = new StudentService();
        }
        public HttpResponseMessage Get(string id) {
            var allItems = id.Split(',').ToList();
            List<Product> productsList = new List<Product>();
            allItems.ForEach((item) => productsList.Add(_studentService.Get(item)));
            if (allItems != null) return Request.CreateResponse(HttpStatusCode.OK, productsList);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Student not found for provided id.");
        }
        public HttpResponseMessage GetAll() {
            var students = _studentService.GetAll();
            if (students.Any()) return Request.CreateResponse(HttpStatusCode.OK, students);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No students found.");
        }
        public void Post([FromBody] Product student) {
            _studentService.Insert(student);
        }
        public void Delete(string id) {
            _studentService.Delete(id);
        }
        public void Put([FromBody] Product student) {
            _studentService.Update(student);
        }
    }
}