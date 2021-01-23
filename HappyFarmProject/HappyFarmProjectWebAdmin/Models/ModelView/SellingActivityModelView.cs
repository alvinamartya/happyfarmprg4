using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class SellingActivityModelView
    {
        public int Id { get; set; }
        public int SellingId { get; set; }
        public int SellingStatusid { get; set; }
        public string SellingStatusName { get; set; }
        public int CreatedBy { get; set; }
    }
}