﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class Purchase
    {
        public string RecipientName { get; set; }
        public string RecipientPhone { get; set; }
        public string RecipientAddress { get; set; }
        public string PromoCode { get; set; }
        public int RegionId { get; set; }
        public int CustomerId { get; set; }
        public int SubdistrictId { get; set; }
        public int TotalPurchase { get; set; }
        public int Discount { get; set; }
        public int ShippingCharges { get; set; }
        public List<PurchasingDetailRequest> Products { get; set; }
    }
}