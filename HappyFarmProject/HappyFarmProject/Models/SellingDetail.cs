using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyFarmProject.Models
{
    public class SellingDetail
    {
        public int Id { get; set; }
        public int GoodsId { get; set; }
        public string GoodsName { get; set; }
        public int Qty { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public decimal GoodsPrice { get; set; }
    }
}