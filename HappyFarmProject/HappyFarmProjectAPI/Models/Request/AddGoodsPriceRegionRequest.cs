﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddGoodsPriceRegionRequest
    {
        public int GoodsId { get; set; }
        public int RegionId { get; set; }
        public int CreatedBy { get; set; }
        public long Price { get; set; }
    }
}