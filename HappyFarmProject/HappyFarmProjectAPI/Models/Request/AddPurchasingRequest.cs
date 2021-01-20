using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddPurchasingRequest
    {
        public int EmployeeId { get; set; }
        public string FarmerName { get; set; }
        public string FarmerPhone { get; set; }
        public string FarmerAddress { get; set; }
        public List<PurchasingDetailRequest> PurchasingDetails { get; set; }
    }
}