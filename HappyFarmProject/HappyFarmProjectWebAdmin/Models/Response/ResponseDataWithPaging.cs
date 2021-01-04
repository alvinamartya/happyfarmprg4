using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectWebAdmin.Models
{
    public class ResponseDataWithPaging<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}