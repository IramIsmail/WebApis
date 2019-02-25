using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApis.Controllers
{
    public class WebController : ApiController
    {

        public string Get() {
            return "Welcome to our Web Api";
        }

        public List<string> Get(int id) {

            return new List<string>{
                "data1","data2"
                    };
        }
    }
}
