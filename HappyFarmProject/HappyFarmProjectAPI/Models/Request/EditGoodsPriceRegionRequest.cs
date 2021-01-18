using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class EditGoodsPriceRegionRequest
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public int RegionId { get; set; }
        public int ModifiedBy { get; set; }
        public decimal Price { get; set; }
    }
}