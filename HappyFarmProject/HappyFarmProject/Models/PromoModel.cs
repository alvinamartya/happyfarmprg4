using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class PromoModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Code { get; set; }
        public decimal MinTransaction { get; set; }
        public decimal MaxDicount { get; set; }
        public string IsFreeDelivery { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}