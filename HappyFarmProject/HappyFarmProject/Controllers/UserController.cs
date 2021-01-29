using HappyFarmProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProject.Controllers
{
    public class UserController : Controller
    {
        #region Variable
        HttpClient hcChangePassword = APIHelper.GetHttpClient("ChangePassword");
        HttpClient hcProfile = APIHelper.GetHttpClient("EditProfile");
        #endregion

        #region Change Password
        // GET: User
        [Route("~/GantiPassword")]
        public ActionResult ChangePassword()
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

        [Route("~/GantiPassword")]
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword changePassword)
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
                    return RedirectToAction("ChangePassword", "User");
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

        [Route("~/Profil")]
        public ActionResult UpdateProfile()
        {
            if (Session["ErrMessage"] != null && Session["ErrHeader"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                Session["ErrMessage"] = null;

                TempData["ErrHeader"] = Session["ErrHeader"];
                Session["ErrHeader"] = null;
            }
            Profile profile = null;

            // get employee
            HttpClient hcProfileGet = APIHelper.GetHttpClient("Profile");

            var apiGet = hcProfileGet.GetAsync("Profile/" + (int)Session["UserId"]);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<Profile>>();
                displayData.Wait();

                if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data karyawan";
                }
                else
                {
                    profile = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }
            return View(profile);
        }

        [Route("~/Profil")]
        [HttpPost]
        public ActionResult UpdateProfile(Profile profile)
        {
            var apiEditProfile = hcProfile.PostAsJsonAsync<Profile>("EditProfile", profile);
            apiEditProfile.Wait();

            var editProfileData = apiEditProfile.Result;
            if (editProfileData.IsSuccessStatusCode)
            {
                var editProfileResponse = editProfileData.Content.ReadAsAsync<LoginResponse>();
                editProfileResponse.Wait();

                if (editProfileResponse.Result.StatusCode == HttpStatusCode.OK)
                {
                    Session["ErrHeader"] = "Berhasil";
                    Session["ErrMessage"] = "Berhasil menyimpan perubahan";
                    return RedirectToAction("UpdateProfile", "User");
                }
                else
                {
                    TempData["ErrHeader"] = "Gagal";
                    TempData["ErrMessage"] = editProfileResponse.Result.Message;
                }
            }
            else
            {
                TempData["ErrHeader"] = "Gagal";
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem, silahkan hubungi admin.";
            }
            return View(profile);
        }
    }
}