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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Text;
using System.IO;
using Firebase.Storage;

namespace WebApis.Controllers
{

    public class UserController : ApiController
    {
       /*  [HttpGet]   
       
        public IHttpActionResult Get() {
           
            return Ok("done");
           
        }*/

        [HttpGet]     
        [Route("api/user/list/")]
        public IHttpActionResult list(String id)
        {
            string url = id;
            string uu = url.Replace(".", ","); 
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users");
            

            FirebaseResponse getResponse = firebaseDBTeams.Get();
            System.Diagnostics.Debug.WriteLine(getResponse.JSONContent+ "jjjjjjj");
            Customer c = JsonConvert.DeserializeObject<Customer>(getResponse.JSONContent);
           
          //  List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(getResponse.JSONContent);
            List<Customer> newlist = new List<Customer>();
            System.Diagnostics.Debug.WriteLine(JObject.Parse(getResponse.JSONContent) + "jjjj000jjj");
           
            var obj = JsonConvert.DeserializeObject<RootObject>(getResponse.JSONContent);
            var mList = JsonConvert.DeserializeObject<IDictionary<string, Customer>>(getResponse.JSONContent);
           // System.Diagnostics.Debug.WriteLine(obj.logInResult.Count() + "======current=========");
            System.Diagnostics.Debug.WriteLine(mList.Count + "=mmmm=====current=========");


            foreach (var v in mList)
            {
                if (v.Value.Id != id)
                {
                    newlist.Add(v.Value);
                }
                System.Diagnostics.Debug.WriteLine(v.Value.Id + "===============");
            }
            System.Diagnostics.Debug.WriteLine(newlist.Count()+ "======new=========");
            // return Ok(JObject.Parse(getResponse.JSONContent));
            return Ok(newlist);
            //   if (getResponse.Success)
            //      System.Diagnostics.Debug.WriteLine(getResponse.JSONContent);
            //  System.Diagnostics.Debug.WriteLine(getResponse.ToString());

        }

        [HttpPost]
        [Route("api/user/authuser")]
        public IHttpActionResult authuser([FromBody]AuthModel c)
        {
            string url = c.email_Id;
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
                Customer c2 =JsonConvert.DeserializeObject<Customer>(getResponse.JSONContent);
                if (c2.password == c.password)
                {
                    System.Diagnostics.Debug.WriteLine(getResponse.JSONContent + "Login Succesfully");
                   
                    return Ok(c2);
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpPost]
        [Route("api/user/create")]
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


        [HttpPost]
        [Route("api/user/upload")]
        public IHttpActionResult upload([FromBody]ImageModel imageModel) {
            try
            {
              
                string key = imageModel.title.Replace(".", ",");
                System.Diagnostics.Debug.WriteLine(imageModel.title + "jjjjjjjj");
            System.Diagnostics.Debug.WriteLine(imageModel.image + "lllllllllllll");
                System.Diagnostics.Debug.WriteLine("==========================================");
                System.Diagnostics.Debug.WriteLine(imageModel.document + "lllllll000llllll");

                FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
                FirebaseDB firebaseDBTeams = firebaseDB.Node("Pictures").NodePath(key);

                FirebaseResponse getResponse = firebaseDBTeams.Get();


               
                    System.Diagnostics.Debug.WriteLine("PUT Request");
                    FirebaseResponse putResponse = firebaseDBTeams.Put(JsonConvert.SerializeObject(imageModel));
                    System.Diagnostics.Debug.WriteLine(putResponse.Success);
                return Ok(imageModel);
                

               



            }
            catch (Exception f) {
                return BadRequest("Errorrr");
            }

        }


        public Bitmap stringToImage(string inputString)
        {
            byte[] imageBytes = Encoding.Unicode.GetBytes(inputString);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return new Bitmap(ms);
            }
        }


        [HttpPatch]
        [Route("api/user/update")]
        public IHttpActionResult update([FromBody]Customer c)
        {
            
            string uu = c.Id.Replace(".", ",");
            System.Diagnostics.Debug.WriteLine(uu+ "jjjjjjj----");

            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(c) + "jjjjjjj");
            
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                System.Diagnostics.Debug.WriteLine("PATCH Request");
                FirebaseResponse putResponse = firebaseDBTeams.Patch(JsonConvert.SerializeObject(c));
                System.Diagnostics.Debug.WriteLine(putResponse.Success);
                return Ok("Updated Successfully");

            }

            else
            {
                System.Diagnostics.Debug.WriteLine("Already present");
                return BadRequest("User is not Registered with this Email Id");
            }
        }






        [HttpPost]
        [Route("api/user/getimage")]
        public IHttpActionResult getImageModel(String id)
        {
            System.Diagnostics.Debug.WriteLine(id+"jjjjj");
           
           string uu = id.Replace(".", ",");

            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Pictures").NodePath(uu);
            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) == "null")
            {
                System.Diagnostics.Debug.WriteLine("Doesn't exists");

                return null;
            }
            else
            {
                ImageModel c2 = JsonConvert.DeserializeObject<ImageModel>(getResponse.JSONContent);
                if (c2!=null)
                {
                    System.Diagnostics.Debug.WriteLine(getResponse.JSONContent + "Getting ImageModel");

                    return Ok(c2);
                }
                else
                {
                    return null;
                }
            }
        }
    }

   
}
