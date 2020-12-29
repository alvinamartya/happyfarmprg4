using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace HappyFarmProjectAPI.Models
{
    public class ResponseWithoutData
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}