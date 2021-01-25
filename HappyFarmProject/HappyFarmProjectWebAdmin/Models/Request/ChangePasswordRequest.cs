using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class ChangePasswordRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}