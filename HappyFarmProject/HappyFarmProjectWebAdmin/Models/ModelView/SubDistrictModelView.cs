using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class SubDistrictModelView
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public int RegionId { get; set; }
        public string Name { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal ShippingCharges { get; set; }
    }
}