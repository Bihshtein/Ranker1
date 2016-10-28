using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using RestModel;

namespace Students.Services
{
    public class ValuesController : ApiController
    {

        protected List<Option> options;

        public class Option
        {
            public string id;
            public string name;

            public Option()
            {
                this.id = "unset";
                this.name = "unset";
            }
            public Option(string name)
            {
                this.id = name;
                this.name = name;
            }
        }
        public ValuesController()
        {
            populateOptions();
        }

        protected void populateOptions()
        {
            this.options = new List<Option>();

            foreach (var opt in Enum.GetValues(typeof(ProfileType)))
            {
                options.Add(new Option(opt.ToString()));
            }
        }

        public HttpResponseMessage Get(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK, options);
        }
        public HttpResponseMessage GetAll()
        {
            return Get(null);
        }
    }
}
