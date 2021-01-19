using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class GoodsPriceModelView
    {
        public int RegionId { get; set; }
        public string Region { get; set; }
        public decimal? Price { get; set; }
    }
}