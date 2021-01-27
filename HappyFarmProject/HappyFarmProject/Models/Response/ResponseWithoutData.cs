using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProject.Models
{
    public class ResponseWithoutData
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}