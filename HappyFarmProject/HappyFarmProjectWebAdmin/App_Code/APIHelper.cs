using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace HappyFarmProjectWebAdmin
{
    public class APIHelper
    {
        private static string BaseUrl = "https://localhost:44301/api/v1";
        public static string SA = "SA";
        public static string Manager = "Manager";
        public static string MA = "MA";
        public static string PA = "PA";
        public static string CS = "CS";
        public static HttpClient GetHttpClient(string url)
        {
            HttpClient hc = new HttpClient();
            hc.BaseAddress = new Uri(BaseUrl + "/" + url);
            return hc;
        }
    }
}