﻿using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class LoginController : Controller
    {
        #region Variable
        private HttpClient hc = APIHelper.GetHttpClient("User/Login");
        #endregion

        #region Action
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

                System.Diagnostics.Debug.WriteLine(apiLogin.Result.StatusCode);
                if (loginData.IsSuccessStatusCode)
                {
                    var loginResponse = loginData.Content.ReadAsAsync<LoginResponse>();
                    loginResponse.Wait();

                    if (loginResponse.Result.StatusCode == HttpStatusCode.OK)
                    {
                        Session["UserId"] = loginResponse.Result.UserId;
                        Session["Token"] = loginResponse.Result.Token.Token;

                        if (loginResponse.Result.Role == "Super Admin")
                        {
                            return RedirectToAction("Index", "SuperAdminEmployee");
                        }
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
        #endregion
    }
}