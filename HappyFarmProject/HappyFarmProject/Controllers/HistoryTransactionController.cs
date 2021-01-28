using HappyFarmProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProject.Controllers
{
    public class HistoryTransactionController : Controller
    {

        HttpClient hcUploadBuktiTransfer = APIHelper.GetHttpClient("UploadBuktiTransfer");
        HttpClient hcFeedback = APIHelper.GetHttpClient("Feedback");
        #region Index

        [Route("~/RiwayatTransaksi")]
        public ActionResult Index(string Sorting_Order, int? Page_No, string Search)
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
            ViewBag.SortingId = Sorting_Order == "Id_Desc" ? "Id_Asc" : "Id_Desc";
            ViewBag.SortingTanggal = Sorting_Order == "Tanggal_Desc" ? "Tanggal_Asc" : "Tanggal_Desc";
            ViewBag.SortingShippingCharges = Sorting_Order == "ShippingCharges_Desc" ? "ShippingCharges_Asc" : "ShippingCharges_Desc";
            ViewBag.SortingTotalSalePrice = Sorting_Order == "TotalSalePrice_Desc" ? "TotalSalePrice_Asc" : "TotalSalePrice_Desc";
            ViewBag.SortingSellingActivity = Sorting_Order == "SortingSellingActivity_Desc" ? "SortingSellingActivity_Asc" : "SortingSellingActivity_Desc";

            // default request paging
            ResponseWithData<List<HistoryTransaction>> historyRequest = GetHistoryTransaction();

            // status code
            if (historyRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = historyRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (historyRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "Id_Desc":
                    historyRequest.Data = historyRequest.Data.OrderByDescending(x => x.Id).ToList();
                    break;
                case "Id_Asc":
                    historyRequest.Data = historyRequest.Data.OrderBy(x => x.Id).ToList();
                    break;
                case "Tanggal_Desc":
                    historyRequest.Data = historyRequest.Data.OrderByDescending(x => x.DateTime).ToList();
                    break;
                case "Tanggal_Asc":
                    historyRequest.Data = historyRequest.Data.OrderBy(x => x.DateTime).ToList();
                    break;
                case "ShippingCharges_Desc":
                    historyRequest.Data = historyRequest.Data.OrderByDescending(x => x.ShippingCharges).ToList();
                    break;
                case "ShippingCharges_Asc":
                    historyRequest.Data = historyRequest.Data.OrderBy(x => x.ShippingCharges).ToList();
                    break;
                case "TotalSalePrice_Desc":
                    historyRequest.Data = historyRequest.Data.OrderByDescending(x => x.TotalSalePrice).ToList();
                    break;
                case "TotalSalePrice_Asc":
                    historyRequest.Data = historyRequest.Data.OrderBy(x => x.TotalSalePrice).ToList();
                    break;
                case "SortingSellingActivity_Desc":
                    historyRequest.Data = historyRequest.Data.OrderByDescending(x => x.LastSellingActivity).ToList();
                    break;
                case "SortingSellingActivity_Asc":
                    historyRequest.Data = historyRequest.Data.OrderBy(x => x.LastSellingActivity).ToList();
                    break;
            }

            if (Search != null)
            {
                if (Search.Contains("ORD"))
                {
                    string temp = Search;
                    try
                    {
                        Search = int.Parse(Search.Replace("ORD", "")).ToString();
                        Search = temp;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        Search = temp;
                    }
                }
                historyRequest.Data = historyRequest.Data
                    .Where(x =>
                    x.Id.ToString().Contains(Search) ||
                    x.DateTime.ToString().Contains(Search) ||
                    x.TotalSalePrice.ToString().Contains(Search) ||
                    x.ShippingCharges.ToString().Contains(Search)).ToList();
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);

            return View(historyRequest.Data.ToPagedList(noOfPage, sizeOfPage));
        }
        #endregion
        #region Upload Bukti Transfer
        [Route("~/BuktiTransfer")]
        [HttpGet]
        public ActionResult UploadBuktiTransfer(int id)
        {
            UploadBuktiTransfer uploadBuktiTransfer = new UploadBuktiTransfer();
            uploadBuktiTransfer.Id = "ORD" + id.ToString().PadLeft(4, '0');
            return View(uploadBuktiTransfer);
        }

        [HttpPost]
        public ActionResult UploadBuktiTransfer(UploadBuktiTransfer uploadBuktiTransfer)
        {
            if (uploadBuktiTransfer.HiddenFileName == null && uploadBuktiTransfer.Image != null)
            {
                // decrypt file name
                Guid uid = Guid.NewGuid();
                var guidFileName = uid.ToString() + Path.GetExtension(uploadBuktiTransfer.Image.FileName);

                // save to server
                var filePath = Server.MapPath("~/Images/" + guidFileName);
                uploadBuktiTransfer.Image.SaveAs(filePath);

                // set to hidden file
                uploadBuktiTransfer.HiddenFileName = guidFileName;
                uploadBuktiTransfer.OriginalFileName = uploadBuktiTransfer.Image.FileName;
            }

            if (uploadBuktiTransfer.HiddenFileName != null)
            {
                UploadBuktiTransferRequest request = new UploadBuktiTransferRequest();
                // set path from hidden file
                request.ImagePath = uploadBuktiTransfer.HiddenFileName;
                request.Id = int.Parse(uploadBuktiTransfer.Id.Replace("ORD", ""));
                request.CreatedBy = (int)Session["UserId"];

                // insert data using API
                var apiSave = hcUploadBuktiTransfer.PostAsJsonAsync<UploadBuktiTransferRequest>("UploadBuktiTransfer", request);
                apiSave.Wait();

                var dataSave = apiSave.Result;
                if (dataSave.IsSuccessStatusCode)
                {
                    var displayDataSave = dataSave.Content.ReadAsAsync<ResponseWithoutData>();
                    displayDataSave.Wait();
                    if (displayDataSave.Result.StatusCode == HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrMessage"] = displayDataSave.Result.Message;
                        TempData["ErrHeader"] = "Gagal Mengupload Bukti Transfer";
                    }
                }
                else
                {
                    TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                    TempData["ErrHeader"] = "Gagal Mengupload Bukti Transfer";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Gambar belum diupload";
                TempData["ErrHeader"] = "Gagal Mengupload Bukti Transfer";
            }

            return View(uploadBuktiTransfer);
        }
        #endregion
        #region SellingDetails
        [Route("~/DetailTransaksi")]
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
            ResponseWithData<List<SellingDetail>> sellingRequest = GetSellingDetail(id);

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
        #region CustomerFeedback
        [Route("~/ReviewPelanggan")]
        [HttpGet]
        public ActionResult Feedback(int id)
        {
            CustomerFeedback customerFeedback = new CustomerFeedback();
            customerFeedback.Id = "ORD" + id.ToString().PadLeft(4, '0');
            return View(customerFeedback);
        }

        [Route("~/ReviewPelanggan")]
        [HttpPost]
        public ActionResult Feedback(CustomerFeedback feedback)
        {
            CustomerFeedbackRequest request = new CustomerFeedbackRequest()
            {
                Id = int.Parse(feedback.Id.Replace("ORD", "")),
                Note = feedback.Note,
                Rating = feedback.Rating,
                CreatedBy = (int)Session["UserId"]
            };

            // insert data using API
            var apiSave = hcFeedback.PostAsJsonAsync<CustomerFeedbackRequest>("Feedback", request);
            apiSave.Wait();

            var dataSave = apiSave.Result;
            if (dataSave.IsSuccessStatusCode)
            {
                var displayDataSave = dataSave.Content.ReadAsAsync<ResponseWithoutData>();
                displayDataSave.Wait();
                if (displayDataSave.Result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrMessage"] = displayDataSave.Result.Message;
                    TempData["ErrHeader"] = "Gagal Melakukan review";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Melakukan review";
            }

            return View(feedback);
        }
        #endregion
        #region Request Data
        public ResponseWithData<List<HistoryTransaction>> GetHistoryTransaction()
        {
            // get categories
            HttpClient hcCategoryGet = APIHelper.GetHttpClient("HistoryTransaction");
            System.Diagnostics.Debug.WriteLine(Session["UserId"]);

            var apiGet = hcCategoryGet.GetAsync("HistoryTransaction/" + Session["UserId"].ToString());
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<List<HistoryTransaction>>>();
                displayData.Wait();

                return new ResponseWithData<List<HistoryTransaction>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<HistoryTransaction>()
                };
            }
            else
            {
                return new ResponseWithData<List<HistoryTransaction>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<HistoryTransaction>()
                };
            }
        }

        public ResponseWithData<List<SellingDetail>> GetSellingDetail(int id)
        {
            // get categories
            HttpClient hcCategoryGet = APIHelper.GetHttpClient("SellingDetail");

            var apiGet = hcCategoryGet.GetAsync("SellingDetail/" + id.ToString());
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<List<SellingDetail>>>();
                displayData.Wait();

                return new ResponseWithData<List<SellingDetail>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<SellingDetail>()
                };
            }
            else
            {
                return new ResponseWithData<List<SellingDetail>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<SellingDetail>()
                };
            }
        }
        #endregion
    }
}