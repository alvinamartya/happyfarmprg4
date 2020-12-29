using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace HappyFarmProjectAPI.Models
{
    public class ResponseLogin
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public AuthModel Token { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}