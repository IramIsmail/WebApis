using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApis.Models
{
    public class AuthModel
    {

        public string email_Id { set; get; }
        public string password { set; get; }


        public AuthModel() {
        }

        public AuthModel(string email_Id,string password)
        {
            this.email_Id = email_Id;
            this.password = password;
        }
    }
}