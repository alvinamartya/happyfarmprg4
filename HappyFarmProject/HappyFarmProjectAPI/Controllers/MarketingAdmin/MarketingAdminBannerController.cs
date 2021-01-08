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
    public class MarketingAdminBannerController : ApiController
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
        /// To delete banner using marketing admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/MA/Banner/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBanner(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = bannerLogic.GetBannerById(id, "Admin Promosi");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
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
        /// To edit banner using marketing admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bannerRequest"></param>
        /// <returns></returns>
        [Route("api/v1/MA/Banner/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditBanner(int id)
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    var unsupportedMediaTypeResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.UnsupportedMediaType,
                        Message = "Tipe data pada media tidak di didukung"
                    };

                    return Ok(unsupportedMediaTypeResponse);
                }
                else
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
                    {
                        // get request from multipart/form-data
                        var httpRequest = HttpContext.Current.Request;
                        EditBannerRequest bannerRequest = new EditBannerRequest();
                        foreach (string key in httpRequest.Form.AllKeys)
                        {
                            foreach (string val in httpRequest.Form.GetValues(key))
                            {
                                switch (key)
                                {
                                    case "PromoId":
                                        if (val != "")
                                        {
                                            bannerRequest.PromoId = int.Parse(val);
                                        }
                                        break;
                                    case "Name":
                                        bannerRequest.Name = val;
                                        break;
                                    case "ModifiedBy":
                                        bannerRequest.ModifiedBy = int.Parse(val);
                                        break;
                                }
                            }
                        }

                        // save file
                        foreach (string file in httpRequest.Files)
                        {
                            try
                            {
                                // get file
                                var postedFile = httpRequest.Files[file];

                                // decrypt file name
                                Guid uid = Guid.NewGuid();
                                var guidFileName = uid.ToString() + Path.GetExtension(postedFile.FileName);

                                // save to server
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Banner/" + guidFileName);
                                postedFile.SaveAs(filePath);
                                bannerRequest.FilePath = guidFileName;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                                bannerRequest.FilePath = "";
                            }
                        }

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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// To create new banner using marketing admin account
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("api/v1/MA/Banner/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddBanners()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    var unsupportedMediaTypeResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.UnsupportedMediaType,
                        Message = "Tipe data pada media tidak di didukung"
                    };

                    return Ok(unsupportedMediaTypeResponse);
                }
                else
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
                    {
                        // get request from multipart/form-data
                        var httpRequest = HttpContext.Current.Request;
                        AddBannerRequest bannerRequest = new AddBannerRequest();
                        foreach (string key in httpRequest.Form.AllKeys)
                        {
                            foreach (string val in httpRequest.Form.GetValues(key))
                            {
                                switch (key)
                                {
                                    case "PromoId":
                                        if (val != "")
                                        {
                                            bannerRequest.PromoId = int.Parse(val);
                                        }
                                        break;
                                    case "Name":
                                        bannerRequest.Name = val;
                                        break;
                                    case "CreatedBy":
                                        bannerRequest.CreatedBy = int.Parse(val);
                                        break;
                                }
                            }
                        }

                        // save file
                        foreach (string file in httpRequest.Files)
                        {
                            try
                            {
                                // get file
                                var postedFile = httpRequest.Files[file];

                                // decrypt file name
                                Guid uid = Guid.NewGuid();
                                var guidFileName = uid.ToString() + Path.GetExtension(postedFile.FileName);

                                // save to server
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Banner/" + guidFileName);
                                postedFile.SaveAs(filePath);
                                bannerRequest.FilePath = guidFileName;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Write("Error: " + ex.Message);

                                var ImageNotFoundResponse = new ResponseWithoutData()
                                {
                                    StatusCode = HttpStatusCode.BadRequest,
                                    Message = "Gambar tidak tersedia"
                                };

                                return Ok(ImageNotFoundResponse);
                            }
                        }

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
        [Route("api/v1/MA/Banner/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBannerById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = bannerLogic.GetBannerById(id, "Admin Promosi");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
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
        [Route("api/v1/MA/Banner")]
        [HttpPost]
        public async Task<IHttpActionResult> GetBanners(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Banner>> listBannerPaging = await Task.Run(() => repo.GetBanners(getListData.CurrentPage, getListData.LimitPage, getListData.Search));

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
                                PromoName = x.PromoId == null ? "-" : x.Promo.Name,
                                x.Image
                            })
                            .ToList(),
                        CurrentPage = listBannerPaging.CurrentPage,
                        TotalPage = listBannerPaging.TotalPage
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
