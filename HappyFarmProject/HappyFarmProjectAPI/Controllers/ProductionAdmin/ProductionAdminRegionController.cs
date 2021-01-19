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
    public class ProductionAdminRegionController : ApiController
    {
        #region Variable
        // logic
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private RegionRepository repo = new RegionRepository();
        #endregion

        #region GetGoods
        /// <summary>
        /// To get list goods
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/PA/Regions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGoods()
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                {
                    List<Region> regions = await Task.Run(() => repo.GetRegions());

                    // response success
                    var response = new ResponseWithData<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = regions
                            .Select(x => new
                            {
                                x.Id,
                                x.Name
                            })
                            .ToList(),
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
