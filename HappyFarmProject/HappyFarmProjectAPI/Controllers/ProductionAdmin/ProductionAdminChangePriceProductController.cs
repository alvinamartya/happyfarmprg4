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
    public class ProductionAdminChangePriceProductController : ApiController
    {
        #region Variable
        // logic
        private GoodsPriceRegionLogic goodsPriceLogic = new GoodsPriceRegionLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private GoodsPriceRegionRepository repo = new GoodsPriceRegionRepository();
        private RegionRepository repoRegion = new RegionRepository();
        #endregion

        #region Action
        /// <summary>
        /// To get price by goods
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/PA/GoodsPrice/{goodsId}")]
        [HttpPost]
        public async Task<IHttpActionResult> GetPriceByGoods(int goodsId, GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                {
                    // get price by goods
                    ResponsePagingModel<List<GoodsPriceRegion>> listGoodsPriceRegion = await Task.Run(() => repo.GetGoodsPriceRegion(goodsId, getListData.CurrentPage, getListData.LimitPage, getListData.Search));

                    using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                    {
                        List<GoodsPriceResponse> priceResponse = new List<GoodsPriceResponse>();
                        ResponsePagingModel<List<Region>> regionListPaging = repoRegion.GetRegionsPaging(getListData.CurrentPage, getListData.LimitPage, "");;
                        
                        foreach(Region region in regionListPaging.Data.OrderBy(x=>x.Name).ToList())
                        {
                            var goodsPrice = db.GoodsPriceRegions
                                .Where(x => x.RegionId == region.Id && x.GoodsId == goodsId)
                                .OrderByDescending(x=>x.Id)
                                .FirstOrDefault();

                            decimal? price = null;
                            if (goodsPrice != null) price = goodsPrice.Price;

                            priceResponse.Add(new GoodsPriceResponse()
                            {
                                RegionId = region.Id,
                                Region = region.Name,
                                Price = price
                            });
                        }

                        var response = new ResponseDataWithPaging<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = priceResponse,
                            CurrentPage = listGoodsPriceRegion.CurrentPage,
                            TotalPage = listGoodsPriceRegion.TotalPage
                        };

                        return Ok(response);
                    }
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
        /// To update new goods price region using production admin account
        /// </summary>
        /// <param name="goodsPriceRegionRequest"></param>
        /// <returns></returns>
        [Route("api/v1/PA/GoodsPrice/Edit")]
        [HttpPost]
        public async Task<IHttpActionResult> EditGoodsPrice(AddGoodsPriceRegionRequest goodsPriceRegionRequest)
        {
            try
            {
                // validate token
                if(tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                {
                    await Task.Run(() => repo.AddGoodsPriceRegion(goodsPriceRegionRequest));

                    // response success
                    var response = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil menambah produk"
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
