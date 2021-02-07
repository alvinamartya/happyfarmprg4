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
    public class PurchasingController : Controller
    {
        #region Index
        // GET: Purchasing
        public ActionResult Index()
        {
            // error
            if (Session["ErrMessage"] != null && Session["ErrHeader"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            return View();
        }
        #endregion
        #region Get Purchasing Detail
        [HttpGet]
        public ActionResult GetPurchasingDetail()
        {
            return Json(Purchasing.GetDetailPurchasing().OrderBy(x => x.CategoryName).ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Add Purchasing Detail
        [HttpGet]
        public ActionResult Add(int? Category)
        {
            List<Category> categories = new List<Category>();
            ResponseWithData<List<Category>> categoryRequest = GetCategory();
            if (categoryRequest.StatusCode == HttpStatusCode.OK)
            {
                if (Category != null)
                {
                    ViewBag.Categories = new SelectList(categoryRequest.Data, "Id", "Name", Category);
                }
                else
                {
                    ViewBag.Categories = new SelectList(categoryRequest.Data, "Id", "Name");
                }
                categories = categoryRequest.Data;
            }
            else
            {
                TempData["ErrMessage"] = categoryRequest.Message;
                TempData["ErrHeader"] = "Gagal meload kategori";
            }

            // get goods
            ResponseWithData<List<GoodsListModelView>> goodsRequest = GetGoods();
            if (goodsRequest.StatusCode == HttpStatusCode.OK)
            {
                var goodsList = Purchasing.GetGoodsListModel(goodsRequest.Data);
                if (goodsList.Count > 0)
                {
                    ResponseWithData<List<GoodsListModelView>> goodsRegionRequest = GetGoodsRegion(new GoodsRegion()
                    {
                        CategoryId = Category ?? categories[0].Id,
                        RegionId = (int)Session["RegionId"]
                    });

                    if (goodsRegionRequest.StatusCode == HttpStatusCode.OK)
                    {
                        ViewBag.Products = new SelectList(goodsRegionRequest.Data, "Id", "Name");

                        if (goodsRegionRequest.Data.Count > 0)
                        {
                            ViewBag.Price = goodsRegionRequest.Data[0].Price;
                        }
                        else
                        {
                            ViewBag.Price = 0;
                        }
                    }
                    else
                    {
                        TempData["ErrMessage"] = goodsRegionRequest.Message;
                        TempData["ErrHeader"] = "Gagal meload produk per wilayah";
                        return View();
                    }
                }
                else
                {
                    Session["ErrMessage"] = "Produk sudah tidak tersedia";
                    Session["ErrHeader"] = "Tidak dapat menambah detail";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrMessage"] = goodsRequest.Message;
                TempData["ErrHeader"] = "Gagal meload produk";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Add(PurchasingDetailRequest purchasingDetailRequest)
        {
            List<Category> categories = new List<Category>();
            ResponseWithData<List<Category>> categoryRequest = GetCategory();
            if (categoryRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Categories = new SelectList(categoryRequest.Data, "Id", "Name");
            }
            else
            {
                TempData["ErrMessage"] = categoryRequest.Message;
                TempData["ErrHeader"] = "Gagal meload kategori";
                return RedirectToAction("Index");
            }

            ResponseWithData<List<GoodsListModelView>> goodsRequest = GetGoods();
            if (goodsRequest.StatusCode == HttpStatusCode.OK)
            {
                var goodsList = Purchasing.GetGoodsListModel(goodsRequest.Data);
                if (goodsList.Count > 0)
                {

                    ResponseWithData<List<GoodsListModelView>> goodsRegionRequest = GetGoodsRegion(new GoodsRegion()
                    {
                        CategoryId = purchasingDetailRequest.CategoryId,
                        RegionId = (int)Session["RegionId"]
                    });

                    if (goodsRegionRequest.StatusCode == HttpStatusCode.OK)
                    {
                        ViewBag.Products = new SelectList(goodsRegionRequest.Data, "Id", "Name");
                        ViewBag.Price = goodsRegionRequest.Data.Where(x => x.Id == purchasingDetailRequest.GoodsId).FirstOrDefault().Price;

                        string name = goodsRequest.Data.Where(x => x.Id == purchasingDetailRequest.GoodsId).FirstOrDefault().Name;
                        purchasingDetailRequest.GoodsName = name;

                        string categoryName = categoryRequest.Data.Where(x => x.Id == purchasingDetailRequest.CategoryId).FirstOrDefault().Name;
                        purchasingDetailRequest.CategoryName = categoryName;

                        purchasingDetailRequest.Price = goodsRegionRequest.Data.Where(x => x.Id == purchasingDetailRequest.GoodsId).FirstOrDefault().Price;
                        Purchasing.AddDetailPurchasing(purchasingDetailRequest);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrMessage"] = goodsRegionRequest.Message;
                        TempData["ErrHeader"] = "Gagal meload produk per wilayah";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Session["ErrMessage"] = "Produk sudah tidak tersedia";
                    Session["ErrHeader"] = "Tidak dapat menambah detail";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrMessage"] = categoryRequest.Message;
                TempData["ErrHeader"] = "Gagal meload produk";
                return RedirectToAction("Index");
            }

            return View(purchasingDetailRequest);
        }
        #endregion
        #region Edit Purchasing Detail
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var detailPurchasing = Purchasing.GetDetailPurchasingByID(id);
            return View(detailPurchasing);
        }

        [HttpPost]
        public ActionResult Edit(PurchasingDetailRequest purchasingDetailRequest)
        {
            System.Diagnostics.Debug.WriteLine(purchasingDetailRequest.Qty);
            System.Diagnostics.Debug.WriteLine(purchasingDetailRequest.GoodsId);

            Purchasing.EditDetailPurchasing(purchasingDetailRequest);
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete Purchasing Detail
        [HttpGet]
        public ActionResult Delete(int id)
        {
            // delete detail
            Purchasing.DeleteDetailPurchasing(id);
            return RedirectToAction("Index");
        }
        #endregion
        #region Request Data
        public ResponseWithData<List<Category>> GetCategory()
        {
            // get categories
            HttpClient hcCategories = APIHelper.GetHttpClient("Category");
            var apiGetCategories = hcCategories.GetAsync("Category");
            apiGetCategories.Wait();

            var dataCategories = apiGetCategories.Result;
            if (dataCategories.IsSuccessStatusCode)
            {
                var displayDataCategories = dataCategories.Content.ReadAsAsync<ResponseWithData<List<Category>>>();
                displayDataCategories.Wait();
                return new ResponseWithData<List<Category>>()
                {
                    StatusCode = displayDataCategories.Result.StatusCode,
                    Message = displayDataCategories.Result.Message,
                    Data = displayDataCategories.Result.StatusCode == HttpStatusCode.OK ? displayDataCategories.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<Category>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<Category>()
                };
            }
        }
        public ResponseWithData<List<GoodsListModelView>> GetGoods()
        {
            // get goods
            HttpClient hcGoods = APIHelper.GetHttpClient("Goods");
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

        public ResponseWithData<List<GoodsListModelView>> GetGoodsRegion(GoodsRegion goodsRegion)
        {
            // get goods
            HttpClient hcGoods = APIHelper.GetHttpClient("Goods");
            var apiGetGoods = hcGoods.PostAsJsonAsync<GoodsRegion>("Goods", goodsRegion);
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
        #endregion
        #region Save Session Purchasing
        [HttpPost]
        public ActionResult SaveSessionPurchasing(PurchasingModel session)
        {
            Session["RecipientName"] = session.RecipientName;
            Session["RecipientPhone"] = session.RecipientPhone;
            Session["RecipientAddress"] = session.RecipientAddress;
            Session["PromoCode"] = session.PromoCode;
            Session["RegionId"] = session.RegionId;
            Session["SubdistrictId"] = session.SubdistrictId;
            Session["TotalPurchase"] = session.TotalPurchase;
            Session["Discount"] = session.Discount;
            Session["ShippingCharges"] = session.ShippingCharges;
            return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Purchase
        [HttpPost]
        public ActionResult Purchase(Purchase purchase)
        {
            if (purchase.TotalPurchase <= 0)
            {
                Session["ErrHeader"] = "Gagal melakukan pembelian";
                Session["ErrMessage"] = "Produk belum dipilih";

                Session["RecipientName"] = purchase.RecipientName;
                Session["RecipientPhone"] = purchase.RecipientPhone;
                Session["RecipientAddress"] = purchase.RecipientAddress;
                Session["PromoCode"] = purchase.PromoCode;
                Session["RegionId"] = purchase.RegionId;
                Session["SubdistrictId"] = purchase.SubdistrictId;
                Session["TotalPurchase"] = purchase.TotalPurchase;
                Session["Discount"] = purchase.Discount;
                Session["ShippingCharges"] = purchase.ShippingCharges;
                return RedirectToAction("Index");
            }
            else
            {
                HttpClient hc = APIHelper.GetHttpClient("Pembelian");
                purchase.CustomerId = (int)Session["UserId"];
                purchase.RegionId = (int)Session["RegionId"];
                purchase.SubdistrictId = (int)Session["SubdistrictId"];
                purchase.Discount = (int)Session["Discount"];
                purchase.ShippingCharges = (int)Session["ShippingCharges"];
                purchase.TotalPurchase = (int)Session["TotalPurchase"];
                purchase.Products = Purchasing.GetDetailPurchasing();
                var apiRegister = hc.PostAsJsonAsync<Purchase>("Pembelian", purchase);
                apiRegister.Wait();

                var registerData = apiRegister.Result;
                if (registerData.IsSuccessStatusCode)
                {
                    var registerResponse = registerData.Content.ReadAsAsync<ResponseWithoutData>();
                    registerResponse.Wait();

                    if (registerResponse.Result.StatusCode == HttpStatusCode.OK)
                    {
                        Session["RecipientName"] = null;
                        Session["RecipientPhone"] = null;
                        Session["RecipientAddress"] = null;
                        Session["PromoCode"] = null;
                        Session["RegionId"] = null;
                        Session["SubdistrictId"] = null;
                        Session["TotalPurchase"] = null;
                        Session["Discount"] = null;
                        Session["ShippingCharges"] = null;
                        Purchasing.ClearDetailPurchasing();

                        Session["ErrHeader"] = "Berhasil melakukan pembelian";
                        Session["ErrMessage"] = "Pembelian telah berhasil";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session["ErrHeader"] = "Gagal melakukan pembelian";
                        Session["ErrMessage"] = registerResponse.Result.Message;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    Session["ErrHeader"] = "Gagal melakukan pembelian";
                    Session["ErrMessage"] = "Terjadi kesalahan pada sistem, silahkan hubungi admin.";
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion
    }
}