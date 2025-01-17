﻿using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace HappyFarmProjectWebAdmin.Controllers
{
    public class ProductionAdminGoodsController : Controller
    {
        #region Variable
        HttpClient hcGoodsAdd = APIHelper.GetHttpClient(APIHelper.PA + "/Goods/Add");
        HttpClient hcGoodsDelete = APIHelper.GetHttpClient(APIHelper.PA + "/Goods/Delete");
        HttpClient hcGoodsEdit = APIHelper.GetHttpClient(APIHelper.PA + "/Goods/Edit");
        #endregion
        #region GetGoods
        // GET Goods
        [Route("~/PA/Produk")]
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
            ViewBag.CurrentSortOder = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "Name_Desc" ? "Name_Asc" : "Name_Desc";
            ViewBag.SortingCategory = Sorting_Order == "Category_Desc" ? "Category_Asc" : "Category_Desc";
            ViewBag.SortingDescription = Sorting_Order == "Description_Desc" ? "Description_Asc" : "Description_Desc";

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

            // sorting
            switch (Sorting_Order)
            {
                case "Name_Desc":
                    goodsRequest.Data = goodsRequest.Data.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Name_Asc":
                    goodsRequest.Data = goodsRequest.Data.OrderBy(x => x.Name).ToList();
                    break;
                case "Category_Desc":
                    goodsRequest.Data = goodsRequest.Data.OrderByDescending(x => x.CategoryName).ToList();
                    break;
                case "Category_Asc":
                    goodsRequest.Data = goodsRequest.Data.OrderBy(x => x.CategoryName).ToList();
                    break;
                case "Description_Desc":
                    goodsRequest.Data = goodsRequest.Data.OrderByDescending(x => x.Description).ToList();
                    break;
                case "Description_Asc":
                    goodsRequest.Data = goodsRequest.Data.OrderBy(x => x.Description).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<GoodsModelView>> indexViewModel = new IndexModelView<IPagedList<GoodsModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = goodsRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }

        [Route("~/PA/Produk")]
        [HttpPost]
        public ActionResult Index(IndexModelView<IEnumerable<GoodsModelView>> indexGoods, string Sorting_Order, int? Page_No)
        {
            System.Diagnostics.Debug.Write(indexGoods.DataPaging.Search);
            if (Session["ErrMessage"] != null)
            {
                TempData["ErrMessage"] = Session["ErrMessage"];
                TempData["ErrHeader"] = Session["ErrHeader"];

                Session["ErrMessage"] = null;
                Session["ErrHeader"] = null;
            }

            // sorting state
            ViewBag.CurrentSortOder = Sorting_Order;
            ViewBag.SortingName = Sorting_Order == "Name_Desc" ? "Name_Asc" : "Name_Desc";
            ViewBag.SortingCategory = Sorting_Order == "Category_Desc" ? "Category_Asc" : "Category_Desc";
            ViewBag.SortingDescription = Sorting_Order == "Description_Desc" ? "Description_Asc" : "Description_Desc";

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

            // sorting
            switch (Sorting_Order)
            {
                case "Name_Desc":
                    goodsRequest.Data = goodsRequest.Data.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Name_Asc":
                    goodsRequest.Data = goodsRequest.Data.OrderBy(x => x.Name).ToList();
                    break;
                case "Category_Desc":
                    goodsRequest.Data = goodsRequest.Data.OrderByDescending(x => x.CategoryName).ToList();
                    break;
                case "Category_Asc":
                    goodsRequest.Data = goodsRequest.Data.OrderBy(x => x.CategoryName).ToList();
                    break;
                case "Description_Desc":
                    goodsRequest.Data = goodsRequest.Data.OrderByDescending(x => x.Description).ToList();
                    break;
                case "Description_Asc":
                    goodsRequest.Data = goodsRequest.Data.OrderBy(x => x.Description).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<GoodsModelView>> indexViewModel = new IndexModelView<IPagedList<GoodsModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = goodsRequest.Data.ToPagedList(noOfPage, sizeOfPage)
            };

            return View(indexViewModel);
        }
        #endregion
        #region Delete Goods
        [Route("~/PA/Produk/Hapus")]
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
        // Add Banner
        [Route("~/PA/Produk/Tambah")]
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

            return View();
        }

        [Route("~/PA/Produk/Tambah")]
        [HttpPost]
        public ActionResult Add(ProcessGoodsModelView goodsModelView)
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

            // add created by
            AddGoodsRequest goodsRequest = new AddGoodsRequest()
            {
                CategoryId = goodsModelView.CategoryId,
                Description = goodsModelView.Description,
                Name = goodsModelView.Name,
                CreatedBy = (int)Session["UserId"]
            };

            //
            if (goodsModelView.HiddenFileName == null && goodsModelView.Image != null)
            {
                // decrypt file name
                Guid uid = Guid.NewGuid();
                var guidFileName = uid.ToString() + Path.GetExtension(goodsModelView.Image.FileName);

                // save to server
                var filePath = Server.MapPath("~/Images/Goods/" + guidFileName);
                goodsModelView.Image.SaveAs(filePath);

                // set to hidden file
                goodsModelView.HiddenFileName = guidFileName;
                goodsModelView.OriginalFileName = goodsModelView.Image.FileName;
            }

            //
            if (goodsModelView.HiddenFileName != null)
            {
                // set path from hidden file
                goodsRequest.ImagePath = goodsModelView.HiddenFileName;

                // insert data using API
                hcGoodsAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
                var apiSave = hcGoodsAdd.PostAsJsonAsync<AddGoodsRequest>("Add", goodsRequest);
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
                        TempData["ErrHeader"] = "Gagal Menambah Produk";
                    }
                }
                else
                {
                    TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                    TempData["ErrHeader"] = "Gagal Menambah Produk";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Gambar belum diupload";
                TempData["ErrHeader"] = "Gagal Menambah Produk";
            }

            return View(goodsModelView);
        }
        #endregion
        #region Edit Goods
        [Route("~/PA/Produk/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProcessGoodsModelView processGoodsModelView = new ProcessGoodsModelView();

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

            // get goods
            HttpClient hcGoodsGet = APIHelper.GetHttpClient(APIHelper.PA + "/Goods");
            hcGoodsGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcGoodsGet.GetAsync("Goods/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<GoodsModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data produk";
                }
                else
                {
                    GoodsModelView goodsModelView = displayData.Result.Data;
                    processGoodsModelView.Name = goodsModelView.Name;
                    processGoodsModelView.CategoryId = goodsModelView.CategoryId;
                    processGoodsModelView.Description = goodsModelView.Description;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            return View(processGoodsModelView);
        }

        [Route("~/PA/Produk/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, ProcessGoodsModelView goodsModelView)
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

            //
            if (goodsModelView.HiddenFileName == null && goodsModelView.Image != null)
            {
                // decrypt file name
                Guid uid = Guid.NewGuid();
                var guidFileName = uid.ToString() + Path.GetExtension(goodsModelView.Image.FileName);

                // save to server
                var filePath = Server.MapPath("~/Images/Goods/" + guidFileName);
                goodsModelView.Image.SaveAs(filePath);

                // set to hidden file
                goodsModelView.HiddenFileName = guidFileName;
                goodsModelView.OriginalFileName = goodsModelView.Image.FileName;
            }

            // create edit goods request
            EditGoodsRequest goodsRequest = new EditGoodsRequest()
            {
                CategoryId = goodsModelView.CategoryId,
                Description = goodsModelView.Description,
                ImagePath = goodsModelView.HiddenFileName,
                ModifiedBy = (int)Session["UserId"],
                Name = goodsModelView.Name
            };

            // update goods
            hcGoodsEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcGoodsEdit.PutAsJsonAsync<EditGoodsRequest>("Edit/" + id, goodsRequest);
            apiEdit.Wait();

            var dateEdit = apiEdit.Result;
            if (dateEdit.IsSuccessStatusCode)
            {
                var displayDataEdit = dateEdit.Content.ReadAsAsync<ResponseWithoutData>();
                displayDataEdit.Wait();
                if (displayDataEdit.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayDataEdit.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayDataEdit.Result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrMessage"] = displayDataEdit.Result.Message;
                    TempData["ErrHeader"] = "Gagal Mengubah Data Produk";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Produk";
            }

            return View(goodsModelView);
        }
        #endregion
        #region Request Data
        public ResponseWithData<List<CategoryModelView>> GetCategories()
        {
            // get categories
            HttpClient hcCategories = APIHelper.GetHttpClient(APIHelper.PA + "/Category");
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
                    Data = new List<CategoryModelView>()
                };
            }
        }

        public ResponseDataWithPaging<List<GoodsModelView>> GetGoods(GetListDataRequest dataPaging)
        {
            // get goods
            HttpClient hcGoodsGet = APIHelper.GetHttpClient(APIHelper.PA + "/Goods");
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