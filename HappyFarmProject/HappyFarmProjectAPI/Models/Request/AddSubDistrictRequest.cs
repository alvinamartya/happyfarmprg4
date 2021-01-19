﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddSubDistrictRequest
    {
        public int RegionId { get; set; }
        public string Name { get; set; }
        public long ShippingCharges { get; set; }
        public int CreatedBy { get; set; }
    }
}