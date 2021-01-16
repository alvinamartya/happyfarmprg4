using HappyFarmProjectWebAdmin.Models;
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
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortName = Sorting_Order == "Name_Desc" ? "Name_Asc" : "Name_Desc";
            ViewBag.SortPromo = Sorting_Order == "Promo_Desc" ? "Promo_Asc" : "Promo_Desc";

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

            // sorting list
            switch(Sorting_Order)
            {
                case "Name_Desc":
                    bannerRequest.Data = bannerRequest.Data.OrderByDescending(x => x.Name).ToList();
                    break;
                case "Name_Asc":
                    bannerRequest.Data = bannerRequest.Data.OrderBy(x => x.Name).ToList();
                    break;
                case "Promo_Desc":
                    bannerRequest.Data = bannerRequest.Data.OrderByDescending(x => x.PromoName).ToList();
                    break;
                case "Promo_Asc":
                    bannerRequest.Data = bannerRequest.Data.OrderBy(x => x.PromoName).ToList();
                    break;
            }

            int sizeOfPage = 4;
            int noOfPage = (Page_No ?? 1);
            IndexModelView<IPagedList<BannerModelView>> indexViewModel = new IndexModelView<IPagedList<BannerModelView>>()
            {
                DataPaging = dataPaging,
                ModelViews = bannerRequest.Data.ToPagedList(noOfPage, sizeOfPage)
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
        #region Add Banner
        // Add Banner
        [Route("~/SA/Banner/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            // get promos
            ResponseWithData<List<PromoListModelView>> categoriesRequest = GetPromos();
            if (categoriesRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Promos = new SelectList(categoriesRequest.Data, "Id", "Name");
            }
            else if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal Meload Promo";
            }

            return View();
        }

        [Route("~/SA/Banner/Tambah")]
        [HttpPost]
        public ActionResult Add(ProcessBannerModelView bannerModelView)
        {
            // get promos
            ResponseWithData<List<PromoListModelView>> categoriesRequest = GetPromos();
            if (categoriesRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Promos = new SelectList(categoriesRequest.Data, "Id", "Name");
            }
            else if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal Meload Promo";
            }

            // add created by
            AddBannerRequest bannerRequest = new AddBannerRequest()
            {
                Name = bannerModelView.Name,
                CreatedBy = (int)Session["UserId"],
                PromoId = bannerModelView.PromoId == 0 ? null : bannerModelView.PromoId
            };

            //
            if (bannerModelView.HiddenFileName == null && bannerModelView.Image != null)
            {
                // decrypt file name
                Guid uid = Guid.NewGuid();
                var guidFileName = uid.ToString() + Path.GetExtension(bannerModelView.Image.FileName);

                // save to server
                var filePath = Server.MapPath("~/Images/Banner/" + guidFileName);
                bannerModelView.Image.SaveAs(filePath);

                // set to hidden file
                bannerModelView.HiddenFileName = guidFileName;
                bannerModelView.OriginalFileName = bannerModelView.Image.FileName;
            }

            //
            if (bannerModelView.HiddenFileName != null)
            {
                // set path from hidden file
                bannerRequest.ImagePath = bannerModelView.HiddenFileName;

                // insert data using API
                hcBannerAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
                var apiSave = hcBannerAdd.PostAsJsonAsync<AddBannerRequest>("Add", bannerRequest);
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
                        TempData["ErrHeader"] = "Gagal Menambah Banner";
                    }
                }
                else
                {
                    TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                    TempData["ErrHeader"] = "Gagal Menambah Banner";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Gambar belum diupload";
                TempData["ErrHeader"] = "Gagal Menambah Banner";
            }

            return View(bannerModelView);
        }
        #endregion
        #region Edit Banner
        [Route("~/SA/Banner/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProcessBannerModelView processBannerModelView = new ProcessBannerModelView();

            // get promos
            ResponseWithData<List<PromoListModelView>> categoriesRequest = GetPromos();
            if (categoriesRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Promos = new SelectList(categoriesRequest.Data, "Id", "Name");
            }
            else if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal Meload Promo";
            }

            // get goods
            HttpClient hcBannerGet = APIHelper.GetHttpClient(APIHelper.SA + "/Banner");
            hcBannerGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcBannerGet.GetAsync("Banner/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<BannerModelView>>();
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
                    BannerModelView bannerModelView = displayData.Result.Data;
                    processBannerModelView.Name = bannerModelView.Name;
                    processBannerModelView.PromoId = bannerModelView.PromoId;
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            return View(processBannerModelView);
        }

        [Route("~/SA/Banner/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, ProcessBannerModelView bannerModelView)
        {
            // get promos
            ResponseWithData<List<PromoListModelView>> categoriesRequest = GetPromos();
            if (categoriesRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Promos = new SelectList(categoriesRequest.Data, "Id", "Name");
            }
            else if (categoriesRequest.StatusCode == HttpStatusCode.Unauthorized)
            {
                Session["ErrMessage"] = categoriesRequest.Message;
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["ErrMessage"] = categoriesRequest.Message;
                TempData["ErrHeader"] = "Gagal Meload Promo";
            }

            //
            if (bannerModelView.HiddenFileName == null && bannerModelView.Image != null)
            {
                // decrypt file name
                Guid uid = Guid.NewGuid();
                var guidFileName = uid.ToString() + Path.GetExtension(bannerModelView.Image.FileName);

                // save to server
                var filePath = Server.MapPath("~/Images/Banner/" + guidFileName);
                bannerModelView.Image.SaveAs(filePath);

                // set to hidden file
                bannerModelView.HiddenFileName = guidFileName;
                bannerModelView.OriginalFileName = bannerModelView.Image.FileName;
            }

            // create edit goods request
            EditBannerRequest bannerRequest = new EditBannerRequest()
            {
                ImagePath = bannerModelView.HiddenFileName,
                ModifiedBy = (int)Session["UserId"],
                Name = bannerModelView.Name,
                PromoId = bannerModelView.PromoId == 0 ? null : bannerModelView.PromoId
            };

            System.Diagnostics.Debug.WriteLine(bannerRequest.PromoId);

            // update goods
            hcBannerEdit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcBannerEdit.PutAsJsonAsync<EditBannerRequest>("Edit/" + id, bannerRequest);
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
                    TempData["ErrHeader"] = "Gagal Mengubah Data Banner";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Banner";
            }

            return View(bannerModelView);
        }
        #endregion
        #region Request Data
        public ResponseWithData<List<PromoListModelView>> GetPromos()
        {
            // get categories
            HttpClient hcCategories = APIHelper.GetHttpClient(APIHelper.SA + "/Promo");
            hcCategories.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiGetCategories = hcCategories.GetAsync("Promo");
            apiGetCategories.Wait();

            // create new list of promo list model view
            List<PromoListModelView> promoListModelViews = new List<PromoListModelView>()
            {
                new PromoListModelView()
                {
                    ID = 0,
                    Name = "-"
                }
            };

            var dataCategories = apiGetCategories.Result;
            System.Diagnostics.Debug.WriteLine(dataCategories.StatusCode);
            if (dataCategories.IsSuccessStatusCode)
            {
                var displayDataRegion = dataCategories.Content.ReadAsAsync<ResponseWithData<List<PromoModelView>>>();
                displayDataRegion.Wait();

                foreach (PromoModelView item in displayDataRegion.Result.Data)
                {
                    promoListModelViews.Add(new PromoListModelView()
                    {
                        ID = item.Id,
                        Name = item.Name
                    });
                }

                return new ResponseWithData<List<PromoListModelView>>()
                {
                    StatusCode = displayDataRegion.Result.StatusCode,
                    Message = displayDataRegion.Result.Message,
                    Data = displayDataRegion.Result.StatusCode == HttpStatusCode.OK ? promoListModelViews : null
                };
            }
            else
            {
                return new ResponseWithData<List<PromoListModelView>>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = "Terjadi kesalahan pada sistem",
                    Data = promoListModelViews
                };
            }
        }
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