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
    public class ChangePasswordController : ApiController
    {
        private ProfileRepository repo = new ProfileRepository();

        [Route("api/v1/ChangePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePasswordEmployee(ChangePasswordRequest changeRequest)
        {
            try
            {
                string oldPassword = await Task.Run(() => repo.GetOldPassword(changeRequest.UserId));
                if (changeRequest.OldPassword == "")
                {
                    var responseEmptyOldPassword = new ResponseWithoutData()
                    {
                        Message = "Kata sandi lama harus diisi",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return Ok(responseEmptyOldPassword);
                }
                else if (Helper.EncryptStringSha256Hash(changeRequest.OldPassword) != oldPassword)
                {
                    var responseOldPassword = new ResponseWithoutData()
                    {
                        Message = "Kata sandi lama tidak valid",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return Ok(responseOldPassword);
                }
                else if (changeRequest.NewPassword == "")
                {
                    var responseNewPassword = new ResponseWithoutData()
                    {
                        Message = "Kata sandi baru harus diisi",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return Ok(responseNewPassword);
                }
                else if (changeRequest.ConfirmPassword == "")
                {
                    var responseConfirmPassword = new ResponseWithoutData()
                    {
                        Message = "Konfirmasi kata sandi harus diisi",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return Ok(responseConfirmPassword);
                }
                else if (changeRequest.NewPassword != changeRequest.ConfirmPassword)
                {
                    var responseConfirmPassword = new ResponseWithoutData()
                    {
                        Message = "Konfirmasi kata sandi tidak sama dengan kata sandi baru",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return Ok(responseConfirmPassword);
                }
                else
                {
                    await Task.Run(() => repo.ChangePassword(changeRequest));
                    var response = new ResponseWithoutData()
                    {
                        Message = "Berhasil mengubah kata sandi",
                        StatusCode = HttpStatusCode.OK
                    };
                    return Ok(response);
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
