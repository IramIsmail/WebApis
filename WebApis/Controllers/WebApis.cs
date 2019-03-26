using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApis.Firebase;
using WebApis.Models;
using Firebase.Database;
using Firebase.Utils;
using Firebase.Auth;
using Firebase;

using LiteDB;

namespace WebApis.Controllers
{

    public class UserController : ApiController
    {
       /*  [HttpGet]   
       
        public IHttpActionResult Get() {
           
            return Ok("done");
           
        }*/

        [HttpGet]
        public IHttpActionResult getuser()
        {
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users");
            System.Diagnostics.Debug.WriteLine(firebaseDBTeams +"jjj---jjjj");

            FirebaseResponse getResponse = firebaseDBTeams.Get();
            System.Diagnostics.Debug.WriteLine(getResponse.JSONContent+ "jjjjjjj");
            List<Customer> list = new List<Customer>();
            System.Diagnostics.Debug.WriteLine(JObject.Parse(getResponse.JSONContent) + "jjjj000jjj");
            //  var obj = JsonConvert.DeserializeObject<List<Customer>>(getRes00ponse.JSONContent);

            return Ok(JObject.Parse(getResponse.JSONContent));
            //   if (getResponse.Success)
            //      System.Diagnostics.Debug.WriteLine(getResponse.JSONContent);
            //  System.Diagnostics.Debug.WriteLine(getResponse.ToString());

        }

        [HttpPost]
        public IHttpActionResult authuser(String email,String password)
        {
            string url = email;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users").NodePath(uu);
            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) == "null")
            {

                System.Diagnostics.Debug.WriteLine("Doesn't exists");
               
                return null;
            }
            else
            {
                Customer c =JsonConvert.DeserializeObject<Customer>(getResponse.JSONContent);
                if (c.password == password)
                {
                    System.Diagnostics.Debug.WriteLine(getResponse.JSONContent + "Login Succesfully");
                    System.Diagnostics.Debug.WriteLine(c.email_Id + "Login Succesfully");
                    System.Diagnostics.Debug.WriteLine(c.password + "Login Succesfully");
                    return Ok(c);
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpPost]
        public IHttpActionResult create([FromBody]Customer c)
        {
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(c) + "jjjjjjj");

           
            string url = c.email_Id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users").NodePath(uu);
            
            FirebaseResponse getResponse = firebaseDBTeams.Get();
            
            
            if (Convert.ToString(getResponse.JSONContent)=="null")            
            {

                System.Diagnostics.Debug.WriteLine("PUT Request");
                FirebaseResponse putResponse = firebaseDBTeams.Put(JsonConvert.SerializeObject(c));
                System.Diagnostics.Debug.WriteLine(putResponse.Success);
                return Ok("Resgistered Successfully") ;
                
            }

            else {
                System.Diagnostics.Debug.WriteLine("Already present");
                return BadRequest("User Already Registered with this Email Id");
            }
        }
       
      /*   List<Customer> customers = new List<Customer> {
            new Customer(1,"Iram","iram@gmail.com"),
            new Customer(2,"Ismail","ismail.@gmail.com")
        };*/



    }
}
