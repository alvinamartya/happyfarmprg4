﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddEmployeeRequest
    {
        public string Username { get; set; }
        public int RoleId { get; set; }
        public int? RegionId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int CreatedBy { get; set; }
    }
}