using HappyFarmProjectAPI.Controllers.Repository;
using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HappyFarmProjectAPI.Controllers
{
    public class DashboardController : ApiController
    {
        #region Variable
        private DashboardRepository repo = new DashboardRepository();
        #endregion
        #region Action
        [Route("api/v1/Dashboard/{month}/{year}")]
        [HttpGet]
        public async Task<IHttpActionResult> DashboardSelling(int month, int year)
        {
            try
            {
                List<Dashboard> dashboards = new List<Dashboard>();
                List<DashboardModel> sellings = await Task.Run(() => repo.GetSumSelling(month, year));
                List<DashboardModel> purchasing = await Task.Run(() => repo.GetSumPurchasing(month, year));

                foreach (var x in sellings)
                {
                    dashboards.Add(new Dashboard() { Date = x.Date, TotalSale = x.Total, TotalPurchase = 0 });
                    System.Diagnostics.Debug.WriteLine(x.Date.ToString() + " : " + x.Total);
                }

                foreach(var x in purchasing)
                {
                    Dashboard dashboard = dashboards.Where(z => z.Date == x.Date).FirstOrDefault();
                    if(dashboard != null)
                    {
                        dashboard.TotalPurchase = x.Total;
                    }
                    else
                    {
                        dashboards.Add(new Dashboard()
                        {
                            Date = x.Date,
                            TotalSale = 0,
                            TotalPurchase = x.Total
                        });
                    }
                }

                var response = new ResponseWithData<IEnumerable<Object>>()
                {
                    Message = "Berhasil",
                    StatusCode = HttpStatusCode.OK,
                    Data = dashboards.OrderBy(x => x.Date).ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
