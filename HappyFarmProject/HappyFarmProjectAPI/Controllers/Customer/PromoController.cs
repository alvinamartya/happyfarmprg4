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
    public class PromoController : ApiController
    {
        #region Variable
        // repo
        private PromoRepository repo = new PromoRepository();
        #endregion

        #region Action
        /// <summary>
        /// To get promoes
        /// </summary>
        /// <returns></returns>
        [Route("api/v1/Promo")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPromos()
        {
            try
            {
                List<Promo> promos = await Task.Run(() => repo.GetPromosByDate());

                // response success
                var response = new ResponseWithData<Object>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Berhasil",
                    Data = promos
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Image,
                            x.Code,
                            x.MinTransaction,
                            x.MaxDiscount,
                            x.IsFreeDelivery,
                            x.StartDate,
                            x.EndDate
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
