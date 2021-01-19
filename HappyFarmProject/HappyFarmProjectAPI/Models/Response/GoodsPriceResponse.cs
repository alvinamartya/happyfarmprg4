using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class GoodsPriceResponse
    {
        public int RegionId { get; set; }
        public string Region { get; set; }
        public decimal? Price { get; set; }
    }
}