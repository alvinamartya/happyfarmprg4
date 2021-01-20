using HappyFarmProjectAPI.Controllers.BusinessLogic;
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
    public class ProductionAdminPurchasingController : ApiController
    {
        #region Variable
        // logic
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private PurchasingRepository repo = new PurchasingRepository();
        #endregion

        #region Action
        /// <summary>
        /// To get purchasing history by production admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/PA/Purchasing/{employeeId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetHistoryPurchasing(int employeeId)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                {
                    // get goods by id
                    List<Purchasing> purchasing = await Task.Run(() => repo.GetPurchasingHistory(employeeId));

                    // response success
                    var response = new ResponseWithData<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = purchasing
                        .Select(x => new
                        {
                            x.Id,
                            x.DateTime,
                            x.FarmerName,
                            x.FarmerAddress,
                            x.FarmerPhone,
                            x.TotalPurchasePrice
                        })
                        .OrderByDescending(x => x.DateTime)
                        .ToList()
                    };

                    return Ok(response);
                }
                else
                {
                    // unauthorized
                    var unAuthorizedResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "Anda tidak memiliki hak akses"
                    };

                    return Ok(unAuthorizedResponse);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Add Purchasing
        /// </summary>
        /// <param name="purchasingRequest"></param>
        /// <returns></returns>
        [Route("api/v1/PA/Purchasing/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPurchasing(AddPurchasingRequest purchasingRequest)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                {
                    // get goods by id
                    await Task.Run(() => repo.AddPurchasing(purchasingRequest));

                    // response success
                    var response = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.Created,
                        Message = "Berhasil"
                    };

                    return Ok(response);
                }
                else
                {
                    // unauthorized
                    var unAuthorizedResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "Anda tidak memiliki hak akses"
                    };

                    return Ok(unAuthorizedResponse);
                }
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
