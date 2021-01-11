using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            System.Diagnostics.Debug.Write(indexGoods.DataPaging.Search);
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
        #region Delete Goods
        [Route("~/SA/Produk/Hapus")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            hcGoodsDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcGoodsDelete.DeleteAsync("Delete/" + id);
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
                    Session["ErrMessage"] = "Berhasil menghapus produk";
                    Session["ErrHeader"] = "Berhasil";
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal Menghapus Produk";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal Menghapus Produk";
            }
            return RedirectToAction("Index");
        }
        #endregion
        #region Add Goods
        // Add Employee
        [Route("~/SA/Produk/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            // get categories
            ResponseWithData<List<CategoryModelView>> categoriesRequest = GetCategories();
            if (categoriesRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Categories = new SelectList(categoriesRequest.Data, "Id", "Name");
            }
            else if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal Meload Kategori";
            }

            AddGoodsRequest goodsRequest = new AddGoodsRequest()
            {
                CreatedBy = (int)Session["UserId"]
            };

            return View(goodsRequest);
        }

        [Route("~/SA/Produk/Tambah")]
        [HttpPost]
        public ActionResult Add(AddGoodsRequest goodRequest)
        {
            // get categories
            ResponseWithData<List<CategoryModelView>> categoriesRequest = GetCategories();
            if (categoriesRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Categories = new SelectList(categoriesRequest.Data, "Id", "Name");
            }
            else if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal Meload Kategori";
            }

            System.Diagnostics.Debug.WriteLine(Path.GetFileName(goodRequest.Image.FileName));

            //// add created by
            //goodRequest.CreatedBy = (int)Session["UserId"];

            //// insert data using API
            //hcGoodsAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            //var apiSave = hcGoodsAdd.PostAsJsonAsync<AddGoodsRequest>("Add", goodRequest);
            //apiSave.Wait();

            //var dataSave = apiSave.Result;
            //if (dataSave.IsSuccessStatusCode)
            //{
            //    var displayDataSave = dataSave.Content.ReadAsAsync<ResponseWithoutData>();
            //    displayDataSave.Wait();
            //    if (displayDataSave.Result.StatusCode == HttpStatusCode.Unauthorized)
            //    {
            //        Session["ErrMessage"] = displayDataSave.Result.Message;
            //        return RedirectToAction("Index", "Login");
            //    }
            //    else if (displayDataSave.Result.StatusCode == HttpStatusCode.Created)
            //    {
            //        return RedirectToAction("Index");
            //    }
            //    else
            //    {
            //        TempData["ErrMessage"] = displayDataSave.Result.Message;
            //        TempData["ErrHeader"] = "Gagal Menambah Produk";
            //    }
            //}
            //else
            //{
            //    TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
            //    TempData["ErrHeader"] = "Gagal Menambah Produk";
            //}
            return View();
        }
        #endregion
        #region Request Data
        public ResponseWithData<List<CategoryModelView>> GetCategories()
        {
            // get categories
            HttpClient hcCategories = APIHelper.GetHttpClient(APIHelper.SA + "/Category");
            hcCategories.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetCategories = hcCategories.GetAsync("Category");
            apiGetCategories.Wait();

            var dataCategories = apiGetCategories.Result;
            System.Diagnostics.Debug.WriteLine(dataCategories.StatusCode);
            if (dataCategories.IsSuccessStatusCode)
            {
                var displayDataRegion = dataCategories.Content.ReadAsAsync<ResponseWithData<List<CategoryModelView>>>();
                displayDataRegion.Wait();
                return new ResponseWithData<List<CategoryModelView>>()
                {
                    StatusCode = displayDataRegion.Result.StatusCode,
                    Message = displayDataRegion.Result.Message,
                    Data = displayDataRegion.Result.StatusCode == HttpStatusCode.OK ? displayDataRegion.Result.Data : null
                };
            }
            else
            {
                return new ResponseWithData<List<CategoryModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = null
                };
            }
        }

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