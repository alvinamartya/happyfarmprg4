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
    public class HistoryTransactionController : ApiController
    {
        #region Variable
        private SellingRepository repo = new SellingRepository();
        #endregion

        #region Action
        [Route("api/v1/HistoryTransaction/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetHistoryTransaction(int id)
        {
            try
            {
                List<Selling> promos = await Task.Run(() => repo.GetHistoryTransaction(id));

                // response success
                using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    var response = new ResponseWithData<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = promos
                       .Select(x => new
                       {
                           x.Id,
                           x.DateTime,
                           x.TotalSalePrice,
                           x.ShippingCharges,
                           LastSellingActivity = db.SellingActivities.Where(z=>z.SellingId == x.Id).OrderByDescending(z=>z.Id).FirstOrDefault().SellingStatu.Name
                       })
                       .ToList(),
                    };

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [Route("api/v1/UploadBuktiTransfer")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadBuktiTransfer(UploadBuktiTransfer request)
        {
            try
            {
                await Task.Run(() => repo.UploadBuktiTransfer(request));

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

        [Route("api/v1/SellingDetail/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSellingDetail(int id)
        {
            try
            {
                List<SellingDetail> promos = await Task.Run(() => repo.GetSellingDetail(id));

                // response success
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    var response = new ResponseWithData<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = promos
                       .Select(x => new
                       {
                           x.Id,
                           x.GoodsId,
                           GoodsName = db.Goods.Where(z=> z.Id == x.GoodsId).FirstOrDefault().Name,
                           x.Qty,
                           x.GoodsPrice
                       })
                       .ToList(),
                    };

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        [Route("api/v1/Feedback")]
        [HttpPost]
        public async Task<IHttpActionResult> CustomerFeedback(CreateCustomerFeedback customerFeedback)
        {
            try
            {
                if(customerFeedback.Rating > 5 || customerFeedback.Rating < 0)
                {
                    var ratingResponse = new ResponseWithoutData()
                    {
                        Message = "Rating harus diantara 1 dan 5",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    return Ok(ratingResponse);
                }

                await Task.Run(() => repo.CreateCustomerFeedback(customerFeedback));

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
