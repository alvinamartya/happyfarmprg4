using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class EditSellingActivityRequest
    {
        public int SellingStatusid { get; set; }
        public int CreatedBy { get; set; }
    }
}