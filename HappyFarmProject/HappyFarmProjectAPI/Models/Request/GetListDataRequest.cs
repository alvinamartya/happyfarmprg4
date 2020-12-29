using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class GetListDataRequest
    {
        public int CurrentPage { get; set; }
        public int LimitPage { get; set; }
        public string Search { get; set; }
    }
}