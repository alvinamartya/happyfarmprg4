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
    public class RegisterController : Controller
    {
        private HttpClient hc = APIHelper.GetHttpClient("Register");
        // GET: Register
        public ActionResult Index()
        {
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                Session["ErrMessage"] = null;
            }

            if (Session["SuccMessage"] != null)
            {
                TempData["SuccMessage"] = Session["SuccMessage"];
                Session["SuccMessage"] = null;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterRequest register)
        {

            if (register.Name == null)
            {
                TempData["ErrMessage"] = "Nama harus diisi";
            }
            else if (register.PhoneNumber == null)
            {
                TempData["ErrMessage"] = "No telepon harus diisi";
            }
            else if (register.Email == null)
            {
                TempData["ErrMessage"] = "Email harus diisi";
            }
            else if (register.Gender == null)
            {
                TempData["ErrMessage"] = "Jenis kelamin harus diisi";
            }
            else if (register.Username == null)
            {
                TempData["ErrMessage"] = "Nama pengguna harus diisi";
            }
            else if (register.Password == null)
            {
                TempData["ErrMessage"] = "Kata sandi harus diisi";
            }
            else if (register.ConfirmPassword == null)
            {
                TempData["ErrMessage"] = "Konfirmasi kata sandi harus diisi";
            }
            else if (register.ConfirmPassword != register.Password)
            {
                TempData["ErrMessage"] = "Konfirmasi kata sandi tidak sama dengan kata sandi";
            }
            else
            {
                var apiRegister = hc.PostAsJsonAsync<RegisterRequest>("Register", register);
                apiRegister.Wait();

                var registerData = apiRegister.Result;
                if (registerData.IsSuccessStatusCode)
                {
                    var registerResponse = registerData.Content.ReadAsAsync<ResponseWithoutData>();
                    registerResponse.Wait();

                    if (registerResponse.Result.StatusCode == HttpStatusCode.OK)
                    {
                        Session["SuccMessage"] = "Pendaftaran berhasil";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrMessage"] = registerResponse.Result.Message;
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