using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class SuperAdminBannerController : Controller
    {
        #region Variable
        HttpClient hcBannerAdd = APIHelper.GetHttpClient(APIHelper.SA + "/Banner/Add");
        HttpClient hcBannerDelete = APIHelper.GetHttpClient(APIHelper.SA + "/Banner/Delete");
        HttpClient hcBannerEdit = APIHelper.GetHttpClient(APIHelper.SA + "/Banner/Edit");
        #endregion

        #region GetBanner
        // GET Banner
        [Route("~/SA/Banner")]
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

            ResponseDataWithPaging<List<BannerModelView>> bannerRequest = GetBanner(dataPaging);
            ViewBag.CurrentPage = bannerRequest.CurrentPage;
            ViewBag.TotalPage = bannerRequest.TotalPage;

            // status code
            if (bannerRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = bannerRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (bannerRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = bannerRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }


            // data is empty
            if (bannerRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<BannerModelView>> indexViewModel = new IndexModelView<IEnumerable<BannerModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = bannerRequest.Data
            };

            return View(indexViewModel);
        }

        [Route("~/SA/Banner")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<BannerModelView>> indexBanner)
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
                Search = indexBanner.DataPaging.Search
            };

            ResponseDataWithPaging<List<BannerModelView>> bannerRequest = GetBanner(dataPaging);
            ViewBag.CurrentPage = bannerRequest.CurrentPage;
            ViewBag.TotalPage = bannerRequest.TotalPage;

            // status code
            if (bannerRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = bannerRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (bannerRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = bannerRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (bannerRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<BannerModelView>> indexViewModel = new IndexModelView<IEnumerable<BannerModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = bannerRequest.Data
            };

            return View(indexViewModel);
        }
        #endregion
        #region Delete Banner
        [Route("~/SA/Banner/Hapus")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            hcBannerDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcBannerDelete.DeleteAsync("Delete/" + id);
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
                    Session["ErrMessage"] = "Berhasil menghapus banner";
                    Session["ErrHeader"] = "Berhasil";
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal menghapus banner";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal menghapus banner";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Request Data
        public ResponseDataWithPaging<List<BannerModelView>> GetBanner(GetListDataRequest dataPaging)
        {
            // get banner
            HttpClient hcBannerGet = APIHelper.GetHttpClient(APIHelper.SA + "/Banner");
            hcBannerGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcBannerGet.PostAsJsonAsync<GetListDataRequest>("Banner", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<BannerModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<BannerModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<BannerModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<BannerModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<BannerModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}