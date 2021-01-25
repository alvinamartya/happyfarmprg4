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
    public class BannerController : ApiController
    {
        #region Variable
        // repo
        private BannerRepository repo = new BannerRepository();
        #endregion

        #region Action
        /// <summary>
        /// To get promoes
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/Banner")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPromos()
        {
            try
            {
                List<Banner> banners = await Task.Run(() => repo.GetBanners());

                // response success
                var response = new ResponseWithData<Object>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Berhasil",
                    Data = banners
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Image
                        })
                        .ToList(),
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
