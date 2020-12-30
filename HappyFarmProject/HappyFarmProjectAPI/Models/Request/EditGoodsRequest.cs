using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class EditGoodsRequest
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int ModifiedBy { get; set; }
        public string Description { get; set; }
    }
}