using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class HistoryTransaction
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal TotalSalePrice { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal ShippingCharges { get; set; }
        public string LastSellingActivity { get; set; }
    }
}