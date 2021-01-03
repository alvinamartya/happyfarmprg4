using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class SuperAdminEmployeeController : Controller
    {
        #region Variable
        HttpClient hc = APIHelper.GetHttpClient(APIHelper.SA + "/Employee");
        #endregion

        // GET: SuperAdminEmployee
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("~/SA/Karyawan")]
        [HttpGet]
        public ActionResult Index()
        {
            List<EmployeeModelView> employees = null;

            // default request paging
            var dataPaging = new GetListDataRequest()
            {
                CurrentPage = 1,
                LimitPage = 10,
                Search = ""
            };

            hc.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["Token"]);

            var apiGet = hc.PostAsJsonAsync<GetListDataRequest>("Employee", dataPaging);
            apiGet.Wait();

            var data = apiGet.Result;
            if (data.IsSuccessStatusCode)
            {
                var displayData = data.Content.ReadAsAsync<ResponsWithData<List<EmployeeModelView>>>();
                displayData.Wait();

                employees = displayData.Result.Data;
            }

            return View(employees);
        }
    }
}