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
    public class SuperAdminPromoController : Controller
    {
        #region Variable
        HttpClient hcPromoAdd = APIHelper.GetHttpClient(APIHelper.SA + "/Promo/Add");
        HttpClient hcPromoDelete = APIHelper.GetHttpClient(APIHelper.SA + "/Promo/Delete");
        HttpClient hcPromodit = APIHelper.GetHttpClient(APIHelper.SA + "/Promo/Edit");
        #endregion

        #region GetPromo
        // GET Promo
        [Route("~/SA/Promo")]
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

            ResponseDataWithPaging<List<PromoModelView>> promoRequest = GetPromo(dataPaging);
            ViewBag.CurrentPage = promoRequest.CurrentPage;
            ViewBag.TotalPage = promoRequest.TotalPage;

            // status code
            if (promoRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = promoRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (promoRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = promoRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }


            // data is empty
            if (promoRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<PromoModelView>> indexViewModel = new IndexModelView<IEnumerable<PromoModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = promoRequest.Data
            };

            return View(indexViewModel);
        }

        [Route("~/SA/Promo")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<PromoModelView>> indexPromo)
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
                Search = indexPromo.DataPaging.Search
            };

            ResponseDataWithPaging<List<PromoModelView>> promoRequest = GetPromo(dataPaging);
            ViewBag.CurrentPage = promoRequest.CurrentPage;
            ViewBag.TotalPage = promoRequest.TotalPage;

            // status code
            if (promoRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = promoRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (promoRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = promoRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (promoRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<PromoModelView>> indexViewModel = new IndexModelView<IEnumerable<PromoModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = promoRequest.Data
            };

            return View(indexViewModel);
        }
        #endregion
        #region Delete Promo
        [Route("~/SA/Promo/Hapus")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            hcPromoDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcPromoDelete.DeleteAsync("Delete/" + id);
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
                    Session["ErrMessage"] = "Berhasil menghapus promo";
                    Session["ErrHeader"] = "Berhasil";
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal menghapus promo";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal menghapus promo";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Request Data
        public ResponseDataWithPaging<List<PromoModelView>> GetPromo(GetListDataRequest dataPaging)
        {
            // get promo
            HttpClient hcPromoGet = APIHelper.GetHttpClient(APIHelper.SA + "/Promo");
            hcPromoGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcPromoGet.PostAsJsonAsync<GetListDataRequest>("Promo", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<PromoModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<PromoModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<PromoModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<PromoModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<PromoModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}