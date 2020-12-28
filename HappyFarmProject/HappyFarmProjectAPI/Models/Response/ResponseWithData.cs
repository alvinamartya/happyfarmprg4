using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Models
{
    public class ResponseWithData<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}