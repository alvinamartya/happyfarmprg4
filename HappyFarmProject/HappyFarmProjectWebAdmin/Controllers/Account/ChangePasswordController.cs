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
        HttpClient hcUserLoginEdit = APIHelper.GetHttpClient(APIHelper.User + "/ChangePassword");
        #endregion

        #region Edit Region
        [Route("~/User/GantiPassword/{id}")]
        [HttpGet]
        public ActionResult Edit()
        {
            UserLoginModelView user = null;

            // get userlogin
            HttpClient hcAccountGet = APIHelper.GetHttpClient(APIHelper.User + "/UserLogin");
            hcAccountGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcAccountGet.GetAsync("UserLogin/" + (int)Session["UserId"]);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<UserLoginModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data wilayah";
                }
                else
                {
                    user = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            ChangePasswordRequest editUser = new ChangePasswordRequest()
            {
                Password = user.Password,
                Username = user.Username
            };

            return View(editUser);
        }

        [Route("~/User/GantiPassword/{id}")]
        [HttpPost]
        public ActionResult Edit(ChangePasswordRequest changePasswordRequest)
        {
            UserLoginModelView user = null;

            // get userlogin
            HttpClient hcAccountGet = APIHelper.GetHttpClient(APIHelper.User + "/UserLogin");
            hcAccountGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcAccountGet.GetAsync("UserLogin/" + (int)Session["UserId"]);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<UserLoginModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data wilayah";
                }
                else
                {
                    user = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            ChangePasswordRequest editUser = new ChangePasswordRequest()
            {
                Username = user.Username,
                Password = user.Password
            };

            // update region
            hcUserLoginEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcUserLoginEdit.PutAsJsonAsync<ChangePasswordRequest>("Edit/" + (int)Session["UserId"], changePasswordRequest);
            apiEdit.Wait();

            var dateEdit = apiEdit.Result;
            if (dateEdit.IsSuccessStatusCode)
            {
                var displayDataEdit = dateEdit.Content.ReadAsAsync<ResponseWithoutData>();
                displayDataEdit.Wait();
                if (displayDataEdit.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataEdit.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayDataEdit.Result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrMessage"] = displayDataEdit.Result.Message;
                    TempData["ErrHeader"] = "Gagal Mengubah Data Wilayah";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Wilayah";
            }

            return View(editUser);
        }
        #endregion

        #region Request Data
        public ResponseDataWithPaging<List<UserLoginModelView>> GetAccounts(GetListDataRequest dataPaging)
        {
            // get categories
            HttpClient hcAccountGet = APIHelper.GetHttpClient(APIHelper.User + "/UserLogin");
            hcAccountGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcAccountGet.PostAsJsonAsync<GetListDataRequest>("UserLogin", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<UserLoginModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<UserLoginModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<UserLoginModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<UserLoginModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<UserLoginModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}