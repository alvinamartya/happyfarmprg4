using HappyFarmProjectAPI.Controllers.BusinessLogic;
using HappyFarmProjectAPI.Controllers.Repository;
using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HappyFarmProjectAPI.Controllers
{
    public class UserController : ApiController
    {
        #region Variable
        private UserLoginLogic userLoginLogic = new UserLoginLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private ChangePasswordRepository repo = new ChangePasswordRepository();
        #endregion

        #region Action
        [Route("api/v1/Register")]
        [HttpPost]
        public IHttpActionResult Register(RegisterRequest registerRequest)
        {
            try
            {
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    var emailAlreadyExists = db.Customers.Where(x => x.Email == registerRequest.Email).FirstOrDefault() != null;

                    if (registerRequest.ConfirmPassword != registerRequest.Password)
                    {
                        var confirmPasswordResponse = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Konfirmasi password tidak sama dengan password"
                        };
                        return Ok(confirmPasswordResponse);
                    }
                    else
                    {
                        if (emailAlreadyExists)
                        {
                            var responseEmail = new ResponseWithoutData()
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Message = "Email sudah tersedia"
                            };
                            return Ok(responseEmail);
                        }
                        else
                        {
                            var usernameAlreadyExists = db.UserLogins.Where(x => x.Username == registerRequest.Username).FirstOrDefault() != null;
                            if (usernameAlreadyExists)
                            {
                                var responseEmail = new ResponseWithoutData()
                                {
                                    StatusCode = HttpStatusCode.BadRequest,
                                    Message = "Nama pengguna sudah tersedia"
                                };
                                return Ok(responseEmail);
                            }
                            else
                            {
                                UserLogin login = new UserLogin()
                                {
                                    RoleId = db.Roles.Where(x => x.Name == "Customer").FirstOrDefault().Id,
                                    Username = registerRequest.Username,
                                    Password = Helper.EncryptStringSha256Hash(registerRequest.Password)
                                };

                                db.UserLogins.Add(login);
                                db.SaveChanges();

                                int lastUserLoginId = db.UserLogins.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                                Customer customer = new Customer()
                                {
                                    UserLoginId = lastUserLoginId,
                                    Name = registerRequest.Name,
                                    PhoneNumber = registerRequest.PhoneNumber,
                                    Email = registerRequest.Email,
                                    Gender = registerRequest.Gender
                                };

                                db.Customers.Add(customer);
                                db.SaveChanges();

                                var response = new ResponseWithoutData()
                                {
                                    Message = "Pendaftaran berhasil",
                                    StatusCode = HttpStatusCode.OK
                                };
                                return Ok(response);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

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

                            if (!isCustomer)
                            {
                                var employee = db.Employees.Where(x => x.UserLoginId == user.Id).FirstOrDefault();
                                if (employee.RowStatus == "D")
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
                                UserId = user.Role.Name == "Customer" ?
                                    db.Customers.Where(x => x.UserLoginId == user.Id).FirstOrDefault().Id :
                                    db.Employees.Where(x => x.UserLoginId == user.Id).FirstOrDefault().Id,
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

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/User/Employee/ForgotPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPasswordEmployee(ForgotPasswordRequest forgotRequest)
        {
            try
            {
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    // get employee by email
                    var user = db.Employees.Where(x => x.Email == forgotRequest.Email).FirstOrDefault();
                    if (user != null)
                    {
                        string newPassword = Helper.RandomPassword();
                        string ecryptPassword = Helper.EncryptStringSha256Hash(newPassword);
                        user.UserLogin.Password = ecryptPassword;
                        db.SaveChanges();

                        await Task.Run(() => Helper.SendMailAsync(user.Email, "Ganti Kata Sandi",
                            "<div>" +
                            "Hai <span style = \"font-weight: bold;\">" + user.Name + "</span>," +
                            "<br>" +
                            "Kata sandi anda telah berhasil diganti, login ke aplikasi <span style=\"font-weight: bold;\">HappyFarm</span> segera dengan menggunakan akun anda:" +
                            "<br><br>" +
                            "<div>Nama Pengguna : " + user.UserLogin.Username + "<br>Kata Sandi : " + newPassword + "</div><br>" +
                            "<div>Segala bentuk informasi seperti nomor kontak, alamat e-mail, atau password kamu bersifat rahasia. Jangan " +
                            "menginformasikan data - data tersebut kepada siapapun, termasuk kepada pihak yang mengatasnamakan perusahaan.</div>" +
                            "</div>"));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil mengubah kata sandi"
                        };

                        return Ok(response);
                    }
                    else
                    {
                        ResponseWithoutData userNotAvailableResponse = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Email tidak tersedia"
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

        /// <summary>
        /// Change Password
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/User/Employee/ChangePassword/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> ChangePasswordEmployee(int id, ChangePasswordRequest changeRequest)
        {
            try
            {
                ResponseModel responseModel = userLoginLogic.EditUserLogin(id, changeRequest);
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // update region
                    await Task.Run(() => repo.ChangePassword(id, changeRequest));

                    // response success
                    var response = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil mengubah password"
                    };

                    return Ok(response);
                }
                else
                {
                    // bad request
                    var badRequestResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = responseModel.Message
                    };

                    return Ok(badRequestResponse);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// employee account
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/User/Employee/{id}")]
        [HttpGet]
        public IHttpActionResult GetEmployeeAccount(int id)
        {
            try
            {
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    var employee = db.Employees.Where(x => x.Id == id)
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.PhoneNumber,
                            x.Email,
                            x.Address,
                            x.Gender,
                            Role = x.UserLogin.Role.Name
                        })
                        .FirstOrDefault();
                    if (employee != null)
                    {
                        // validate token
                        string role = employee.Role;
                        if (tokenLogic.ValidateTokenInHeader(Request, role))
                        {
                            // response success
                            var response = new ResponseWithData<Object>()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Message = "Berhasil",
                                Data = employee
                            };

                            return Ok(response);
                        }
                        else
                        {
                            // unauthorized
                            var unAuthorizedResponse = new ResponseWithoutData()
                            {
                                StatusCode = HttpStatusCode.Unauthorized,
                                Message = "Anda tidak memiliki hak akses"
                            };

                            return Ok(unAuthorizedResponse);
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
        #endregion
    }
}
