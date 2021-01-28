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
    public class ProductController : Controller
    {
        #region Variable
        #endregion

        // GET: Product
        #region Action
        public ActionResult Index(int? Category)
        {
            ResponseWithData<List<Category>> categoryRequest = GetCategories();
            if (categoryRequest.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.Categories = new SelectList(categoryRequest.Data, "Id", "Name");
            }
            else
            {
                TempData["ErrMessage"] = categoryRequest.Message;
                TempData["ErrHeader"] = "Gagal meload category";
            }

            System.Diagnostics.Debug.WriteLine(Category.ToString());

            ViewBag.CategoryId = Category ?? categoryRequest.Data[0].Id;

            return View();
        }
        #endregion

        #region Request Data
        public ResponseWithData<List<Category>> GetCategories()
        {
            // get categories
            HttpClient hcCategoryGet = APIHelper.GetHttpClient("Category");

            var apiGet = hcCategoryGet.GetAsync("Category");
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponseWithData<List<Category>>>();
                displayData.Wait();

                return new ResponseWithData<List<Category>>()
                {
                    StatusCode = displayData.Result.StatusCode,
                    Message = displayData.Result.Message,
                    Data = displayData.Result.StatusCode == HttpStatusCode.OK ? displayData.Result.Data : new List<Category>()
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
        #endregion
    }
}