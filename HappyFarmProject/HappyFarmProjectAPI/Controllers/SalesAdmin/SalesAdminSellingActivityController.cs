using HappyFarmProjectAPI.Controllers.BusinessLogic;
using HappyFarmProjectAPI.Controllers.Repository;
using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HappyFarmProjectAPI.Controllers
{
    public class SalesAdminSellingActivityController : ApiController
    {
        #region Variable
        // logic
        private SellingActivityLogic sellingActivityLogic = new SellingActivityLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private SellingActivityRepository repo = new SellingActivityRepository();
        #endregion

        #region Action
        /// <summary>
        /// To create new region using super admin account
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("api/v1/SALA/SellingActivity/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSellingActivity(AddSellingActivityRequest sellingActivityRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = sellingActivityLogic.AddSellingActivity(sellingActivityRequest);
                if (responseModel.StatusCode == HttpStatusCode.Created)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Penjualan"))
                    {
                        // create region
                        await Task.Run(() => repo.AddSellingActivity(sellingActivityRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
                            Message = "Berhasil menambah status"
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
                else if (responseModel.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // unauthorized
                    var unAuthorizedResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "Anda tidak memiliki hak akses"
                    };

                    return Ok(unAuthorizedResponse);
                }
                else
                {
                    // bad request
                    var badRequestResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = responseModel.Message
                    };

                    return Ok(badRequestResponse);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// To edit selling status using sales admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sellingActivityRequest"></param>
        /// <returns></returns>
        [Route("api/v1/SALA/SellingActivity/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditSellingActivity(int id, EditSellingActivityRequest sellingActivityRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = sellingActivityLogic.EditSellingActivity(id, sellingActivityRequest);
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Penjualan"))
                    {
                        // update region
                        await Task.Run(() => repo.EditSellingActivity(id, sellingActivityRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil mengubah status penjualan"
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
                else if (responseModel.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // unauthorized
                    var unAuthorizedResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "Anda tidak memiliki hak akses"
                    };

                    return Ok(unAuthorizedResponse);
                }
                else
                {
                    // bad request
                    var badRequestResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = responseModel.Message
                    };

                    return Ok(badRequestResponse);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// To get selling activity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SALA/SellingActivity/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSellingActivityById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = sellingActivityLogic.GetSellingActivityById(id, "Admin Penjualan");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Penjualan"))
                    {
                        // get selling status by id
                        Object sellingStatus = await Task.Run(() => repo.GetSellingActivityById(id));

                        // response success
                        var response = new ResponseWithData<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = sellingStatus
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
                else
                {
                    // bad request
                    var badRequestResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = responseModel.Message
                    };

                    return Ok(badRequestResponse);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// To get regions with paging
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/SALA/SellingActivity")]
        [HttpPost]
        public async Task<IHttpActionResult> GetSellingActivity(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Penjualan"))
                {
                    ResponsePagingModel<Object> listSellingStatusPaging = await Task.Run(() => repo.GetSellingActivityPaging(getListData.CurrentPage, getListData.LimitPage, getListData.Search));
                    using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                    {
                        // response success
                        var response = new ResponseDataWithPaging<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = listSellingStatusPaging.Data,
                            CurrentPage = listSellingStatusPaging.CurrentPage,
                            TotalPage = listSellingStatusPaging.TotalPage
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

        [Route("api/v1/SALA/SellingDetail/{id}")]
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
                           GoodsName = db.Goods.Where(z => z.Id == x.GoodsId).FirstOrDefault().Name,
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
        #endregion
    }
}
