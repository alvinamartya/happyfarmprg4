using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddSellingActivityRequest
    {
        public int SellingStatusid { get; set; }
        public string SellingId { get; set; }
        public int CreatedBy { get; set; }
    }
}