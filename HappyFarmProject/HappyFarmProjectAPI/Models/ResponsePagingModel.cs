using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class ResponsePagingModel<T>
    {
        public T Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}