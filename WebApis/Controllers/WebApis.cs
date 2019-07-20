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
using System.Dynamic;

namespace WebApis.Controllers
{

    public class UserController : ApiController
    {
      

        [HttpGet]     
        [Route("api/user/list/")]
        public IHttpActionResult list(String id)
        {
            string url = id;
            string uu = url.Replace(".", ","); 
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users");
            

            FirebaseResponse getResponse = firebaseDBTeams.Get();
           
            Customer c = JsonConvert.DeserializeObject<Customer>(getResponse.JSONContent);
           
            List<Customer> newlist = new List<Customer>();
           
            var obj = JsonConvert.DeserializeObject<RootObject>(getResponse.JSONContent);
            var mList = JsonConvert.DeserializeObject<IDictionary<string, Customer>>(getResponse.JSONContent);
           

            foreach (var v in mList)
            {
                if (v.Value.Id != id)
                {
                    newlist.Add(v.Value);
                }
            }
            return Ok(newlist);
           

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
                return Ok("Resgistered Successfully") ;
                
            }

            else {
                return BadRequest("User Already Registered with this Email Id");
            }
        }
        


        [HttpPost]
        [Route("api/user/upload")]
        public IHttpActionResult upload([FromBody]ImageModel imageModel) {
            try
            {
              
                string key = imageModel.title.Replace(".", ",");

                FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
                FirebaseDB firebaseDBTeams = firebaseDB.Node("Pictures").NodePath(key);

                FirebaseResponse getResponse = firebaseDBTeams.Get();
                FirebaseResponse putResponse = firebaseDBTeams.Put(JsonConvert.SerializeObject(imageModel));
                return Ok(imageModel);
   
            }
            catch (Exception f) {
                return BadRequest("Error");
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
          
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                FirebaseResponse putResponse = firebaseDBTeams.Patch(JsonConvert.SerializeObject(c));
                return Ok("Updated Successfully");

            }

            else
            {
                return BadRequest("User is not Registered with this Email Id");
            }
        }






