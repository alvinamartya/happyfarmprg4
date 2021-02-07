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
    public class SalesAdminSellingActivityController : Controller
    {
        #region Variable
        HttpClient hcSellingActivityAdd = APIHelper.GetHttpClient(APIHelper.SALA + "/SellingActivity/Add");
        HttpClient hcSellingActivityEdit = APIHelper.GetHttpClient(APIHelper.SALA + "/SellingActivity/Edit");
        #endregion

        #region Get Selling Status
        // GET Selling status
        [Route("~/SALA/StatusPenjualan")]
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

            // sorting
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "Name_Desc" ? "Name_Asc" : "Name_Desc";
            ViewBag.SortingOrderId = Sorting_Order == "OrderId_Desc" ? "OrderId_Asc" : "OrderId_Desc";
            ViewBag.SortingDate = Sorting_Order == "Date_Desc" ? "Date_Asc" : "Date_Desc";

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            ResponseDataWithPaging<List<SellingActivityModelView>> sellingActivityRequest = GetSellingActivity(dataPaging);
            ViewBag.CurrentPage = sellingActivityRequest.CurrentPage;
            ViewBag.TotalPage = sellingActivityRequest.TotalPage;

            // status code
            if (sellingActivityRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = sellingActivityRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (sellingActivityRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = sellingActivityRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (sellingActivityRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "Name_Desc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderByDescending(x => x.SellingStatusName).ToList();
                    break;
                case "Name_Asc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderBy(x => x.SellingStatusName).ToList();
                    break;
                case "OrderId_Desc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderByDescending(x => x.SellingId).ToList();
                    break;
                case "OrderId_Asc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderBy(x => x.SellingId).ToList();
                    break;
                case "Date_Desc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderByDescending(x => x.CreatedAt).ToList();
                    break;
                case "Date_Asc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderBy(x => x.CreatedAt).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<SellingActivityModelView>> indexViewModel = new IndexModelView<IPagedList<SellingActivityModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = sellingActivityRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }

        [Route("~/SALA/StatusPenjualan")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<SellingActivityModelView>> indexSellingActivity, string Sorting_Order, int? Page_No)
        {
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // sorting
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "Name_Desc" ? "Name_Asc" : "Name_Desc";

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = indexSellingActivity.DataPaging.Search
            };

            ResponseDataWithPaging<List<SellingActivityModelView>> sellingActivityRequest = GetSellingActivity(dataPaging);
            ViewBag.CurrentPage = sellingActivityRequest.CurrentPage;
            ViewBag.TotalPage = sellingActivityRequest.TotalPage;

            // status code
            if (sellingActivityRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = sellingActivityRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (sellingActivityRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = sellingActivityRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (sellingActivityRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "Name_Desc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderByDescending(x => x.SellingStatusName).ToList();
                    break;
                case "Name_Asc":
                    sellingActivityRequest.Data = sellingActivityRequest.Data.OrderBy(x => x.SellingStatusName).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<SellingActivityModelView>> indexViewModel = new IndexModelView<IPagedList<SellingActivityModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = sellingActivityRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }
        #endregion

        #region Add Selling Activity
        // Add Selling Activity
        [Route("~/SALA/StatusPenjualan/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            // get selling status
            ResponseWithData<List<SellingStatusModelView>> sellingStatusRequest = GetSellingStatus();
            if (sellingStatusRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.SellingStatus = new SelectList(sellingStatusRequest.Data, "Id", "Name");
            }
            else if (sellingStatusRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = sellingStatusRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = sellingStatusRequest.Message;
                TempData["ErrHeader"] = "Gagal meload status";
            }
            return View();
        }

        [Route("~/SALA/StatusPenjualan/Tambah")]
        [HttpPost]
        public ActionResult Add(AddSellingActivityRequest sellingActivityRequest)
        {
            // get selling status
            ResponseWithData<List<SellingStatusModelView>> sellingStatusRequest = GetSellingStatus();
            if (sellingStatusRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.SellingStatus = new SelectList(sellingStatusRequest.Data, "Id", "Name");
            }
            else if (sellingStatusRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = sellingStatusRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = sellingStatusRequest.Message;
                TempData["ErrHeader"] = "Gagal meload status";
            }

            // add created by
            sellingActivityRequest.CreatedBy = (int)Session["UserId"];

            // insert data using API
            hcSellingActivityAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiSave = hcSellingActivityAdd.PostAsJsonAsync<AddSellingActivityRequest>("Add", sellingActivityRequest);
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
                    TempData["ErrHeader"] = "Gagal Menambah Status";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Order ID Tidak Ditemukan";
                TempData["ErrHeader"] = "Gagal Menambah Status";
            }
            return View();
        }
        #endregion

        #region Edit Status Penjualan
        [Route("~/SALA/StatusPenjualan/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            SellingActivityModelView sellingActivity = null;

            // get selling status
            ResponseWithData<List<SellingStatusModelView>> sellingStatusRequest = GetSellingStatus();
            if (sellingStatusRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.SellingStatus = new SelectList(sellingStatusRequest.Data, "Id", "Name");
            }
            else if (sellingStatusRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = sellingStatusRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = sellingStatusRequest.Message;
                TempData["ErrHeader"] = "Gagal meload status";
            }

            // get selling activity
            HttpClient hcSellingActivityGet = APIHelper.GetHttpClient(APIHelper.SALA + "/SellingActivity");
            hcSellingActivityGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcSellingActivityGet.GetAsync("SellingActivity/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<SellingActivityModelView>>();
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
                    sellingActivity = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditSellingActivityRequest editSellingActivity = new EditSellingActivityRequest()
            {
                SellingStatusid = sellingActivity.SellingStatusid,
                CreatedBy = (int)Session["UserId"]
            };

            return View(editSellingActivity);
        }

        [Route("~/SALA/StatusPenjualan/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, EditSellingActivityRequest sellingActivityRequest)
        {
            SellingActivityModelView sellingActivity = null;

            // get selling status
            ResponseWithData<List<SellingStatusModelView>> sellingStatusRequest = GetSellingStatus();
            if (sellingStatusRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.SellingStatus = new SelectList(sellingStatusRequest.Data, "Id", "Name");
            }
            else if (sellingStatusRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = sellingStatusRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = sellingStatusRequest.Message;
                TempData["ErrHeader"] = "Gagal meload status";
            }

            // get selling activity
            HttpClient hcSellingActivityGet = APIHelper.GetHttpClient(APIHelper.SALA + "/SellingActivity");
            hcSellingActivityGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcSellingActivityGet.GetAsync("SellingActivity/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<SellingActivityModelView>>();
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
                    sellingActivity = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditSellingActivityRequest editSellingActivity = new EditSellingActivityRequest()
            {
                SellingStatusid = sellingActivity.SellingStatusid
            };

            // add created by
            sellingActivityRequest.CreatedBy = (int)Session["UserId"];

            // update selling activity
            hcSellingActivityEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcSellingActivityEdit.PutAsJsonAsync<EditSellingActivityRequest>("Edit/" + id, sellingActivityRequest);
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
                    TempData["ErrHeader"] = "Gagal Mengubah Data Status Penjualan";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Status Penjualan";
            }

            return View(editSellingActivity);
        }
        #endregion

        #region SellingDetails
        [Route("~/SALA/DetailTransaksi")]
        [HttpGet]
        public ActionResult SellingDetails(int id)
        {
            // error
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // default request paging
            ResponseWithData<List<SellingDetailModelView>> sellingRequest = GetSellingDetail(id);

            // status code
            if (sellingRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = sellingRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            sellingRequest.Data = sellingRequest.Data.OrderBy(x => x.GoodsName).ToList();

            // data is empty
            if (sellingRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            return View(sellingRequest.Data);
        }
        #endregion

        #region Request Data
        public ResponseWithData<List<SellingStatusModelView>> GetSellingStatus()
        {
            // get regions
            HttpClient hcSellingStatus = APIHelper.GetHttpClient(APIHelper.SALA + "/SellingStatus");
            hcSellingStatus.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetSellingStatus = hcSellingStatus.GetAsync("SellingStatus");
            apiGetSellingStatus.Wait();

            var dataSellingStatus = apiGetSellingStatus.Result;
            if (dataSellingStatus.IsSuccessStatusCode)
            {
                var displaydataSellingStatus = dataSellingStatus.Content.ReadAsAsync<ResponseWithData<List<SellingStatusModelView>>>();
                displaydataSellingStatus.Wait();
                return new ResponseWithData<List<SellingStatusModelView>>()
                {
                    StatusCode = displaydataSellingStatus.Result.StatusCode,
                    Message = displaydataSellingStatus.Result.Message,
                    Data = displaydataSellingStatus.Result.StatusCode == HttpStatusCode.OK ? displaydataSellingStatus.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<SellingStatusModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = null
                };
            }
        }
        public ResponseDataWithPaging<List<SellingActivityModelView>> GetSellingActivity(GetListDataRequest dataPaging)
        {
            // get selling activity
            HttpClient hcSellingActivityGet = APIHelper.GetHttpClient(APIHelper.SALA + "/SellingActivity");
            hcSellingActivityGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcSellingActivityGet.PostAsJsonAsync<GetListDataRequest>("SellingActivity", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<SellingActivityModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<SellingActivityModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<SellingActivityModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<SellingActivityModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<SellingActivityModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }

        public ResponseWithData<List<SellingDetailModelView>> GetSellingDetail(int id)
        {
            // get categories
            HttpClient hcCategoryGet = APIHelper.GetHttpClient(APIHelper.SALA + "/SellingDetails");

            var apiGet = hcCategoryGet.GetAsync("SellingDetail/" + id.ToString());
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<List<SellingDetailModelView>>>();
                displayData.Wait();

                return new ResponseWithData<List<SellingDetailModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<SellingDetailModelView>()
                };
            }
            else
            {
                return new ResponseWithData<List<SellingDetailModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<SellingDetailModelView>()
                };
            }
        }
        #endregion
    }
}