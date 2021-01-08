using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddBannerRequest
    {
        public int? PromoId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public int CreatedBy { get; set; }
    }
}