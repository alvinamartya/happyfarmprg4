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
    public class ProductionAdminChangePriceProductController : Controller
    {
        #region Variable
        HttpClient hcGoodsPriceRegionAdd = APIHelper.GetHttpClient(APIHelper.PA + "/GoodsPrice/Edit");
        #endregion

        #region Get GoodsPrice()
        [Route("~/PA/HargaWilayah")]
        [HttpGet]
        public ActionResult Index(string Sorting_Order, int? Page_No, string Product)
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
            ViewBag.CurrentSortOder = Sorting_Order;
            ViewBag.SortingRegion = Sorting_Order == "Region_Desc" ? "Region_Asc" : "Region_Desc";
            ViewBag.SortingPrice = Sorting_Order == "Price_Desc" ? "Price_Asc" : "Price_Desc";
            
            // get goods
            ResponseWithData<List<GoodsListModelView>> goodsRequest = GetGoods();
            if (goodsRequest.StatusCode == HttpStatusCode.OK)
            {
                var item = new SelectList(goodsRequest.Data, "Id", "Name");

                if(Product != null)
                {
                    var selected = item.Where(x => x.Value == Product).FirstOrDefault();
                    selected.Selected = true;
                }

                ViewBag.Products = item;
            }
            else if (goodsRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = goodsRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = goodsRequest.Message;
                TempData["ErrHeader"] = "Gagal meload produk";
            }

            string productId = "0";
            if (Product != null) productId = Product;
            else if(goodsRequest.StatusCode == HttpStatusCode.OK) productId = goodsRequest.Data[0].Id.ToString();

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            ViewBag.ProductId = productId;
            ResponseDataWithPaging<List<GoodsPriceModelView>> goodsPriceRequest = GetGoodsPrice(dataPaging, int.Parse(productId));
            // status code
            if (goodsPriceRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = goodsPriceRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (goodsPriceRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = goodsPriceRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (goodsPriceRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            // sorting
            switch (Sorting_Order)
            {
                case "Region_Asc":
                    goodsPriceRequest.Data = goodsPriceRequest.Data.OrderBy(x => x.Region).ToList();
                    break;
                case "Region_Desc":
                    goodsPriceRequest.Data = goodsPriceRequest.Data.OrderByDescending(x => x.Region).ToList();
                    break;
                case "Price_Asc":
                    goodsPriceRequest.Data = goodsPriceRequest.Data.OrderBy(x => x.Price).ToList();
                    break;
                case "Price_Desc":
                    goodsPriceRequest.Data = goodsPriceRequest.Data.OrderByDescending(x => x.Price).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<GoodsPriceModelView>> indexViewModel = new IndexModelView<IPagedList<GoodsPriceModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = goodsPriceRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }
        #endregion

        #region Get GoodsPrice()
        [Route("~/PA/HargaWilayah/Ubah")]
        [HttpGet]
        public ActionResult Edit(string GoodsId)
        {
            // get regions
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

            AddGoodsPriceRegionRequest goodsPriceRegionRequest = new AddGoodsPriceRegionRequest()
            {
                GoodsId = int.Parse(GoodsId)
            };

            return View(goodsPriceRegionRequest);
        }

        [Route("~/PA/HargaWilayah/Ubah")]
        [HttpPost]
        public ActionResult Edit(AddGoodsPriceRegionRequest goodsPriceRegionRequest)
        {
            // get goods
            ResponseWithData<List<RegionModelView>> goodsRequest = GetRegions();
            if (goodsRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Regions = new SelectList(goodsRequest.Data, "Id", "Name");
            }
            else if (goodsRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = goodsRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = goodsRequest.Message;
                TempData["ErrHeader"] = "Gagal meload wilayah";
            }

            goodsPriceRegionRequest.CreatedBy = (int)Session["UserId"];

            // insert data using API
            hcGoodsPriceRegionAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiSave = hcGoodsPriceRegionAdd.PostAsJsonAsync<AddGoodsPriceRegionRequest>("Edit", goodsPriceRegionRequest);
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
                else if (displayDataSave.Result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index",new { ProductId = goodsPriceRegionRequest.GoodsId });
                }
                else
                {
                    TempData["ErrMessage"] = displayDataSave.Result.Message;
                    TempData["ErrHeader"] = "Gagal Menambah Harga Produk";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Menambah Harga Produk";
            }

            return View(goodsPriceRegionRequest);
        }
        #endregion

        #region RequestData
        public ResponseWithData<List<GoodsListModelView>> GetGoods()
        {
            // get categories
            HttpClient hcGoods = APIHelper.GetHttpClient(APIHelper.PA + "/Goods");
            hcGoods.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetGoods = hcGoods.GetAsync("Goods");
            apiGetGoods.Wait();

            var dataGoods = apiGetGoods.Result;
            if (dataGoods.IsSuccessStatusCode)
            {
                var displayDataGoods = dataGoods.Content.ReadAsAsync<ResponseWithData<List<GoodsListModelView>>>();
                displayDataGoods.Wait();
                return new ResponseWithData<List<GoodsListModelView>>()
                {
                    StatusCode = displayDataGoods.Result.StatusCode,
                    Message = displayDataGoods.Result.Message,
                    Data = displayDataGoods.Result.StatusCode == HttpStatusCode.OK ? displayDataGoods.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<GoodsListModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<GoodsListModelView>()
                };
            }
        }

        public ResponseWithData<List<RegionModelView>> GetRegions()
        {
            // get categories
            HttpClient hcRegions = APIHelper.GetHttpClient(APIHelper.PA + "/Regions");
            hcRegions.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetRegions = hcRegions.GetAsync("Regions");
            apiGetRegions.Wait();

            var dataRegions = apiGetRegions.Result;
            if (dataRegions.IsSuccessStatusCode)
            {
                var displayDataGoods = dataRegions.Content.ReadAsAsync<ResponseWithData<List<RegionModelView>>>();
                displayDataGoods.Wait();
                return new ResponseWithData<List<RegionModelView>>()
                {
                    StatusCode = displayDataGoods.Result.StatusCode,
                    Message = displayDataGoods.Result.Message,
                    Data = displayDataGoods.Result.StatusCode == HttpStatusCode.OK ? displayDataGoods.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<RegionModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<RegionModelView>()
                };
            }
        }

        public ResponseDataWithPaging<List<GoodsPriceModelView>> GetGoodsPrice(GetListDataRequest dataPaging, int goodsId)
        {
            // get goods
            HttpClient hcGoodsGet = APIHelper.GetHttpClient(APIHelper.PA + "/GoodsPrice");
            hcGoodsGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcGoodsGet.PostAsJsonAsync<GetListDataRequest>("GoodsPrice/" + goodsId, dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<GoodsPriceModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<GoodsPriceModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<GoodsPriceModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<GoodsPriceModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<GoodsPriceModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}