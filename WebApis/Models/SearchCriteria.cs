using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApis.Models
{
    public class SearchCriteria
    {
    public string age;
     
    public string occupation;
     
    public string address;
   
    public string gender;

        public SearchCriteria(string age, string occupation, string address, string gender)
        {
            this.age = age;
            this.occupation = occupation;
            this.address = address;
            this.gender = gender;
        }

        public SearchCriteria()
        {
        }
    }
}