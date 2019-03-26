using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApis.Firebase
{
    public class FirebaseResponse
    {
        public bool Success { get; set; }
        public String JSONContent { get; set; }
        public string ErrorMessage { get; set; }
        public HttpResponseMessage HttpResponse { get; set; }
        public FirebaseResponse()
        {
        }
        public FirebaseResponse(bool success, string errorMessage, HttpResponseMessage httpResponse = null, string jsonContent = null)
        {
            this.Success = success;
            this.JSONContent = jsonContent;
            this.ErrorMessage = errorMessage;
            this.HttpResponse = httpResponse;
        }
    }
}