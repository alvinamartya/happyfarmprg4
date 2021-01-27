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
    public class LoginController : Controller
    {

        private HttpClient hc = APIHelper.GetHttpClient("User/Login");

        // GET: Login
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
        public ActionResult Login(LoginRequest login)
        {
            if (login.Username == null)
            {
                TempData["ErrMessage"] = "Nama pengguna belum diisi";
            }
            else if (login.Password == null)
            {
                TempData["ErrMessage"] = "Kata sandi belum diisi";
            }
            else
            {
                var apiLogin = hc.PostAsJsonAsync<LoginRequest>("Login", login);
                apiLogin.Wait();

                var loginData = apiLogin.Result;

                if (loginData.IsSuccessStatusCode)
                {
                    var loginResponse = loginData.Content.ReadAsAsync<LoginResponse>();
                    loginResponse.Wait();

                    if (loginResponse.Result.StatusCode == HttpStatusCode.OK)
                    {
                        Session["UserId"] = loginResponse.Result.UserId;
                        Session["Token"] = loginResponse.Result.Token.Token;

                        return RedirectToAction("Index", "Promo");
                    }
                    else
                    {
                        TempData["ErrMessage"] = loginResponse.Result.Message;
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