        [HttpPost]
        [Route("api/user/getimage")]
        public IHttpActionResult getImageModel(String id)
        {
           
           string uu = id.Replace(".", ",");

            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Pictures").NodePath(uu);
            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) == "null")
            {

                return null;
            }
            else
            {
                ImageModel c2 = JsonConvert.DeserializeObject<ImageModel>(getResponse.JSONContent);
                if (c2!=null)
                {

                    return Ok(c2);
                }
                else
                {
                    return null;
                }
            }
        }


        [HttpGet]
        [Route("api/user/getuser")]
        public IHttpActionResult getClient(String id)
        {

            string uu = id.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) == "null")
            {

                return null;

            }

            else
            {
                Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse.JSONContent);
                return Ok(c2);
            }
        }




        [HttpPost]
        [Route("api/user/add")]
        public IHttpActionResult add([FromBody]Request request)
        {

            RequestDTO requestDTO = new RequestDTO();
            
            string loginId = request.senderId.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Requests").NodePath(loginId).Node(request.destinationId.Replace(".", ","));
            FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Proposals").NodePath(request.destinationId.Replace(".", ",")).Node(loginId);


            requestDTO.status =request.destinationId.Replace(".", ",");

            dynamic foo = new ExpandoObject();
            foo.status = request.status;
          
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);
            FirebaseResponse getResponse = firebaseDBTeams.Get();

            if (Convert.ToString(getResponse.JSONContent) == "null")
            {
                FirebaseResponse putResponse = firebaseDBTeams.Post(JsonConvert.SerializeObject(json));

                FirebaseResponse putResponse2 = firebaseDBTeams2.Post(JsonConvert.SerializeObject(json)); 
                return Ok("Resgistered Successfully");

            }

            else
            {
                System.Diagnostics.Debug.WriteLine("Already present");
                return BadRequest("User Already Registered with this Email Id");
            }
        }


        [HttpPost]
        [Route("api/user/addresponds")]
        public IHttpActionResult addResponds([FromBody]Request request)
        {

            RequestDTO requestDTO = new RequestDTO();

            
            string loginId = request.senderId.Replace(".", ",");

            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("RespondsBy").NodePath(loginId).Node(request.destinationId.Replace(".", ","));
            FirebaseDB firebaseDBTeams2 = firebaseDB.Node("RespondsTo").NodePath(request.destinationId.Replace(".", ",")).Node(loginId);
            FirebaseDB firebaseDBTeams3 = firebaseDB.Node("Requests").NodePath(loginId).Node(request.destinationId.Replace(".", ","));
            FirebaseDB firebaseDBTeams4 = firebaseDB.Node("Proposals").NodePath(loginId).Node(request.destinationId.Replace(".", ","));

            requestDTO.status = request.destinationId.Replace(".", ",");

            dynamic foo = new ExpandoObject();
            foo.status = request.status;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);
            FirebaseResponse getResponse = firebaseDBTeams.Get();
            FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
            FirebaseResponse getResponse3 = firebaseDBTeams3.Get();


           
                FirebaseResponse deleteResponse = firebaseDBTeams3.Delete();
                FirebaseResponse deleteResponse2 = firebaseDBTeams4.Delete();
            
            if (Convert.ToString(getResponse.JSONContent) == "null")
            {
                FirebaseResponse putResponse = firebaseDBTeams.Post(JsonConvert.SerializeObject(json));
                firebaseDBTeams2.Post(JsonConvert.SerializeObject(json));
              
                return Ok("Respond Successfully");

            }

            else
            {
                System.Diagnostics.Debug.WriteLine("Already present");
                return BadRequest("User Already Registered with this Email Id");
            }
        }
        
        [HttpPost]
        [Route("api/user/search")]
        public IHttpActionResult searchuser([FromBody]SearchCriteria c)
        {
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users");
            FirebaseResponse getResponse = firebaseDBTeams.Get();
            List<Customer> newlist = new List<Customer>();

            var obj = JsonConvert.DeserializeObject<RootObject>(getResponse.JSONContent);
            var mList = JsonConvert.DeserializeObject<IDictionary<string, Customer>>(getResponse.JSONContent);
            int min, max;
            
            if (c.age == "0") {
                min = 20;
                max = 25;
            }
            else if(c.age=="1"){
                min = 25;
                max = 30;
            }
            else if(c.age=="2"){
                min = 30;
                max = 35;
            }
            else
            {
                min = 35;
                max =45;
            }


            foreach (var v in mList)            
            {
                string[] splitString = v.Value.dob.Split('/');

                int birthDay = int.Parse(splitString[0].Trim());
                int birthMonth = int.Parse(splitString[1].Trim());
                int birthYear  = int.Parse(splitString[2].Trim());
                double year;
                DateTime birthDate = new DateTime(birthYear, birthMonth, birthDay);
                DateTime currentDate = new DateTime();
                currentDate = DateTime.Now;
                TimeSpan age = new TimeSpan();
                age = currentDate - birthDate;
                year = age.Days / 365;
                if (v.Value.address==c.address && v.Value.profession==c.occupation && v.Value.gender==c.gender && (year>=min && year<=max))
                {
                    newlist.Add(v.Value);
                }
                
            }

            return Ok(newlist);
        }
        [HttpGet]
        [Route("api/user/requests/")]
        public IHttpActionResult requestlist(String id)
        {
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Requests").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();
           
            if (Convert.ToString(getResponse.JSONContent) != "null")
            {
                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
                List<Customer> newlist = new List<Customer>();

                foreach (var v in mList)
                {
                    FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                    FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                    Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                    newlist.Add(c2);
                }

                return Ok(newlist);
            }
            else {
                return null;
            }
        }

        
        [HttpGet]
        [Route("api/user/proposals/")]
        public IHttpActionResult proposallist(String id)
        {
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Proposals").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
                  List<Customer> newlist = new List<Customer>();

              foreach (var v in mList)
                  {
                FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                newlist.Add(c2);
                 }

                return Ok(newlist);
            }
        else{
                return null;
        }
        }


        [HttpGet]
        [Route("api/user/respondedtolist/")]
        public IHttpActionResult respondedtolist(String id)
        {
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("RespondsBy").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
                List<Customer> newlist = new List<Customer>();

                foreach (var v in mList)
                {
                    FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                    FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                    Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                    newlist.Add(c2);
                  
                }

                return Ok(newlist);
            }
            else
            {
                return null;
            }
        }



        [HttpGet]
        [Route("api/user/respondedbylist/")]
        public IHttpActionResult respondedBylist(String id)
        {
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("RespondsTo").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
                List<Customer> newlist = new List<Customer>();

                foreach (var v in mList)
                {
                    FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                    FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                    Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                    newlist.Add(c2);
                    
                }

                return Ok(newlist);
            }
            else
            {
                return null;
            }
        }
    }


}
