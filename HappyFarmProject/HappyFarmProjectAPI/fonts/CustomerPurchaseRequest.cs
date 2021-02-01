﻿using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.fonts
{
    public class CustomerPurchaseRequest
    {
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public string PromoCode { get; set; }
        public int RegionId { get; set; }
        public int SubdistrictId { get; set; }
        public int TotalPurchase { get; set; }
        public int Discount { get; set; }
        public int DeliveryFee { get; set; }
        public List<CustomerPurchasingDetailRequest> Products { get; set; }
    }
}