using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class BannerModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PromoId { get; set; }
        public string PromoName { get; set; }
    }
}