using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class PurchasingDetailRequest
    {
        public int GoodsId { get; set; }
        public int Qty { get; set; }
        public long Price { get; set; }
    }
}