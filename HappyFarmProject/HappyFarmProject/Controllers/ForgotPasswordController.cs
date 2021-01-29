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
    public class ForgotPasswordController : Controller
    {
        #region Variable
        private HttpClient hc = APIHelper.GetHttpClient("ForgotPassword");
        #endregion

        // GET: ForgotPassword
        public ActionResult Index()
        {
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                Session["ErrMessage"] = null;
            }

            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            if (forgotPassword.Email == null)
            {
                TempData["ErrMessage"] = "Email belum diisi";
            }
            else
            {
                var apiForgotPassword = hc.PostAsJsonAsync<ForgotPassword>("ForgotPassword", forgotPassword);
                apiForgotPassword.Wait();

                var forgotPasswordData = apiForgotPassword.Result;
                System.Diagnostics.Debug.Write(forgotPasswordData.StatusCode);
                if (forgotPasswordData.IsSuccessStatusCode)
                {
                    var forgotPasswordResponse = forgotPasswordData.Content.ReadAsAsync<LoginResponse>();
                    forgotPasswordResponse.Wait();

                    if (forgotPasswordResponse.Result.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        TempData["ErrMessage"] = forgotPasswordResponse.Result.Message;
                    }
                }
                else
                {
                    TempData["ErrMessage"] = "Terjadi kesalahan pada sistem, silahkan hubungi admin.";
                }
            }
            return View("Index");
        }
    }
}