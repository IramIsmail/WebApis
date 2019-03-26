using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebApis.Firebase
{
    public class FirebaseDB
    {
        private string RootNode { get; set; }
      //  private string Node { get; set; }
       // private string NodePath { get; set; }
        public FirebaseDB(string baseURL) {
            this.RootNode = baseURL;
        }

        public FirebaseDB Node(string node) {
            if (node.Contains("/")) {
                throw new FormatException("Node must not contain /");
            }
            return new FirebaseDB(this.RootNode + "/" + node);
        }

        public FirebaseDB NodePath(string nodePath)
        {
            return new FirebaseDB(this.RootNode + '/' + nodePath);
        }
        //get request
        public FirebaseResponse Get()
        {
            return new FirebaseRequest(HttpMethod.Get, this.RootNode).Execute();
        }
        //put request
        public FirebaseResponse Put(string jsonData)
        {
            return new FirebaseRequest(HttpMethod.Put, this.RootNode, jsonData).Execute();
        }

        //post request
        public FirebaseResponse Post(string jsonData)
        {
            return new FirebaseRequest(HttpMethod.Post, this.RootNode, jsonData).Execute();
        }

        //patch request
        public FirebaseResponse Patch(string jsonData)
        {
            return new FirebaseRequest(new HttpMethod("PATCH"), this.RootNode, jsonData).Execute();
        }

        //delete request
        public FirebaseResponse Delete()
        {
            return new FirebaseRequest(HttpMethod.Delete, this.RootNode).Execute();
        }




    }
}