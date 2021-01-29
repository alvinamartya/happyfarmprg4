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
    public class ProfileController : ApiController
    {
        #region Variable
        private ProfileRepository repo = new ProfileRepository();
        #endregion

        [Route("api/v1/EditProfile")]
        [HttpPost]
        public async Task<IHttpActionResult> EditProfileEmployee(ProfileRequest request)
        {
            try
            {
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    var emailAlreadyExists = db.Customers.Where(x => x.Email == request.Email && x.Id != request.Id).FirstOrDefault() != null;
                    if (emailAlreadyExists)
                    {
                        var responseEmailExists = new ResponseWithoutData()
                        {
                            Message = "Email sudah tersedia",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                        return Ok(responseEmailExists);
                    }
                    else
                    {
                        await Task.Run(() => repo.EditCustomer(request));
                        var response = new ResponseWithoutData()
                        {
                            Message = "Berhasil",
                            StatusCode = HttpStatusCode.OK
                        };

                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [Route("api/v1/Profile/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProfileCustomer(int id)
        {
            try
            {
                Object customer = await Task.Run(() => repo.GetProfileById(id));
                var response = new ResponseWithData<Object>()
                {
                    Data = customer,
                    Message = "Berhasil",
                    StatusCode = HttpStatusCode.OK
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }


        [Route("api/v1/ForgotPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordRequest forgotRequest)
        {
            try
            {
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    // get employee by email
                    var user = db.Customers.Where(x => x.Email == forgotRequest.Email).FirstOrDefault();
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
    }
}
