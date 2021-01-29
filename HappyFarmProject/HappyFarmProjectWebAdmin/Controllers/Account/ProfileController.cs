using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class ProfileController : Controller
    {
        #region Variable
        HttpClient hcProfile = APIHelper.GetHttpClient(APIHelper.User + "/EditProfileEmployee");
        #endregion
        #region Action
        // GET: Profile
        [Route("~/User/Profil")]
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["ErrMessage"] != null && Session["ErrHeader"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                Session["ErrMessage"] = null;

                TempData["ErrHeader"] = Session["ErrHeader"];
                Session["ErrHeader"] = null;
            }
            EmployeeProfileModelView employee = null;

            // get employee
            HttpClient hcEmployeeGet = APIHelper.GetHttpClient(APIHelper.User + "/Profile");

            var apiGet = hcEmployeeGet.GetAsync("Profile/" + (int)Session["UserId"]);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<EmployeeProfileModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data karyawan";
                }
                else
                {
                    employee = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            return View(employee);
        }

        [Route("~/User/Profil")]
        [HttpPost]
        public ActionResult Index(EmployeeProfileModelView employeeProfile)
        {

            var apiEditProfile = hcProfile.PostAsJsonAsync<EmployeeProfileModelView>("EditProfileEmployee", employeeProfile);
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
                    return RedirectToAction("Index", "Profile");
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
            return View(employeeProfile);
        }
        #endregion
    }
}