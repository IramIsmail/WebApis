using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApis.Models
{
    public class ImageModel
    {
        public string title { set; get; }
        public string image { set; get; }
        public string document { set; get; }

        public ImageModel()
        {

        }
        public ImageModel(string title, string image,string document)
        {
            this.title = title;
            this.image = image;
            this.document = document;
        }
    }
}