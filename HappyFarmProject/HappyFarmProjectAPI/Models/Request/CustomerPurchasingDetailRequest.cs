using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class CustomerPurchasingDetailRequest
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int Qty { get; set; }
        public long Price { get; set; }
    }
}