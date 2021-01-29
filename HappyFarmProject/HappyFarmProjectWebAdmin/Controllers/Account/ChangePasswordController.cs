using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Net;
using System.Web.Mvc;
using PagedList;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class ChangePasswordController : Controller
    {
        #region Variable
        HttpClient hcChangePassword = APIHelper.GetHttpClient(APIHelper.User + "/ChangePassword");
        #endregion

        #region Edit Region
        [Route("~/User/GantiPassword")]
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["ErrMessage"] != null && Session["ErrHeader"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                Session["ErrMessage"] = null;

                TempData["ErrHeader"] = Session["ErrHeader"];
                Session["ErrHeader"] = null;
            }
            return View();
        }

        [Route("~/User/GantiPassword")]
        [HttpPost]
        public ActionResult Edit(ChangePassword changePassword)
        {
            ChangePasswordRequest request = new ChangePasswordRequest()
            {
                ConfirmPassword = changePassword.ConfirmPassword,
                NewPassword = changePassword.NewPassword,
                OldPassword = changePassword.OldPassword,
                UserId = (int)Session["UserId"]
            };

            var apiChangePassword = hcChangePassword.PostAsJsonAsync<ChangePasswordRequest>("ChangePassword", request);
            apiChangePassword.Wait();

            var changePasswordData = apiChangePassword.Result;
            if (changePasswordData.IsSuccessStatusCode)
            {
                var changePasswordResponse = changePasswordData.Content.ReadAsAsync<LoginResponse>();
                changePasswordResponse.Wait();

                if (changePasswordResponse.Result.StatusCode == HttpStatusCode.OK)
                {
                    Session["ErrHeader"] = "Berhasil";
                    Session["ErrMessage"] = "Berhasil mengganti kata sandi";
                    return RedirectToAction("Edit", "ChangePassword");
                }
                else
                {
                    TempData["ErrHeader"] = "Gagal";
                    TempData["ErrMessage"] = changePasswordResponse.Result.Message;
                }
            }
            else
            {
                TempData["ErrHeader"] = "Gagal";
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem, silahkan hubungi admin.";
            }

            return View(changePassword);
        }
        #endregion
    }
}