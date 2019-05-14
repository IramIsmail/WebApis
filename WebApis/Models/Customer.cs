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

        public Customer(string id, string name, string email_Id, string password, string phno, string parentage, string gender, string religion, string address, string dob, string qualification, string profession, string workplace, string caste, string height, string hobbies, string father_occ, string mother_occ, string sibling_info, string looking_for,string mother_name,string mother_belongs)
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
            this.dob = dob;
            this.qualification = qualification;
            this.profession = profession;
            this.workplace = workplace;
            this.caste = caste;
            this.height = height;
            this.hobbies = hobbies;
            this.father_occ = father_occ;
            this.mother_occ = mother_occ;
            this.sibling_info = sibling_info;
            this.looking_for = looking_for;
            this.mother_name = mother_name;
            this.mother_belongs = mother_belongs;
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
        public string dob { set; get; }
        public string qualification { set; get; }
        public string profession { set; get; }
        public string workplace { set; get; }
        public string caste { set; get; }
        public string height { set; get; }
        public string hobbies { set; get; }
        public string father_occ { set; get; }
        public string mother_occ { set; get; }
        public string sibling_info { set; get; }
        public string looking_for { set; get; }     
        public string mother_name { set; get; }
        public string mother_belongs { set; get; }



    }


    public class RootObject
    {
        public List<Customer> logInResult { get; set; }
    }
}