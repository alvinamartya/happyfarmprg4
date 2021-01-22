using HappyFarmProjectWebAdmin.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class CustomerServiceCustomerFeedbackController : Controller
    {
        #region GetCustomerFeedback
        // GET CustomerFeedback
        [Route("~/CS/ReviewPelanggan")]
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
            ViewBag.SortingOrderId = Sorting_Order == "OrderId_Desc" ? "OrderId_Asc" : "OrderId_Desc";
            ViewBag.SortingRating = Sorting_Order == "Rating_Desc" ? "Rating_Asc" : "Rating_Desc";
            ViewBag.SortingNote = Sorting_Order == "Note_Desc" ? "Note_Asc" : "Note_Desc";

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            ResponseDataWithPaging<List<CustomerFeedbackModelView>> customerFeedbackRequest = GetCustomerFeedbacks(dataPaging);
            ViewBag.CurrentPage = customerFeedbackRequest.CurrentPage;
            ViewBag.TotalPage = customerFeedbackRequest.TotalPage;

            // status code
            if (customerFeedbackRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = customerFeedbackRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (customerFeedbackRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = customerFeedbackRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (customerFeedbackRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "OrderId_Desc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderByDescending(x => x.OrderId).ToList();
                    break;
                case "OrderId_Asc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderBy(x => x.OrderId).ToList();
                    break;
                case "Rating_Desc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderByDescending(x => x.Rating).ToList();
                    break;
                case "Rating_Asc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderBy(x => x.Rating).ToList();
                    break;
                case "Note_Desc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderByDescending(x => x.Note).ToList();
                    break;
                case "Note_Asc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderBy(x => x.Note).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<CustomerFeedbackModelView>> indexViewModel = new IndexModelView<IPagedList<CustomerFeedbackModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = customerFeedbackRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }

        [Route("~/CS/ReviewPelanggan")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<SubDistrictModelView>> indexSubDistrict, string Sorting_Order, int? Page_No)
        {
            System.Diagnostics.Debug.WriteLine(indexSubDistrict.DataPaging.Search);
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // sorting state
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingOrderId = Sorting_Order == "OrderId_Desc" ? "OrderId_Asc" : "OrderId_Desc";
            ViewBag.SortingRating = Sorting_Order == "Rating_Desc" ? "Rating_Asc" : "Rating_Desc";
            ViewBag.SortingNote = Sorting_Order == "Note_Desc" ? "Note_Asc" : "Note_Desc";

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = indexSubDistrict.DataPaging.Search
            };

            ResponseDataWithPaging<List<CustomerFeedbackModelView>> customerFeedbackRequest = GetCustomerFeedbacks(dataPaging);
            ViewBag.CurrentPage = customerFeedbackRequest.CurrentPage;
            ViewBag.TotalPage = customerFeedbackRequest.TotalPage;

            // status code
            if (customerFeedbackRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = customerFeedbackRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (customerFeedbackRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = customerFeedbackRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (customerFeedbackRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "OrderId_Desc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderByDescending(x => x.OrderId).ToList();
                    break;
                case "OrderId_Asc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderBy(x => x.OrderId).ToList();
                    break;
                case "Rating_Desc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderByDescending(x => x.Rating).ToList();
                    break;
                case "Rating_Asc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderBy(x => x.Rating).ToList();
                    break;
                case "Note_Desc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderByDescending(x => x.Note).ToList();
                    break;
                case "Note_Asc":
                    customerFeedbackRequest.Data = customerFeedbackRequest.Data.OrderBy(x => x.Note).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<CustomerFeedbackModelView>> indexViewModel = new IndexModelView<IPagedList<CustomerFeedbackModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = customerFeedbackRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }
        #endregion

        #region Request Data
        public ResponseDataWithPaging<List<CustomerFeedbackModelView>> GetCustomerFeedbacks(GetListDataRequest dataPaging)
        {
            // get customerfeedbacks
            HttpClient hcCustomerFeedbackGet = APIHelper.GetHttpClient(APIHelper.CS + "/CustomerFeedback");
            hcCustomerFeedbackGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcCustomerFeedbackGet.PostAsJsonAsync<GetListDataRequest>("CustomerFeedback", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<CustomerFeedbackModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<CustomerFeedbackModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<CustomerFeedbackModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<CustomerFeedbackModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<CustomerFeedbackModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}