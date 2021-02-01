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
    public class PurchasingController : ApiController
    {
        #region Variable
        private PurchasingRepository repo = new PurchasingRepository();
        #endregion
        #region Action
        [Route("api/v1/Pembelian")]
        [HttpPost]
        public async Task<IHttpActionResult> Purchase(CustomerPurchasingRequest request)
        {
            try
            {
                await Task.Run(() => repo.CustomerPurchasing(request));

                var response = new ResponseWithoutData()
                {
                    Message = "Berhasil",
                    StatusCode = HttpStatusCode.OK
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
