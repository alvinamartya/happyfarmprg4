using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AuthModel
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}