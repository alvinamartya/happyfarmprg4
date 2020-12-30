using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class EditBannerRequest
    {
        public int? PromoId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int ModifiedBy { get; set; }
    }
}