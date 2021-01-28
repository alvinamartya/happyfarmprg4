using HappyFarmProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProject.Controllers
{
    public class PromoController : Controller
    {
        #region Action
        // GET: Promo
        public ActionResult Index()
        {
            ResponseWithData<List<PromoModel>> promoRequest = GetPromos();
            if (promoRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Promos = promoRequest.Data;
            }
            else
            {
                TempData["ErrMessage"] = promoRequest.Message;
                TempData["ErrHeader"] = "Gagal meload promo";
            }

            return View();
        }
        #endregion

        #region Request Data
        public ResponseWithData<List<PromoModel>> GetPromos()
        {
            // get categories
            HttpClient hcCategoryGet = APIHelper.GetHttpClient("Promo");

            var apiGet = hcCategoryGet.GetAsync("Promo");
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<List<PromoModel>>>();
                displayData.Wait();

                return new ResponseWithData<List<PromoModel>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<PromoModel>()
                };
            }
            else
            {
                return new ResponseWithData<List<PromoModel>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<PromoModel>()
                };
            }
        }
        #endregion
    }
}