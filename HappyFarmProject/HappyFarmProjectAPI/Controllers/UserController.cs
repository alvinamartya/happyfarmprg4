using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HappyFarmProjectAPI.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// Login To Application
        /// </summary>
        /// <param name="userLogin">Username and Passsword</param>
        /// <returns></returns>
        [Route("api/v1/User/Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginRequest loginRequest)
        {
            try
            {
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    UserLogin user = db.UserLogins.Where(x => x.Username == loginRequest.Username).FirstOrDefault();

                    // validate user must be available
                    if (user != null)
                    {
                        // validate user password must be same with user password in database
                        if (Helper.EncryptStringSha256Hash(loginRequest.Password) != user.Password)
                        {
                            ResponseWithoutData passwordIsNotValidResponse = new ResponseWithoutData()
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Message = "Kata sandi tidak valid"
                            };
                            return Ok(passwordIsNotValidResponse);
                        }
                        else
                        {
                            var isCustomer = db.Customers.Where(x => x.UserLoginId == user.Id).FirstOrDefault() != null;

                            if(!isCustomer)
                            {
                                var employee = db.Employees.Where(x => x.UserLoginId == user.Id).FirstOrDefault();
                                if(employee.RowStatus == "D")
                                {
                                    ResponseWithoutData deactiveAccountResponse = new ResponseWithoutData()
                                    {
                                        StatusCode = HttpStatusCode.Unauthorized,
                                        Message = "Akun anda tidak dapat login ke aplikasi"
                                    };
                                    return Ok(deactiveAccountResponse);
                                }
                            }

                            ResponseLogin response = new ResponseLogin()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Message = "Login Berhasil",
                                Role = user.Role.Name,
                                UserId = user.Id,
                                Token = new AuthModel()
                                {
                                    Token = TokenManager.GenerateToken(loginRequest.Username),
                                    Expires = DateTime.UtcNow.AddDays(7)
                                }
                            };

                            return Ok(response);
                        }
                    }
                    else
                    {
                        ResponseWithoutData userNotAvailableResponse = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Akun tidak tersedia"
                        };
                        return Ok(userNotAvailableResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }
    }
}
