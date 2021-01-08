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
    public class SuperAdminGoodsController : Controller
    {
        #region Variable
        HttpClient hcGoodsAdd = APIHelper.GetHttpClient(APIHelper.SA + "/Goods/Add");
        HttpClient hcGoodsDelete = APIHelper.GetHttpClient(APIHelper.SA + "/Goods/Delete");
        HttpClient hcGoodsEdit = APIHelper.GetHttpClient(APIHelper.SA + "/Goods/Edit");
        #endregion
        #region GetGoods
        // GET Goods
        [Route("~/SA/Produk")]
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

            ResponseDataWithPaging<List<GoodsModelView>> goodsRequest = GetGoods(dataPaging);
            ViewBag.CurrentPage = goodsRequest.CurrentPage;
            ViewBag.TotalPage = goodsRequest.TotalPage;

            // status code
            if (goodsRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = goodsRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (goodsRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = goodsRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }


            // data is empty
            if (goodsRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<GoodsModelView>> indexViewModel = new IndexModelView<IEnumerable<GoodsModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = goodsRequest.Data
            };

            return View(indexViewModel);
        }

        [Route("~/SA/Produk")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<GoodsModelView>> indexGoods)
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
                Search = indexGoods.DataPaging.Search
            };

            ResponseDataWithPaging<List<GoodsModelView>> goodsRequest = GetGoods(dataPaging);
            ViewBag.CurrentPage = goodsRequest.CurrentPage;
            ViewBag.TotalPage = goodsRequest.TotalPage;

            // status code
            if (goodsRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = goodsRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (goodsRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = goodsRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (goodsRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexModelView<IEnumerable<GoodsModelView>> indexViewModel = new IndexModelView<IEnumerable<GoodsModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = goodsRequest.Data
            };

            return View(indexViewModel);
        }
        #endregion
        #region Request Data
        public ResponseDataWithPaging<List<GoodsModelView>> GetGoods(GetListDataRequest dataPaging)
        {
            // get goods
            HttpClient hcGoodsGet = APIHelper.GetHttpClient(APIHelper.SA + "/Goods");
            hcGoodsGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcGoodsGet.PostAsJsonAsync<GetListDataRequest>("Goods", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<GoodsModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<GoodsModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<GoodsModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<GoodsModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<GoodsModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}