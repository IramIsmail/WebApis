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
            System.Diagnostics.Debug.WriteLine(id+"jjj9999999999999999999jj");
           
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


        [HttpGet]
        [Route("api/user/getuser")]
        public IHttpActionResult getClient(String id)
        {
            System.Diagnostics.Debug.WriteLine(id + "jjj0000jjjj");

            string uu = id.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();





            if (Convert.ToString(getResponse.JSONContent) == "null")
            {

                System.Diagnostics.Debug.WriteLine("Doesnt'Exists");
                return null;

            }

            else
            {
                System.Diagnostics.Debug.WriteLine("Yes Exists");
                Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse.JSONContent);
                return Ok(c2);
            }
        }




        [HttpPost]
        [Route("api/user/add")]
        public IHttpActionResult add([FromBody]Request request)
        {

            RequestDTO requestDTO = new RequestDTO();



           

          
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(request) + "############");

            string loginId = request.senderId.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Requests").NodePath(loginId).Node(request.destinationId.Replace(".", ","));
            FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Proposals").NodePath(request.destinationId.Replace(".", ",")).Node(loginId);






            requestDTO.status =request.destinationId.Replace(".", ",");

            dynamic foo = new ExpandoObject();
            foo.status = request.status;
          
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);
            FirebaseResponse getResponse = firebaseDBTeams.Get();

            System.Diagnostics.Debug.WriteLine(getResponse.JSONContent);
            if (Convert.ToString(getResponse.JSONContent) == "null")
            {
                System.Diagnostics.Debug.WriteLine("PUT Request");
                FirebaseResponse putResponse = firebaseDBTeams.Post(JsonConvert.SerializeObject(json));

                FirebaseResponse putResponse2 = firebaseDBTeams2.Post(JsonConvert.SerializeObject(json)); 
                System.Diagnostics.Debug.WriteLine(putResponse);
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






            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(request) + "############");

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

            System.Diagnostics.Debug.WriteLine(getResponse.JSONContent);

           
                FirebaseResponse deleteResponse = firebaseDBTeams3.Delete();
                FirebaseResponse deleteResponse2 = firebaseDBTeams4.Delete();
            
            if (Convert.ToString(getResponse.JSONContent) == "null")
            {
                System.Diagnostics.Debug.WriteLine("PUT Request");
                FirebaseResponse putResponse = firebaseDBTeams.Post(JsonConvert.SerializeObject(json));
                firebaseDBTeams2.Post(JsonConvert.SerializeObject(json));
              
                System.Diagnostics.Debug.WriteLine(putResponse);
                return Ok("Respond Successfully");

            }

            else
            {
                System.Diagnostics.Debug.WriteLine("Already present");
                return BadRequest("User Already Registered with this Email Id");
            }
        }
        /* [HttpPost]
         [Route("api/user/add")]
         public IHttpActionResult add(string id,string loginId)
         {
             System.Diagnostics.Debug.WriteLine(id+ "jjjj88jjj"+ loginId);

             Clients c=new Clients(id);

            loginId= loginId.Replace(".", ",");
             FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
             FirebaseDB firebaseDBTeams = firebaseDB.Node("Clients").NodePath(loginId).Node(id);
             string iiid = id;


           dynamic foo = new JObject();
             foo.id= id;

             string json = Newtonsoft.Json.JsonConvert.SerializeObject(foo);



             FirebaseResponse getResponse = firebaseDBTeams.Get();

             System.Diagnostics.Debug.WriteLine(getResponse.JSONContent);
             if (Convert.ToString(getResponse.JSONContent) == "null")
             {               
                 System.Diagnostics.Debug.WriteLine("PUT Request");
                 FirebaseResponse putResponse = firebaseDBTeams.Post(JsonConvert.SerializeObject(c));
                 System.Diagnostics.Debug.WriteLine(putResponse);
                 return Ok("Resgistered Successfully");

             }          

             else
             {
                 System.Diagnostics.Debug.WriteLine("Already present");
                 return BadRequest("User Already Registered with this Email Id");
             }
         }

     */
        [HttpPost]
        [Route("api/user/search")]
        public IHttpActionResult searchuser([FromBody]SearchCriteria c)
        {
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Users");
            FirebaseResponse getResponse = firebaseDBTeams.Get();
            System.Diagnostics.Debug.WriteLine(c.address+ "jjjjjjj"+c.occupation+"88888"+c.gender+"------"+c.age);
            List<Customer> newlist = new List<Customer>();
            System.Diagnostics.Debug.WriteLine(JObject.Parse(getResponse.JSONContent) + "jjjj000jjj");

            var obj = JsonConvert.DeserializeObject<RootObject>(getResponse.JSONContent);
            var mList = JsonConvert.DeserializeObject<IDictionary<string, Customer>>(getResponse.JSONContent);
            System.Diagnostics.Debug.WriteLine(mList.Count + "=mmmm=====current=========");
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
                 System.Diagnostics.Debug.WriteLine(birthDay + "===============" +birthMonth+ "==============="+birthYear);
                double year;
                DateTime birthDate = new DateTime(birthYear, birthMonth, birthDay);
                DateTime currentDate = new DateTime();
                currentDate = DateTime.Now;
                TimeSpan age = new TimeSpan();
                age = currentDate - birthDate;
                year = age.Days / 365;
                System.Diagnostics.Debug.WriteLine(year + "======age waeeans========="+min +"0000"+max);
                if (v.Value.address==c.address && v.Value.profession==c.occupation && v.Value.gender==c.gender && (year>=min && year<=max))
                {
                    newlist.Add(v.Value);
                    System.Diagnostics.Debug.WriteLine(v.Value.email_Id + "===============");
                }
                
            }

            return Ok(newlist);
        }
        [HttpGet]
        [Route("api/user/requests/")]
        public IHttpActionResult requestlist(String id)
        {
            System.Diagnostics.Debug.WriteLine(id+ "yeyeyeyeyeye");
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Requests").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();
           
            if (Convert.ToString(getResponse.JSONContent) != "null")
            {
                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
                System.Diagnostics.Debug.WriteLine(mList + "tuuuuuuuuu8888uuuuuuuuuuuuu");
                List<Customer> newlist = new List<Customer>();

                foreach (var v in mList)
                {
                    FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                    FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                    Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                    newlist.Add(c2);
                    System.Diagnostics.Debug.WriteLine(v.Key + "=========o======");
                    System.Diagnostics.Debug.WriteLine(c2.Id + "=====jj==========");
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
            System.Diagnostics.Debug.WriteLine(id + "yeyeyeyeyeye");
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("Proposals").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
               System.Diagnostics.Debug.WriteLine(mList + "tuuuuuuuuu8888uuuuuuuuuuuuu");
                  List<Customer> newlist = new List<Customer>();

              foreach (var v in mList)
                  {
                FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                newlist.Add(c2);
                System.Diagnostics.Debug.WriteLine(v.Key + "=========o======");
                System.Diagnostics.Debug.WriteLine(c2.Id + "=====jj==========");
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
            System.Diagnostics.Debug.WriteLine(id + "yeyeyeyeyeye");
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("RespondsBy").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
                System.Diagnostics.Debug.WriteLine(mList + "tuuuuuuuuu8888uuuuuuuuuuuuu");
                List<Customer> newlist = new List<Customer>();

                foreach (var v in mList)
                {
                    FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                    FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                    Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                    newlist.Add(c2);
                    System.Diagnostics.Debug.WriteLine(v.Key + "=========o======");
                    System.Diagnostics.Debug.WriteLine(c2.Id + "=====jj==========");
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
            System.Diagnostics.Debug.WriteLine(id + "yeyeyeyeyeye");
            string url = id;
            string uu = url.Replace(".", ",");
            FirebaseDB firebaseDB = new FirebaseDB("https://myshop-b8424.firebaseio.com/");
            FirebaseDB firebaseDBTeams = firebaseDB.Node("RespondsTo").NodePath(uu);

            FirebaseResponse getResponse = firebaseDBTeams.Get();


            if (Convert.ToString(getResponse.JSONContent) != "null")
            {

                var mList = JsonConvert.DeserializeObject<IDictionary<string, Request>>(getResponse.JSONContent);
                System.Diagnostics.Debug.WriteLine(mList + "tuuuuuuuuu8888uuuuuuuuuuuuu");
                List<Customer> newlist = new List<Customer>();

                foreach (var v in mList)
                {
                    FirebaseDB firebaseDBTeams2 = firebaseDB.Node("Users").NodePath(v.Key);

                    FirebaseResponse getResponse2 = firebaseDBTeams2.Get();
                    Customer c2 = JsonConvert.DeserializeObject<Customer>(getResponse2.JSONContent);
                    newlist.Add(c2);
                    System.Diagnostics.Debug.WriteLine(v.Key + "=========o======");
                    System.Diagnostics.Debug.WriteLine(c2.Id + "=====jj==========");
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
