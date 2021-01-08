using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddGoodsRequest
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
    }
}