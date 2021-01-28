using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class DeletePurchasingRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
    }
}