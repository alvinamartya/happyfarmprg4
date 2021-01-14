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
        #region Add Promo
        // Add Promo
        [Route("~/SA/Promo/Tambah")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [Route("~/SA/Promo/Tambah")]
        [HttpPost]
        public ActionResult Add(ProcessPromoModelView promosModelView)
        {

            // add created by
            AddPromoRequest promosRequest = new AddPromoRequest()
            {
                Name = promosModelView.Name,
                CreatedBy = (int)Session["UserId"],
                Discount = (float)promosModelView.Discount,
                EndDate = promosModelView.EndDate,
                IsFreeDelivery = promosModelView.IsFreeDelivery,
                MaxDiscount = promosModelView.MaxDiscount,
                MinTransaction = promosModelView.MinTransaction,
                StartDate = promosModelView.StartDate
            };

            //
            if (promosModelView.HiddenFileName == null && promosModelView.Image != null)
            {
                // decrypt file name
                Guid uid = Guid.NewGuid();
                var guidFileName = uid.ToString() + Path.GetExtension(promosModelView.Image.FileName);

                // save to server
                var filePath = Server.MapPath("~/Images/Promo/" + guidFileName);
                promosModelView.Image.SaveAs(filePath);

                // set to hidden file
                promosModelView.HiddenFileName = guidFileName;
                promosModelView.OriginalFileName = promosModelView.Image.FileName;
            }

            //
            if (promosModelView.HiddenFileName != null)
            {
                // set path from hidden file
                promosRequest.ImagePath = promosModelView.HiddenFileName;

                // insert data using API
                hcPromoAdd.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
                var apiSave = hcPromoAdd.PostAsJsonAsync<AddPromoRequest>("Add", promosRequest);
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
                        TempData["ErrHeader"] = "Gagal Menambah Promo";
                    }
                }
                else
                {
                    TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                    TempData["ErrHeader"] = "Gagal Menambah Promo";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Gambar belum diupload";
                TempData["ErrHeader"] = "Gagal Menambah Promo";
            }

            return View(promosModelView);
        }
        #endregion
        #region Edit Promo
        [Route("~/SA/Promo/Ubah/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProcessPromoModelView processPromoModelView = new ProcessPromoModelView();

            // get promo
            HttpClient hcPromoGet = APIHelper.GetHttpClient(APIHelper.SA + "/Promo");
            hcPromoGet.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hcPromoGet.GetAsync("Promo/" + id);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<PromoModelView>>();
                displayData.Wait();

                if (displayData.Result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Session["ErrMessage"] = displayData.Result.Message;
                    return RedirectToAction("Index", "Login");
                }
                else if (displayData.Result.StatusCode != HttpStatusCode.OK)
                {
                    TempData["ErrMessage"] = displayData.Result.Message;
                    TempData["ErrHeader"] = "Gagal meload data promo";
                }
                else
                {
                    PromoModelView promoModeView = displayData.Result.Data;
                    processPromoModelView.Name = promoModeView.Name;
                    processPromoModelView.StartDate = promoModeView.StartDate;
                    processPromoModelView.EndDate = promoModeView.EndDate;
                    processPromoModelView.IsFreeDelivery = promoModeView.IsFreeDelivery;
                    processPromoModelView.Discount = promoModeView.Discount;
                    processPromoModelView.MinTransaction = (long)promoModeView.MinTransaction;
                    processPromoModelView.MaxDiscount = (long)promoModeView.MaxDiscount;

                    System.Diagnostics.Debug.WriteLine(processPromoModelView.MinTransaction);
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal meload data";
            }

            return View(processPromoModelView);
        }

        [Route("~/SA/Promo/Ubah/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, ProcessPromoModelView promoModelView)
        {
            System.Diagnostics.Debug.WriteLine(promoModelView.Discount);

            //
            if (promoModelView.HiddenFileName == null && promoModelView.Image != null)
            {
                // decrypt file name
                Guid uid = Guid.NewGuid();
                var guidFileName = uid.ToString() + Path.GetExtension(promoModelView.Image.FileName);

                // save to server
                var filePath = Server.MapPath("~/Images/Promo/" + guidFileName);
                promoModelView.Image.SaveAs(filePath);

                // set to hidden file
                promoModelView.HiddenFileName = guidFileName;
                promoModelView.OriginalFileName = promoModelView.Image.FileName;
            }

            // create edit goods request
            EditPromoRequest promoRequest = new EditPromoRequest()
            {
                ImagePath = promoModelView.HiddenFileName,
                ModifiedBy = (int)Session["UserId"],
                Name = promoModelView.Name,
                Discount = (float)promoModelView.Discount,
                IsFreeDelivery = promoModelView.IsFreeDelivery,
                EndDate = promoModelView.EndDate,
                MaxDiscount = promoModelView.MaxDiscount,
                MinTransaction = promoModelView.MinTransaction,
                StartDate = promoModelView.StartDate,
            };

            // update goods
            hcPromodit.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);
            var apiEdit = hcPromodit.PutAsJsonAsync<EditPromoRequest>("Edit/" + id, promoRequest);
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
                    TempData["ErrHeader"] = "Gagal Mengubah Data Promo";
                }
            }
            else
            {
                TempData["ErrMessage"] = "Terjadi kesalahan pada sistem";
                TempData["ErrHeader"] = "Gagal Mengubah Data Promo";
            }

            return View(promoModelView);
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