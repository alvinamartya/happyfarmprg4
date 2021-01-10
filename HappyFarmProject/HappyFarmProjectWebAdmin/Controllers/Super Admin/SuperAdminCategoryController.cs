using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Net;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class SuperAdminCategoryController : Controller
    {
        #region Variable
        HttpClient hcCategoryAdd = APIHelper.GetHttpClient(APIHelper.SA + "/Category/Add");
        HttpClient hcCategoryDelete = APIHelper.GetHttpClient(APIHelper.SA + "/Category/Delete");
        HttpClient hcCategoryEdit = APIHelper.GetHttpClient(APIHelper.SA + "/Category/Edit");
        #endregion

        #region GetCategories
        // GET Categories
        [Route("~/SA/Kategori")]
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

            ResponseDataWithPaging<List<CategoryModelView>> categoriesRequest = GetCategories(dataPaging);
            ViewBag.CurrentPage = categoriesRequest.CurrentPage;
            ViewBag.TotalPage = categoriesRequest.TotalPage;

            // status code
            if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (categoriesRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }


            // data is empty
            if (categoriesRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexCategoryModelView indexViewModel = new IndexCategoryModelView()
            {
                DataPaging = dataPaging,
                CategoryModelViews = categoriesRequest.Data
            };

            return View(indexViewModel);
        }

        [Route("~/SA/Kategori")]
        [HttpPost]
        public ActionResult Index(IndexRegionModelView indexRegion)
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
                Search = indexRegion.DataPaging.Search
            };

            ResponseDataWithPaging<List<CategoryModelView>> categoriesRequest = GetCategories(dataPaging);
            ViewBag.CurrentPage = categoriesRequest.CurrentPage;
            ViewBag.TotalPage = categoriesRequest.TotalPage;

            // status code
            if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else if (categoriesRequest.StatusCode != HttpStatusCode.OK)
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal meload data";
            }

            // data is empty
            if (categoriesRequest.Data.Count == 0)
            {
                TempData["ErrMessageData"] = "Data belum tersedia";
            }

            IndexCategoryModelView indexViewModel = new IndexCategoryModelView()
            {
                DataPaging = dataPaging,
                CategoryModelViews = categoriesRequest.Data
            };

            return View(indexViewModel);
        }
        #endregion

        #region Add Category
        // Add Region
        [Route("~/SA/Kategori/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Route("~/SA/Kategori/Tambah")]
        [HttpPost]
        public ActionResult Add(AddCategoryRequest categoryRequest)
        {
            // add created by
            categoryRequest.CreatedBy = (int)Session["UserId"];

            // insert data using API
            hcCategoryAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiSave = hcCategoryAdd.PostAsJsonAsync<AddCategoryRequest>("Add", categoryRequest);
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
                    TempData["ErrHeader"] = "Gagal Menambah Kategori";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Menambah Kategori";
            }
            return View();
        }
        #endregion

        #region Delete Category
        [Route("~/SA/Kategori/Hapus")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            hcCategoryDelete.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiDelete = hcCategoryDelete.DeleteAsync("Delete/" + id);
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
                    Session["ErrMessage"] = "Berhasil menghapus kategori";
                    Session["ErrHeader"] = "Berhasil";
                }
                else
                {
                    Session["ErrMessage"] = displayDataDelete.Result.Message;
                    Session["ErrHeader"] = "Gagal menghapus kategori";
                }
            }
            else
            {
                Session["ErrMessage"] = "Terjadi kesalahan pada sistem";
                Session["ErrHeader"] = "Gagal menghapus kategori";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit Kategori
        [Route("~/SA/Kategori/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            CategoryModelView category = null;

            // get category
            HttpClient hcCategoryGet = APIHelper.GetHttpClient(APIHelper.SA + "/Category");
            hcCategoryGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcCategoryGet.GetAsync("Category/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<CategoryModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data wilayah";
                }
                else
                {
                    category = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditCategoryRequest editCategory = new EditCategoryRequest()
            {
                ModifiedBy = (int)Session["UserId"],
                Name = category.Name
            };

            return View(editCategory);
        }

        [Route("~/SA/Kategori/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, EditCategoryRequest categoryRequest)
        {
            RegionModelView region = null;

            // get category
            HttpClient hcCategoryGet = APIHelper.GetHttpClient(APIHelper.SA + "/Category");
            hcCategoryGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcCategoryGet.GetAsync("Category/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<RegionModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data wilayah";
                }
                else
                {
                    region = displayData.Result.Data;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            EditCategoryRequest editCategory = new EditCategoryRequest()
            {
                Name = region.Name
            };

            // add modified by
            categoryRequest.ModifiedBy = (int)Session["UserId"];

            // update region
            hcCategoryEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcCategoryEdit.PutAsJsonAsync<EditCategoryRequest>("Edit/" + id, categoryRequest);
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
                    TempData["ErrHeader"] = "Gagal Mengubah Data Kategori";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Kategori";
            }

            return View(editCategory);
        }
        #endregion

        #region Request Data
        public ResponseDataWithPaging<List<CategoryModelView>> GetCategories(GetListDataRequest dataPaging)
        {
            // get categories
            HttpClient hcCategoryGet = APIHelper.GetHttpClient(APIHelper.SA + "/Category");
            hcCategoryGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcCategoryGet.PostAsJsonAsync<GetListDataRequest>("Category", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseDataWithPaging<List<CategoryModelView>>>();
                displayData.Wait();

                return new ResponseDataWithPaging<List<CategoryModelView>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<CategoryModelView>(),
                    CurrentPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.CurrentPage : 0,
                    TotalPage = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.TotalPage : 0
                };
            }
            else
            {
                return new ResponseDataWithPaging<List<CategoryModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = new List<CategoryModelView>(),
                    CurrentPage = 0,
                    TotalPage = 0
                };
            }
        }
        #endregion
    }
}