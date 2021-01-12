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
    public class SuperAdminBannerController : ApiController
    {
        #region Variable
        // logic
        private BannerLogic bannerLogic = new BannerLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private BannerRepository repo = new BannerRepository();
        #endregion

        #region Action
        /// <summary>
        /// To delete banner using super admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Banner/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBanner(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = bannerLogic.GetBannerById(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // delete banner
                        await Task.Run(() => repo.DeleteBanner(id));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil menghapus banner"
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
        /// To edit banner using super admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bannerRequest"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Banner/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditBanner(int id, EditBannerRequest bannerRequest)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                {
                    // validate data
                    ResponseModel responseModel = bannerLogic.EditBanner(id, bannerRequest);
                    if (responseModel.StatusCode == HttpStatusCode.OK)
                    {
                        // update banner
                        await Task.Run(() => repo.EditBanner(id, bannerRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil mengubah banner"
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
        /// To create new banner using super admin account
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("api/v1/SA/Banner/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddBanners(AddBannerRequest bannerRequest)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                {
                    // validate data
                    ResponseModel responseModel = bannerLogic.AddBanner(bannerRequest);
                    if (responseModel.StatusCode == HttpStatusCode.Created)
                    {
                        // create banner
                        await Task.Run(() => repo.AddBanner(bannerRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
                            Message = "Berhasil menambah banner"
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
        /// To get banner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Banner/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBannerById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = bannerLogic.GetBannerById(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // get banner by id
                        Object banner = await Task.Run(() => repo.GetBannerById(id));

                        // response success
                        var response = new ResponseWithData<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = banner
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
        /// To get banners
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Banner")]
        [HttpPost]
        public async Task<IHttpActionResult> GetBanners(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Banner>> listBannerPaging = await Task.Run(() => repo.GetBanners(getListData.CurrentPage, getListData.LimitPage, getListData.Search));

                    using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                    {
                        // response success
                        var response = new ResponseDataWithPaging<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = listBannerPaging
                                .Data
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Name,
                                    x.PromoId,
                                    PromoName = x.PromoId == null ? "-" : db.Promoes.Where(z=>z.Id == x.PromoId).FirstOrDefault().Name    
                                })
                                .ToList(),
                            CurrentPage = listBannerPaging.CurrentPage,
                            TotalPage = listBannerPaging.TotalPage
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
        #endregion
    }
}
