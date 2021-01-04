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
        HttpClient hcEmployeeGet = APIHelper.GetHttpClient(APIHelper.SA + "/Employee");
        HttpClient hcEmployeeAdd = APIHelper.GetHttpClient(APIHelper.SA + "/Employee/Add");
        HttpClient hcRole = APIHelper.GetHttpClient(APIHelper.SA + "/Role");
        HttpClient hcRegion = APIHelper.GetHttpClient(APIHelper.SA + "/Region");
        #endregion

        #region GetEmployees
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

            hcEmployeeGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcEmployeeGet.PostAsJsonAsync<GetListDataRequest>("Employee", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<EmployeeModelView>>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode == HttpStatusCode.OK)
                {
                    employees = displayData.Result.Data;
                    ViewBag.CurrentPage = displayData.Result.CurrentPage;
                    ViewBag.TotalPage = displayData.Result.TotalPage;

                    System.Diagnostics.Debug.WriteLine(Json(displayData.Result).ToString());

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
        #endregion

        #region Add Employee
        // Add Employee
        [Route("~/SA/Karyawan/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            // get Role
            hcRole.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetRole = hcRole.GetAsync("Role");
            apiGetRole.Wait();

            var dataRole = apiGetRole.Result;
            if (dataRole.IsSuccessStatusCode)
            {
                var displayDataRole = dataRole.Content.ReadAsAsync<ResponseWithData<List<RoleModelView>>>();
                if (displayDataRole.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataRole.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ViewBag.Roles = new SelectList(displayDataRole.Result.Data, "Id", "Name");
                }
            }

            // get region
            hcRegion.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetRegion = hcRegion.GetAsync("Region");
            apiGetRegion.Wait();

            var dataRegion = apiGetRegion.Result;
            if (dataRegion.IsSuccessStatusCode)
            {
                var displayDataRegion = dataRegion.Content.ReadAsAsync<ResponseWithData<List<RegionModelView>>>();
                if (displayDataRegion.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataRegion.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ViewBag.Regions = new SelectList(displayDataRegion.Result.Data, "Id", "Name");
                }
            }

            return View();
        }

        [Route("~/SA/Karyawan/Tambah")]
        [HttpPost]
        public ActionResult Add(AddEmployeeRequest employeeRequest)
        {
            // get Role
            hcRole.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetRole = hcRole.GetAsync("Role");
            apiGetRole.Wait();

            var dataRole = apiGetRole.Result;
            if (dataRole.IsSuccessStatusCode)
            {
                var displayDataRole = dataRole.Content.ReadAsAsync<ResponseWithData<List<RoleModelView>>>();
                if (displayDataRole.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataRole.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ViewBag.Roles = new SelectList(displayDataRole.Result.Data, "Id", "Name");
                }
            }

            // get region
            hcRegion.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetRegion = hcRegion.GetAsync("Region");
            apiGetRegion.Wait();

            var dataRegion = apiGetRegion.Result;
            if (dataRegion.IsSuccessStatusCode)
            {
                var displayDataRegion = dataRegion.Content.ReadAsAsync<ResponseWithData<List<RegionModelView>>>();
                if (displayDataRegion.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataRegion.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ViewBag.Regions = new SelectList(displayDataRegion.Result.Data, "Id", "Name");
                }
            }

            // add created by
            employeeRequest.CreatedBy = (int)Session["UserId"];

            // insert data using API
            hcEmployeeAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiSave = hcEmployeeAdd.PostAsJsonAsync<AddEmployeeRequest>("Add", employeeRequest);
            apiSave.Wait();

            var dataSave = apiSave.Result;
            if(dataSave.IsSuccessStatusCode)
            {
                var displayDataSave = dataSave.Content.ReadAsAsync<ResponseWithoutData>();
                if (displayDataSave.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataSave.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayDataSave.Result.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrMessage"] = displayDataSave.Result.Message;
                    TempData["ErrHeader"] = "Gagal Menambah Karyawan";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Menambah Karyawan";
            }
            return View();
        }
        #endregion
    }
}