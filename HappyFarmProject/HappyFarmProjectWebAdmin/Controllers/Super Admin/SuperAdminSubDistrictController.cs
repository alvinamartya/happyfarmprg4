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

namespace HappyFarmProjectWebAdmin.Controllers.Super_Admin
{
    public class SuperAdminSubDistrictController : Controller
    {
        #region Variable
        HttpClient hcSubDistrictAdd = APIHelper.GetHttpClient(APIHelper.SA + "/SubDistrict/Add");
        HttpClient hcSubDistrictDelete = APIHelper.GetHttpClient(APIHelper.SA + "/SubDistrict/Delete");
        HttpClient hcSubDistrictEdit = APIHelper.GetHttpClient(APIHelper.SA + "/SubDistrict/Edit");
        #endregion

        #region GetSubDistricts
        // GET SubDistricts
        [Route("~/SA/Kecamatan")]
        [HttpGet]
        public ActionResult Index(string Sorting_Order, int? Page_No)
        {
            // error
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // sorting state
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "Name_Desc" ? "Name_Asc" : "Name_Desc";
            ViewBag.SortingRegion = Sorting_Order == "Region_Desc" ? "Region_Asc" : "Region_Desc";
            ViewBag.SortingShippingCharges = Sorting_Order == "ShippingCharges_Desc" ? "ShippingCharges_Asc" : "ShippingCharges_Desc";

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            ResponseDataWithPaging<List<SubDistrictModelView>> subDistrictRequest = GetSubDistricts(dataPaging);
            ViewBag.CurrentPage = subDistrictRequest.CurrentPage;
            ViewBag.TotalPage = subDistrictRequest.TotalPage;

            // status code
            if (subDistrictRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = subDistrictRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (subDistrictRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = subDistrictRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (subDistrictRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch(Sorting_Order)
            {
                case "Name_Desc":
                    subDistrictRequest.Data = subDistrictRequest.Data.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Name_Asc":
                    subDistrictRequest.Data = subDistrictRequest.Data.OrderBy(x => x.Name).ToList();
                    break;
                case "Region_Desc":
                    subDistrictRequest.Data = subDistrictRequest.Data.OrderByDescending(x => x.Region).ToList();
                    break;
                case "Region_Asc":
                    subDistrictRequest.Data = subDistrictRequest.Data.OrderBy(x => x.Region).ToList();
                    break;
                case "ShippingCharges_Desc":
                    subDistrictRequest.Data = subDistrictRequest.Data.OrderByDescending(x => x.ShippingCharges).ToList();
                    break;
                case "ShippingCharges_Asc":
                    subDistrictRequest.Data = subDistrictRequest.Data.OrderBy(x => x.ShippingCharges).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<SubDistrictModelView>> indexViewModel = new IndexModelView<IPagedList<SubDistrictModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = subDistrictRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }

        [Route("~/SA/Kecamatan")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<SubDistrictModelView>> indexSubDistrict)
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
                Search = indexSubDistrict.DataPaging.Search
            };

            ResponseDataWithPaging<List<SubDistrictModelView>> subDistrictRequest = GetSubDistricts(dataPaging);
            ViewBag.CurrentPage = subDistrictRequest.CurrentPage;
            ViewBag.TotalPage = subDistrictRequest.TotalPage;

            // status code
            if (subDistrictRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = subDistrictRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (subDistrictRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = subDistrictRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (subDistrictRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<SubDistrictModelView>> indexViewModel = new IndexModelView<IEnumerable<SubDistrictModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = subDistrictRequest.Data
            };

            return View(indexViewModel);
        }
        #endregion

        #region Add SubDistrict
        // Add SubDistrict
        [Route("~/SA/Kecamatan/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {

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

        [Route("~/SA/Kecamatan/Tambah")]
        [HttpPost]
        public ActionResult Add(AddSubDistrictRequest subDistrictRequest)
        {

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
            subDistrictRequest.CreatedBy = (int)Session["UserId"];

            // insert data using API
            hcSubDistrictAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiSave = hcSubDistrictAdd.PostAsJsonAsync<AddSubDistrictRequest>("Add", subDistrictRequest);
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
                    TempData["ErrHeader"] = "Gagal Menambah Kecamatan";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Menambah Kecamatan";
            }
            return View();
        }
        #endregion

        #region Delete SubDistrict
        [Route("~/SA/Kecamatan/Hapus")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            hcSubDistrictDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcSubDistrictDelete.DeleteAsync("Delete/" + id);
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
                    Session["ErrMessage"] = "Berhasil menghapus kecamatan";
                    Session["ErrHeader"] = "Berhasil";
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal menghapus kecamatan";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal menghapus kecamatan";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit SubDistrict
        [Route("~/SA/Kecamatan/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            SubDistrictModelView subDistrict = null;

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

            // get subDistrict
            HttpClient hcSubDistrictGet = APIHelper.GetHttpClient(APIHelper.SA + "/SubDistrict");
            hcSubDistrictGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcSubDistrictGet.GetAsync("SubDistrict/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<SubDistrictModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data kecamatan";
                }
                else
                {
                    subDistrict = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditSubDistrictRequest editSubDistrict = new EditSubDistrictRequest()
            {

                ModifiedBy = (int)Session["UserId"],
                Name = subDistrict.Name,
                ShippingCharges = subDistrict.ShippingCharges,
                RegionId = subDistrict.RegionId
            };

            return View(editSubDistrict);
        }

        [Route("~/SA/Kecamatan/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, EditSubDistrictRequest subDistrictRequest)
        {
            SubDistrictModelView subDistrict = null;

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

            // get subdistrict
            HttpClient hcSubDistrictGet = APIHelper.GetHttpClient(APIHelper.SA + "/SubDistrict");
            hcSubDistrictGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcSubDistrictGet.GetAsync("SubDistrict/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<SubDistrictModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data kecamatan";
                }
                else
                {
                    subDistrict = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditSubDistrictRequest editSubDistrict = new EditSubDistrictRequest()
            {
                ModifiedBy = (int)Session["UserId"],
                Name = subDistrict.Name,
                ShippingCharges = subDistrict.ShippingCharges,
                RegionId = subDistrict.RegionId
            };

            // add modified by
            subDistrictRequest.ModifiedBy = (int)Session["UserId"];

            // update subdistrict
            hcSubDistrictEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcSubDistrictEdit.PutAsJsonAsync<EditSubDistrictRequest>("Edit/" + id, subDistrictRequest);
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
                    TempData["ErrHeader"] = "Gagal Mengubah Data Kecamatan";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Kecamatan";
            }

            return View(editSubDistrict);
        }
        #endregion

        #region Request Data

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

        public ResponseDataWithPaging<List<SubDistrictModelView>> GetSubDistricts(GetListDataRequest dataPaging)
        {
            // get subdistricts
            HttpClient hcSubDistrictGet = APIHelper.GetHttpClient(APIHelper.SA + "/SubDistrict");
            hcSubDistrictGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcSubDistrictGet.PostAsJsonAsync<GetListDataRequest>("SubDistrict", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<SubDistrictModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<SubDistrictModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<SubDistrictModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<SubDistrictModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<SubDistrictModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}