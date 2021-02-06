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
                List<Selling> sellings = await Task.Run(() => repo.GetHistoryTransaction(id));

                // response success
                using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    int totalSale = 0;
                    List<HistorySellingModel> historiesSelling = new List<HistorySellingModel>();

                    foreach (Selling sale in sellings)
                    {
                        int discount = 0;

                        if (sale.PromoId != null)
                        {
                            Promo promo = db.Promoes.Where(x => x.Id == sale.PromoId).FirstOrDefault();
                            discount = promo.Discount > 0 ? (int)((double)sale.TotalSalePrice * promo.Discount / 100) : 0;
                         }

                        // calculate total sale and shipping charges
                        totalSale = (int)sale.TotalSalePrice - (int)discount;

                        historiesSelling.Add(new HistorySellingModel()
                        {
                            Id = sale.Id,
                            DateTime = sale.DateTime,
                            TotalSalePrice = (decimal)totalSale,
                            ShippingCharges = sale.ShippingCharges,
                            LastSellingActivity = db.SellingActivities.Where(z => z.SellingId == sale.Id).OrderByDescending(z => z.Id).FirstOrDefault().SellingStatu.Name
                        });
                    }

                    var response = new ResponseWithData<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = historiesSelling
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
