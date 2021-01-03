using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}