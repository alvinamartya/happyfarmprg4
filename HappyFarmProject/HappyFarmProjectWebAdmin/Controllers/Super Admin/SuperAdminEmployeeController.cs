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
        HttpClient hcEmployeeAdd = APIHelper.GetHttpClient(APIHelper.SA + "/Employee/Add");
        HttpClient hcEmployeeDelete = APIHelper.GetHttpClient(APIHelper.SA + "/Employee/Delete");
        HttpClient hcEmployeeEdit = APIHelper.GetHttpClient(APIHelper.SA + "/Employee/Edit");
        #endregion

        #region GetEmployees
        // GET Employees
        [Route("~/SA/Karyawan")]
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            ResponseDataWithPaging<List<EmployeeModelView>> employeesRequest = GetEmployees(dataPaging);
            ViewBag.CurrentPage = employeesRequest.CurrentPage;
            ViewBag.TotalPage = employeesRequest.TotalPage;

            // status code
            if (employeesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = employeesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (employeesRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = employeesRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }


            // data is empty
            if (employeesRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<EmployeeModelView>> indexViewModel = new IndexModelView<IEnumerable<EmployeeModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = employeesRequest.Data
            };

            return View(indexViewModel);
        }

        [Route("~/SA/Karyawan")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<EmployeeModelView>> indexEmployee)
        {
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = indexEmployee.DataPaging.Search
            };

            ResponseDataWithPaging<List<EmployeeModelView>> employeesRequest = GetEmployees(dataPaging);
            ViewBag.CurrentPage = employeesRequest.CurrentPage;
            ViewBag.TotalPage = employeesRequest.TotalPage;

            // status code
            if (employeesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = employeesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (employeesRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = employeesRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (employeesRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<EmployeeModelView>> indexViewModel = new IndexModelView<IEnumerable<EmployeeModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = employeesRequest.Data
            };

            return View(indexViewModel);
        }
        #endregion
        #region Add Employee
        // Add Employee
        [Route("~/SA/Karyawan/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            // get roles
            ResponseWithData<List<RoleModelView>> roleRequest = GetRoles();
            if (roleRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Roles = new SelectList(roleRequest.Data, "Id", "Name");
            }
            else if (roleRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = roleRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = roleRequest.Message;
                TempData["ErrHeader"] = "Gagal meload hak akses";
            }

            // get region
            ResponseWithData<List<RegionModelView>> regionRequest = GetRegions();
            if (regionRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Regions = new SelectList(regionRequest.Data, "Id", "Name");
            }
            else if (regionRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = regionRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = regionRequest.Message;
                TempData["ErrHeader"] = "Gagal meload wilayah";
            }

            return View();
        }

        [Route("~/SA/Karyawan/Tambah")]
        [HttpPost]
        public ActionResult Add(AddEmployeeRequest employeeRequest)
        {
            // get roles
            ResponseWithData<List<RoleModelView>> roleRequest = GetRoles();
            if (roleRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Roles = new SelectList(roleRequest.Data, "Id", "Name");
            }
            else if (roleRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = roleRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = roleRequest.Message;
                TempData["ErrHeader"] = "Gagal meload hak akses";
            }

            // get region
            ResponseWithData<List<RegionModelView>> regionRequest = GetRegions();
            if (regionRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Regions = new SelectList(regionRequest.Data, "Id", "Name");
            }
            else if (regionRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = regionRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = regionRequest.Message;
                TempData["ErrHeader"] = "Gagal meload wilayah";
            }

            // add created by
            employeeRequest.CreatedBy = (int)Session["UserId"];

            // insert data using API
            hcEmployeeAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiSave = hcEmployeeAdd.PostAsJsonAsync<AddEmployeeRequest>("Add", employeeRequest);
            apiSave.Wait();

            var dataSave = apiSave.Result;
            if (dataSave.IsSuccessStatusCode)
            {
                var displayDataSave = dataSave.Content.ReadAsAsync<ResponseWithoutData>();
                displayDataSave.Wait();
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
        #region Delete Employee
        [Route("~/SA/Karyawan/Hapus")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            hcEmployeeDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcEmployeeDelete.DeleteAsync("Delete/" + id);
            apiDelete.Wait();

            var dateDelete = apiDelete.Result;
            if (dateDelete.IsSuccessStatusCode)
            {
                var displayDataDelete = dateDelete.Content.ReadAsAsync<ResponseWithoutData>();
                displayDataDelete.Wait();
                if (displayDataDelete.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayDataDelete.Result.StatusCode == HttpStatusCode.OK)
                {
                    Session["ErrMessage"] = "Berhasil menghapus karyawan";
                    Session["ErrHeader"] = "Berhasil";
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal menghapus Karyawan";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal menghapus Karyawan";
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Edit Employee
        [Route("~/SA/Karyawan/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EmployeeModelView employee = null;

            // get region
            ResponseWithData<List<RegionModelView>> regionRequest = GetRegions();
            if (regionRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Regions = new SelectList(regionRequest.Data, "Id", "Name");
            }
            else if (regionRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = regionRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = regionRequest.Message;
                TempData["ErrHeader"] = "Gagal meload wilayah";
            }

            // get employee
            HttpClient hcEmployeeGet = APIHelper.GetHttpClient(APIHelper.SA + "/Employee");
            hcEmployeeGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcEmployeeGet.GetAsync("Employee/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<EmployeeModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
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

            EditEmployeeRequest editEmployee = new EditEmployeeRequest()
            {
                Address = employee.Address,
                Email = employee.Email,
                Gender = employee.Gender,
                ModifiedBy = (int)Session["UserId"],
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                RegionId = employee.RegionId
            };

            return View(editEmployee);
        }

        [Route("~/SA/Karyawan/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, EditEmployeeRequest employeeRequest)
        {
            EmployeeModelView employee = null;

            // get region
            ResponseWithData<List<RegionModelView>> regionRequest = GetRegions();
            if (regionRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Regions = new SelectList(regionRequest.Data, "Id", "Name");
            }
            else if (regionRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = regionRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = regionRequest.Message;
                TempData["ErrHeader"] = "Gagal meload wilayah";
            }

            // get employee
            HttpClient hcEmployeeGet = APIHelper.GetHttpClient(APIHelper.SA + "/Employee");
            hcEmployeeGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcEmployeeGet.GetAsync("Employee/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<EmployeeModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
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

            EditEmployeeRequest editEmployee = new EditEmployeeRequest()
            {
                Address = employee.Address,
                Email = employee.Email,
                Gender = employee.Gender,
                ModifiedBy = (int)Session["UserId"],
                Name = employee.Name,
                PhoneNumber = employee.PhoneNumber,
                RegionId = employee.RegionId
            };

            // add modified by
            employeeRequest.ModifiedBy = (int)Session["UserId"];

            // update employee
            hcEmployeeEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcEmployeeEdit.PutAsJsonAsync<EditEmployeeRequest>("Edit/" + id, employeeRequest);
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
                    TempData["ErrHeader"] = "Gagal Mengubah Data Karyawan";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Karyawan";
            }

            return View(editEmployee);
        }
        #endregion

        #region Request Data
        public ResponseWithData<List<RoleModelView>> GetRoles()
        {
            // get Role
            HttpClient hcRole = APIHelper.GetHttpClient(APIHelper.SA + "/Role");
            hcRole.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetRole = hcRole.GetAsync("Role");
            apiGetRole.Wait();

            var dataRole = apiGetRole.Result;
            if (dataRole.IsSuccessStatusCode)
            {
                var displayDataRole = dataRole.Content.ReadAsAsync<ResponseWithData<List<RoleModelView>>>();
                displayDataRole.Wait();
                return new ResponseWithData<List<RoleModelView>>()
                {
                    StatusCode = displayDataRole.Result.StatusCode,
                    Message = displayDataRole.Result.Message,
                    Data = displayDataRole.Result.StatusCode == HttpStatusCode.OK ? displayDataRole.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<RoleModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = null
                };
            }
        }

        public ResponseWithData<List<RegionModelView>> GetRegions()
        {
            // get regions
            HttpClient hcRegion = APIHelper.GetHttpClient(APIHelper.SA + "/Region");
            hcRegion.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetRegion = hcRegion.GetAsync("Region");
            apiGetRegion.Wait();

            var dataRegion = apiGetRegion.Result;
            if (dataRegion.IsSuccessStatusCode)
            {
                var displayDataRegion = dataRegion.Content.ReadAsAsync<ResponseWithData<List<RegionModelView>>>();
                displayDataRegion.Wait();
                return new ResponseWithData<List<RegionModelView>>()
                {
                    StatusCode = displayDataRegion.Result.StatusCode,
                    Message = displayDataRegion.Result.Message,
                    Data = displayDataRegion.Result.StatusCode == HttpStatusCode.OK ? displayDataRegion.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<RegionModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = null
                };
            }
        }

        public ResponseDataWithPaging<List<EmployeeModelView>> GetEmployees(GetListDataRequest dataPaging)
        {
            // get employees
            HttpClient hcEmployeeGet = APIHelper.GetHttpClient(APIHelper.SA + "/Employee");
            hcEmployeeGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcEmployeeGet.PostAsJsonAsync<GetListDataRequest>("Employee", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<EmployeeModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<EmployeeModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<EmployeeModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<EmployeeModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<EmployeeModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}