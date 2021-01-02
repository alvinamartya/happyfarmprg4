using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class AddCategoryRequest
    {
        public string Name { get; set; }
        public int CreatedBy { get; set; }
    }
}