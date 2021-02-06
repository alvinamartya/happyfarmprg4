using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class HistorySellingModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal TotalSalePrice { get; set; }
        public decimal ShippingCharges { get; set; }
        public string LastSellingActivity { get; set; }
    }
}