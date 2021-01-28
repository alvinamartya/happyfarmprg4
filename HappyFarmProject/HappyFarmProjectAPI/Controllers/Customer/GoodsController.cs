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
    public class GoodsController : ApiController
    {
        #region Variable
        // repo
        private GoodsRepository repo = new GoodsRepository();
        #endregion

        #region Action
        [Route("api/v1/Goods/{CategoryId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGoods(int CategoryId)
        {
            try
            {
                // get employee by id
                List<Good> listGoods = await Task.Run(() => repo.GetGoodsByCategoryId(CategoryId));

                // response success
                var response = new ResponseWithData<Object>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Berhasil",
                    Data = listGoods
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Description,
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
