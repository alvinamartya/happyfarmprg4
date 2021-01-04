using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class TokenLogic
    {
        /// <summary>
        /// Validate Token In Header
        /// </summary>
        /// <param name="request"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool ValidateTokenInHeader(HttpRequestMessage request, string role)
        {
            if (request.Headers.Contains("Authorization"))
            {
                string token = request.Headers.GetValues("Authorization").First().Split(' ')[1];
                System.Diagnostics.Debug.WriteLine(token);
                if (TokenManager.ValidateToken(token, role))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}