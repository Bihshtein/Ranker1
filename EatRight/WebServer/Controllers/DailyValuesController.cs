using System.Linq;
using System.Web.Http;
using RestModel;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;
namespace WebServer.Controllers {
    public class DailyValuesController : ApiController {
        public HttpResponseMessage GetAll() {
            var values = new RestDBInterface().DailyValues.GetAll();
            var x = new List<List<DailyValue>>();
            x.Add(values.ToList());
            if (values.Any()) return Request.CreateResponse(HttpStatusCode.OK, x);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No products found.");
        }
    }
}