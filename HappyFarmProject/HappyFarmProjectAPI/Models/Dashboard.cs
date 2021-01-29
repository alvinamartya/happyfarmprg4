using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class Dashboard
    {
        public DateTime Date { get; set; }
        public int TotalSale { get; set; }
        public int TotalPurchase { get; set; }
    }
}