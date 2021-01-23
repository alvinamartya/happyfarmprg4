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
    public class SalesAdminSellingStatusController : ApiController
    {
        #region Variable
        // logic
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private SellingStatusRepository repo = new SellingStatusRepository();
        #endregion
        #region Action
        /// <summary>
        /// To get selling status
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/SALA/SellingStatus")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSellingStatus()
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Penjualan"))
                {
                    // get employee by id
                    List<SellingStatu> sellingStatus = await Task.Run(() => repo.GetSellingStatus());

                    // response success
                    var response = new ResponseWithData<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = sellingStatus
                            .Select(x => new
                            {
                                x.Id,
                                x.Name
                            })
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
        #endregion
    }
}
