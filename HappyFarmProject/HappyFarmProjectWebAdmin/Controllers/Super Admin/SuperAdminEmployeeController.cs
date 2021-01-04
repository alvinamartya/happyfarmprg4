using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Net;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class SuperAdminEmployeeController : Controller
    {
        #region Variable
        HttpClient hc = APIHelper.GetHttpClient(APIHelper.SA + "/Employee");
        #endregion

        // GET Employees
        [Route("~/SA/Karyawan")]
        [HttpGet]
        public ActionResult Index()
        {
            List<EmployeeModelView> employees = null;

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            hc.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hc.PostAsJsonAsync<GetListDataRequest>("Employee", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<EmployeeModelView>>>();
                displayData.Wait();

                if(displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if(displayData.Result.StatusCode == HttpStatusCode.OK)
                {
                    employees = displayData.Result.Data;
                    ViewBag.CurrentPage = displayData.Result.CurrentPage;
                    ViewBag.TotalPage = displayData.Result.TotalPage;

                    if (employees == null || employees.Count == 0)
                    {
                        TempData["ErrMessage"] = "Data belum tersedia";
                    }
                }
                else
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                }
            }
            else
            {
                Session["ErrMessage"] = "Gagal login ke aplikasi";
                return RedirectToAction("Index", "Login");
            }

            return View(employees);
        }

        // Add Employees
        [Route("~/SA/Karyawan/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
    }
}