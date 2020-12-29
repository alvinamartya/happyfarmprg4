using HappyFarmProjectAPI.Models;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace HappyFarmProjectAPI
{
    public class TokenManager
    {
        // secret key
        private static string SuperAdminSecretKey = "F3038B6A350494F5C5125B53943A733B1E74DD1C8F7913282A0EE2029E3EABC0";
        private static string ManagerSecretKey = "828CF3F840FCE64F0499E2875E45D7662D592EF9AAB110CDADEE1AEC1EFA22D0";
        private static string MarketingAdminSecretKey = "11BF4ED1A3F499F2806FD4F568C939FE7F1B121CD5B696B92BB83D8763DA4193";
        private static string ProductionAdminSecretKey = "3A3BDE7559AF8C28746DAAC9A2294CA57E917E8F613520FCA82EF2E68B4C3B36";
        private static string CustomerServiceSecretKey = "0A40485B90F688CD5DA9C95A06D12E9D5EEA28DC3EF134F68633B67C3369F5FB";
        private static string SalesAdminSecretKey = "23936AE0C77BD8EB6C32E62954B4383A8ACA5E6410542A54171DFE19778ACC48";
        private static string CustomerSecretKey = "BBAEC56FC52E6871591454EDAD4C3E2B2C42F0FA90BEEDC7F460A9731239D0CE";

        /// <summary>
        /// Generate a token
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string GenerateToken(string username)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get user by username
                var user = db.UserLogins
                    .Where(x => x.Username == username)
                    .Select(x => new { id = x.Id, username = x.Username, Role = x.Role.Name })
                    .FirstOrDefault();

                // get secret key
                string secretKey = "";
                if (user.Role == "Manager")
                {
                    secretKey = ManagerSecretKey;
                }
                else if (user.Role == "Super Admin")
                {
                    secretKey = SuperAdminSecretKey;
                }
                else if (user.Role == "Admin Promosi")
                {
                    secretKey = MarketingAdminSecretKey;
                }
                else if (user.Role == "Admin Produksi")
                {
                    secretKey = ProductionAdminSecretKey;
                }
                else if (user.Role == "Customer Service")
                {
                    secretKey = CustomerServiceSecretKey;
                }
                else if (user.Role == "Admin Penjualan")
                {
                    secretKey = SalesAdminSecretKey;
                }
                else
                {
                    secretKey = CustomerSecretKey;
                }

                // generate jwt key
                byte[] key = Convert.FromBase64String(secretKey);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
                return handler.WriteToken(token);
            }
        }

        public static ClaimsPrincipal GetPrincipal(string token, string role)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;

                // get secret key
                string secretKey = "";
                if (role == "Manager")
                {
                    secretKey = ManagerSecretKey;
                }
                else if (role == "Super Admin")
                {
                    secretKey = SuperAdminSecretKey;
                }
                else if (role == "Admin Promosi")
                {
                    secretKey = MarketingAdminSecretKey;
                }
                else if (role == "Admin Produksi")
                {
                    secretKey = ProductionAdminSecretKey;
                }
                else if (role == "Customer Service")
                {
                    secretKey = CustomerServiceSecretKey;
                }
                else if (role == "Admin Penjualan")
                {
                    secretKey = SalesAdminSecretKey;
                }
                else
                {
                    secretKey = CustomerSecretKey;
                }

                // get principal
                byte[] key = Convert.FromBase64String(secretKey);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public static bool ValidateToken(string token, string role)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token, role);
            if (principal == null)
                return false;

            ClaimsIdentity identity = null;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return false;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username != "" && username != String.Empty;
        }
    }
}