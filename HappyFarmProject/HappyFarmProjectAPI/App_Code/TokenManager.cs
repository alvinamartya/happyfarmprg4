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
        private static string ManagerSecretKey = "828CF3F840FCE64F0499E2875E45D7662D592EF9AAB110CDADEE1AEC1EFA22D0";
        private static string AdminSecretKey = "A6875AAED894AEDA0FABF244CC19B7233471179EF0FFFEA121C36DB5217487F6";
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
                    .Select(x => new {id=x.Id, username = x.Username, Role = x.Role.Name})
                    .FirstOrDefault();

                // get secret key
                string secretKey = "";
                if (user.Role == "Manager")
                {
                    secretKey = ManagerSecretKey;
                }
                else if (user.Role == "Admin")
                {
                    secretKey = AdminSecretKey;
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
                else if (role == "Admin")
                {
                    secretKey = AdminSecretKey;
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

        public static string ValidateToken(string token, string role)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token, role);
            if (principal == null)
                return null;

            ClaimsIdentity identity = null;

            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch(NullReferenceException)
            {
                return null;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }
    }
}