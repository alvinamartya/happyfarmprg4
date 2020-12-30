using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddPromoRequest
    {
        public int CreatedBy { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string IsFreeDelivery { get; set; }
        public float Discount { get; set; }
        public int MinTransaction { get; set; }
        public int MaxDiscount { get; set; }
    }
}