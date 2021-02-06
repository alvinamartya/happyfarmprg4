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
    public class ProductionAdminPurchasingController : Controller
    {

        #region Get Purchasing History
        // GET: ProductionAdminPurchasing
        [Route("~/PA/RiwayatPembelian")]
        public ActionResult Index(string Sorting_Order, int? Page_No, string tgl_awal, string tgl_akhir)
        {
            // session
            Purchasing.ClearDetailPurchasing();
            Session["FarmerName"] = null;
            Session["FarmerAddress"] = null;
            Session["FarmerPhone"] = null;

            // error
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // get purchasing
            ResponseWithData<List<PurchasingModelView>> purchasingHistoryRequest = GetPurchasing();
            // status code
            if (purchasingHistoryRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = purchasingHistoryRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (purchasingHistoryRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = purchasingHistoryRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            if (tgl_awal != null && tgl_akhir != null)
            {
                if (tgl_awal != "" && tgl_akhir != "")
                {
                    DateTime tglAwal = DateTime.Now;
                    DateTime tglAkhir = DateTime.Now;
                    try
                    {
                        tglAwal = DateTime.Parse(tgl_awal);
                        tglAkhir = DateTime.Parse(tgl_akhir);
                    }
                    catch
                    {
                        TempData["ErrMessage"] = "Tanggal tidak valid";
                        TempData["ErrHeader"] = "Gagal meload data";
                        return View(purchasingHistoryRequest.Data.ToPagedList(noOfPage, sizeOfPage));
                    }

                    DateTime newTglAwal = new DateTime(tglAwal.Year, tglAwal.Month, tglAwal.Day);
                    DateTime newTglAkhir = new DateTime(tglAkhir.Year, tglAkhir.Month, tglAkhir.Day).AddDays(1);

                    ViewBag.TglAwal = tgl_awal;
                    ViewBag.TglAkhir = tgl_akhir;

                    purchasingHistoryRequest.Data = purchasingHistoryRequest.Data.Where(x => x.DateTime >= newTglAwal && x.DateTime <= newTglAkhir).ToList();
                }
            }

            // data is empty
            if (purchasingHistoryRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }


            return View(purchasingHistoryRequest.Data.ToPagedList(noOfPage, sizeOfPage));
        }
        #endregion
        #region Add Purchasing
        [Route("~/PA/Pembelian/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            // error
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            var purchasingDetailRequest = Purchasing.GetDetailPurchasing();

            // data is empty
            if (purchasingDetailRequest.Count == 0)
            {
                TempData["ErrMessageDetail"] = "Produk belum ada";
            }

            // calculate total purchases
            decimal totalPurchase = 0;
            foreach(PurchasingDetailRequest detailRequest in purchasingDetailRequest)
            {
                totalPurchase += detailRequest.Price * detailRequest.Qty;
            }

            string rupiah = string.Format("{0:C}", totalPurchase);
            if (!rupiah.Contains("Rp"))
                rupiah = "Rp" + rupiah.Remove(0, 1);
            ViewBag.TotalPurchases = rupiah;
            AddPurchasingRequest purchasingRequest = new AddPurchasingRequest()
            {
                PurchasingDetails = purchasingDetailRequest,
                FarmerAddress = (string)Session["FarmerAddress"],
                FarmerName = (string)Session["FarmerName"],
                FarmerPhone = (string)Session["FarmerPhone"]
            };

            foreach(var x in Session.Keys)
            {
                System.Diagnostics.Debug.WriteLine(x);
            }

            return View(purchasingRequest);
        }

        [Route("~/PA/Pembelian/Tambah")]
        [HttpPost]
        public ActionResult Add(AddPurchasingRequest addPurchasingRequest)
        {
            // error
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            if(addPurchasingRequest.PurchasingDetails == null)
            {
                addPurchasingRequest.PurchasingDetails = Purchasing.GetDetailPurchasing();
            }

            // data is empty
            if (addPurchasingRequest.PurchasingDetails.Count == 0)
            {
                TempData["ErrMessageDetail"] = "Produk belum ada";

                TempData["ErrMessage"] = "Produk belum dipilih";
                TempData["ErrHeader"] = "Gagal melakukan pembelian";
            }
            else
            {
                addPurchasingRequest.EmployeeId = (int)Session["UserId"];

                // insert data using API
                HttpClient hcPurchasingAdd = APIHelper.GetHttpClient(APIHelper.PA + "/Purchasing/Add");
                hcPurchasingAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
                var apiSave = hcPurchasingAdd.PostAsJsonAsync<AddPurchasingRequest>("Add", addPurchasingRequest);
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
                        TempData["ErrHeader"] = "Gagal Melakukan Pembelian";
                    }
                }
                else
                {
                    TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                    TempData["ErrHeader"] = "Gagal Melakukan Pembelian";
                }
            }

            // calculate total purchases
            decimal totalPurchase = 0;
            foreach (PurchasingDetailRequest detailRequest in addPurchasingRequest.PurchasingDetails)
            {
                totalPurchase += detailRequest.Price * detailRequest.Qty;
            }

            ViewBag.TotalPurchases = string.Format("{0:C}", totalPurchase);

            return View(addPurchasingRequest);
        }
        #endregion
        #region Add Purchasing Detail
        [Route("~/PA/Pembelian/Tambah/TambahDetail")]
        [HttpGet]
        public ActionResult AddDetail()
        {
            // get goods
            ResponseWithData<List<GoodsListModelView>> goodsRequest = GetGoods();
            if (goodsRequest.StatusCode == HttpStatusCode.OK)
            {
                var goodsList = Purchasing.GetGoodsListModel(goodsRequest.Data);
                if(goodsList.Count > 0)
                {
                    ViewBag.Products = new SelectList(goodsList, "Id", "Name");
                }
                else
                {
                    Session["ErrMessage"] = "Produk sudah tidak tersedia";
                    Session["ErrHeader"] = "Tidak dapat menambah detail";
                    return RedirectToAction("Add");
                }
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

            return View();
        }

        [Route("~/PA/Pembelian/Tambah/TambahDetail")]
        [HttpPost]
        public ActionResult AddDetail(PurchasingDetailRequest purchasingDetailRequest)
        {
            ResponseWithData<List<GoodsListModelView>> goodsRequest = GetGoods();
            if (goodsRequest.StatusCode == HttpStatusCode.OK)
            {
                var goodsList = Purchasing.GetGoodsListModel(goodsRequest.Data);
                if (goodsList.Count > 0)
                {
                    ViewBag.Products = new SelectList(goodsList, "Id", "Name");
                }
                else
                {
                    Session["ErrMessage"] = "Produk sudah tidak tersedia";
                    Session["ErrHeader"] = "Tidak dapat menambah detail";
                    return RedirectToAction("Add");
                }

                string name = goodsRequest.Data.Where(x => x.Id == purchasingDetailRequest.GoodsId).FirstOrDefault().Name;
                purchasingDetailRequest.GoodsName = name;
                Purchasing.AddDetailPurchasing(purchasingDetailRequest);
                return RedirectToAction("Add");
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

            return View(purchasingDetailRequest);
        }
        #endregion
        #region Edit Purchasing Detail
        [Route("~/PA/Pembelian/Tambah/UbahDetail/{id}")]
        [HttpGet]
        public ActionResult EditDetail(int id)
        {
            var detailPurchasing = Purchasing.GetDetailPurchasingByID(id);
            return View(detailPurchasing);
        }

        [Route("~/PA/Pembelian/Tambah/UbahDetail/{id}")]
        [HttpPost]
        public ActionResult EditDetail(PurchasingDetailRequest purchasingDetailRequest)
        {
            Purchasing.EditDetailPurchasing(purchasingDetailRequest);
            return RedirectToAction("Add");
        }
        #endregion
        #region Delete Purchasing Detail
        [Route("~/PA/Pembelian/Tambah/Detail/Hapus/{id}")]
        [HttpPost]
        public ActionResult DeleteDetail(int id)
        {
            // delete detail
            Purchasing.DeleteDetailPurchasing(id);
            return RedirectToAction("Add");
        }
        #endregion
        #region Delete Purchasing
        [Route("~/PA/Pembelian/Hapus")]
        [HttpPost]
        public ActionResult DeletePurchasing(DeletePurchasingRequest deletePurchasingRequest)
        {
            System.Diagnostics.Debug.WriteLine(deletePurchasingRequest.Id);
            deletePurchasingRequest.EmployeeId = (int)Session["UserId"];

            // insert data using API
            HttpClient hcPurchasingDelete = APIHelper.GetHttpClient(APIHelper.PA + "/Purchasing/Delete");
            hcPurchasingDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcPurchasingDelete.PostAsJsonAsync<DeletePurchasingRequest>("Delete", deletePurchasingRequest);
            apiDelete.Wait();

            var dataDelete = apiDelete.Result;
            if (dataDelete.IsSuccessStatusCode)
            {
                var displayDataDelete = dataDelete.Content.ReadAsAsync<ResponseWithoutData>();
                displayDataDelete.Wait();
                if (displayDataDelete.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayDataDelete.Result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal Menghapus Pembelian";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal Menghapus Pembelian";
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Save Session Purchasing
        [Route("~/PA/Pembelian/Tambah/Session")]
        [HttpPost]
        public ActionResult SaveSessionPurchasing(PurchasingSession session)
        {
            Session["FarmerName"] = session.FarmerName;
            Session["FarmerAddress"] = session.FarmerAddress;
            Session["FarmerPhone"] = session.FarmerPhone;

            return Json(HttpStatusCode.OK, JsonRequestBehavior.AllowGet);
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
        public ResponseWithData<List<PurchasingModelView>> GetPurchasing()
        {
            // get categories
            HttpClient hcPurchasing = APIHelper.GetHttpClient(APIHelper.PA + "/Purchasing");
            hcPurchasing.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetGoods = hcPurchasing.GetAsync("Purchasing/" + (int)Session["UserId"]);
            apiGetGoods.Wait();

            var dataGoods = apiGetGoods.Result;
            if (dataGoods.IsSuccessStatusCode)
            {
                var displayDataGoods = dataGoods.Content.ReadAsAsync<ResponseWithData<List<PurchasingModelView>>>();
                displayDataGoods.Wait();
                return new ResponseWithData<List<PurchasingModelView>>()
                {
                    StatusCode = displayDataGoods.Result.StatusCode,
                    Message = displayDataGoods.Result.Message,
                    Data = displayDataGoods.Result.StatusCode == HttpStatusCode.OK ? displayDataGoods.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<PurchasingModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<PurchasingModelView>()
                };
            }
        }
        #endregion
    }
}