using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApis.Models
{
    public class Request
    {
        public Request()
        {
        }

        public string senderId { get; set; }
        public string destinationId { get; set; }
        public string status { get; set; }


    }
}