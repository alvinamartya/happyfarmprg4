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
    public class SuperAdminGoodsController : ApiController
    {
        #region Variable
        // logic
        private GoodsLogic goodsLogic = new GoodsLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private GoodsRepository repo = new GoodsRepository();
        #endregion

        #region Action
        /// <summary>
        /// To delete goods using super admin account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Goods/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteGoods(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = goodsLogic.GetGoodsById(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // delete employee
                        await Task.Run(() => repo.DeleteGoods(id));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil menghapus produk"
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
        /// To edit goods using super admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goodsRequest"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Goods/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditGoods(int id, EditGoodsRequest goodsRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = goodsLogic.EditGoods(id, goodsRequest);
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // update employee
                    await Task.Run(() => repo.EditGoods(id, goodsRequest));

                    // response success
                    var response = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil mengubah produk"
                    };

                    return Ok(response);
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
        /// To create new goods using super admin account
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("api/v1/SA/Goods/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddGoods(AddGoodsRequest goodsRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = goodsLogic.AddGoods(goodsRequest);
                if (responseModel.StatusCode == HttpStatusCode.Created)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // create new employee
                        await Task.Run(() => repo.AddGoods(goodsRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
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
        /// To get goods by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Goods/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGoodsById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = goodsLogic.GetGoodsById(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // get employee by id
                        Object employee = await Task.Run(() => repo.GetGoodsById(id));

                        // response success
                        var response = new ResponseWithData<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = employee
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
        /// To get list goods
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Goods")]
        [HttpPost]
        public async Task<IHttpActionResult> GetGoods(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Good>> listGoodsPaging = await Task.Run(() => repo.GetGoods(getListData.CurrentPage, getListData.LimitPage, getListData.Search));

                    // response success
                    var response = new ResponseDataWithPaging<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = listGoodsPaging
                            .Data
                            .Select(x => new
                            {
                                x.Id,
                                x.Name,
                                CategoryId = x.CategoryId,
                                CategoryName = x.Category.Name
                            })
                            .ToList(),
                        CurrentPage = listGoodsPaging.CurrentPage,
                        TotalPage = listGoodsPaging.TotalPage
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
