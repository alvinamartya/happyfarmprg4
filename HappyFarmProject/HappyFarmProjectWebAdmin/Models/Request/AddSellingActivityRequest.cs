using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class AddSellingActivityRequest
    {
        public string SellingId { get; set; }
        public int SellingStatusid { get; set; }
        public int CreatedBy { get; set; }
    }
}