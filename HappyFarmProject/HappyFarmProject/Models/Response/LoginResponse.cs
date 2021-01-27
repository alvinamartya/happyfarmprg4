using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProject.Models
{
    public class LoginResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public TokenModel Token { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}