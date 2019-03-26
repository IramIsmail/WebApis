using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApis.Models
{
    public class Customer
    {
        public Customer()
        {

        }

        public Customer(string id, string name, string parentage,string address,string password, string email_Id, string phno,  string gender, string religion)
        {
            Id = id;
            this.name = name;
            this.email_Id = email_Id;
            this.password = password;
            this.phno = phno;
            this.parentage = parentage;
            this.gender = gender;
            this.religion = religion;
            this.address = address;
        }

        public string Id { set; get; }
        public string name { set; get; }
        public string email_Id { set; get; }
        public string password { set; get; }
        public string phno { set; get; }
          
        public string parentage { set; get; }
        public string gender { set; get; }
        public string religion { set; get; }
        public string address { set; get; }

    }
}