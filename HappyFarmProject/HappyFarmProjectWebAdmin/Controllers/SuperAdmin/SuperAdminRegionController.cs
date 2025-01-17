﻿using HappyFarmProjectWebAdmin.Models;
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
    public class SuperAdminRegionController : Controller
    {
        #region Variable
        HttpClient hcRegionAdd = APIHelper.GetHttpClient(APIHelper.SA + "/Region/Add");
        HttpClient hcRegionDelete = APIHelper.GetHttpClient(APIHelper.SA + "/Region/Delete");
        HttpClient hcRegionEdit = APIHelper.GetHttpClient(APIHelper.SA + "/Region/Edit");
        #endregion

        #region GetRegions
        // GET Regions
        [Route("~/SA/Wilayah")]
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


            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            ResponseDataWithPaging<List<RegionModelView>> regionsRequest = GetRegions(dataPaging);
            ViewBag.CurrentPage = regionsRequest.CurrentPage;
            ViewBag.TotalPage = regionsRequest.TotalPage;

            // status code
            if (regionsRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = regionsRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (regionsRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = regionsRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (regionsRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "Name_Desc":
                    regionsRequest.Data = regionsRequest.Data.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Name_Asc":
                    regionsRequest.Data = regionsRequest.Data.OrderBy(x => x.Name).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<RegionModelView>> indexViewModel = new IndexModelView<IPagedList<RegionModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = regionsRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }

        [Route("~/SA/Wilayah")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<RegionModelView>> indexRegion, string Sorting_Order, int? Page_No)
        {
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

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = indexRegion.DataPaging.Search
            };

            ResponseDataWithPaging<List<RegionModelView>> regionsRequest = GetRegions(dataPaging);
            ViewBag.CurrentPage = regionsRequest.CurrentPage;
            ViewBag.TotalPage = regionsRequest.TotalPage;

            // status code
            if (regionsRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = regionsRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (regionsRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = regionsRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (regionsRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "Name_Desc":
                    regionsRequest.Data = regionsRequest.Data.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Name_Asc":
                    regionsRequest.Data = regionsRequest.Data.OrderBy(x => x.Name).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<RegionModelView>> indexViewModel = new IndexModelView<IPagedList<RegionModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = regionsRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }
        #endregion

        #region Add Region
        // Add Region
        [Route("~/SA/Wilayah/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Route("~/SA/Wilayah/Tambah")]
        [HttpPost]
        public ActionResult Add(AddRegionRequest regionRequest)
        {
            // add created by
            regionRequest.CreatedBy = (int)Session["UserId"];

            // insert data using API
            hcRegionAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiSave = hcRegionAdd.PostAsJsonAsync<AddRegionRequest>("Add", regionRequest);
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
                    TempData["ErrHeader"] = "Gagal Menambah Wilayah";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Menambah Wilayah";
            }
            return View();
        }
        #endregion

        #region Delete Region
        [Route("~/SA/Wilayah/Hapus")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            hcRegionDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcRegionDelete.DeleteAsync("Delete/" + id);
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
                    Session["ErrMessage"] = "Berhasil menghapus wilayah";
                    Session["ErrHeader"] = "Berhasil";
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal menghapus wilayah";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal menghapus wilayah";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit Region
        [Route("~/SA/Wilayah/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            RegionModelView region = null;

            // get region
            HttpClient hcRegionGet = APIHelper.GetHttpClient(APIHelper.SA + "/Region");
            hcRegionGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcRegionGet.GetAsync("Region/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<RegionModelView>>();
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
                    region = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditRegionRequest editRegion = new EditRegionRequest()
            {
                ModifiedBy = (int)Session["UserId"],
                Name = region.Name
            };

            return View(editRegion);
        }

        [Route("~/SA/Wilayah/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, EditRegionRequest regionRequest)
        {
            RegionModelView region = null;

            // get region
            HttpClient hcRegionGet = APIHelper.GetHttpClient(APIHelper.SA + "/Region");
            hcRegionGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcRegionGet.GetAsync("Region/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<RegionModelView>>();
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
                    region = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditRegionRequest editRegion = new EditRegionRequest()
            {
                Name = region.Name
            };

            // add modified by
            regionRequest.ModifiedBy = (int)Session["UserId"];

            // update region
            hcRegionEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcRegionEdit.PutAsJsonAsync<EditRegionRequest>("Edit/" + id, regionRequest);
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

            return View(editRegion);
        }
        #endregion

        #region Request Data
        public ResponseDataWithPaging<List<RegionModelView>> GetRegions(GetListDataRequest dataPaging)
        {
            // get categories
            HttpClient hcRegionGet = APIHelper.GetHttpClient(APIHelper.SA + "/Region");
            hcRegionGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcRegionGet.PostAsJsonAsync<GetListDataRequest>("Region", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<RegionModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<RegionModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<RegionModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<RegionModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<RegionModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}