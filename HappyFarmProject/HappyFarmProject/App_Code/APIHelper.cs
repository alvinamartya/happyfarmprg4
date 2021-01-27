using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace HappyFarmProject
{
    public class APIHelper
    {
        private static string BaseUrl = "https://localhost:44301/api/v1";
        public static HttpClient GetHttpClient(string url)
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri(BaseUrl + "/" + url);
            return hc;
        }
    }
}