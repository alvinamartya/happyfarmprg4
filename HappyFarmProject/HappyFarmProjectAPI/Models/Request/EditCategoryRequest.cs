using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class EditCategoryRequest
    {
        public string Name { get; set; }
        public int ModifiedBy { get; set; }
    }
}