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
    public class MarketingAdminPromoController : ApiController
    {
        #region Variable
        // logic
        private PromoLogic promoLogic = new PromoLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private PromoRepository repo = new PromoRepository();
        #endregion
        #region Action
        /// <summary>
        /// To delete promo using marketing admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/MA/Promo/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePromo(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = promoLogic.GetPromoById(id, "Admin Promosi");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
                    {
                        // delete promo
                        await Task.Run(() => repo.DeletePromo(id));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil menghapus promo"
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
        /// To edit promo using marketing admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="promoRequest"></param>
        /// <returns></returns>
        [Route("api/v1/MA/Promo/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditPromo(int id)
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
                        EditPromoRequest promoRequest = new EditPromoRequest();
                        foreach (string key in httpRequest.Form.AllKeys)
                        {
                            foreach (string val in httpRequest.Form.GetValues(key))
                            {
                                switch (key)
                                {
                                    case "ModifiedBy":
                                        promoRequest.ModifiedBy = int.Parse(val);
                                        break;
                                    case "Name":
                                        promoRequest.Name = val;
                                        break;
                                    case "StartDate":
                                        promoRequest.StartDate = DateTime.Parse(val);
                                        break;
                                    case "EndDate":
                                        promoRequest.EndDate = DateTime.Parse(val);
                                        break;
                                    case "IsFreeDelivery":
                                        promoRequest.IsFreeDelivery = val;
                                        break;
                                    case "Discount":
                                        promoRequest.Discount = float.Parse(val);
                                        break;
                                    case "MinTransaction":
                                        promoRequest.MinTransaction = int.Parse(val);
                                        break;
                                    case "MaxDiscount":
                                        promoRequest.MaxDiscount = int.Parse(val);
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
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Promo/" + guidFileName);
                                postedFile.SaveAs(filePath);
                                promoRequest.FilePath = guidFileName;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                                promoRequest.FilePath = "";
                            }
                        }

                        // validate data
                        ResponseModel responseModel = promoLogic.EditPromo(id, promoRequest);
                        if (responseModel.StatusCode == HttpStatusCode.OK)
                        {
                            // update promo
                            await Task.Run(() => repo.EditPromo(id, promoRequest));

                            // response success
                            var response = new ResponseWithoutData()
                            {
                                StatusCode = HttpStatusCode.OK,
                                Message = "Berhasil mengubah promo"
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
        /// To create new promo using marketing admin account
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("api/v1/MA/Promo/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPromo()
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
                        AddPromoRequest promoRequest = new AddPromoRequest();
                        foreach (string key in httpRequest.Form.AllKeys)
                        {
                            foreach (string val in httpRequest.Form.GetValues(key))
                            {
                                switch (key)
                                {
                                    case "CreatedBy":
                                        promoRequest.CreatedBy = int.Parse(val);
                                        break;
                                    case "Name":
                                        promoRequest.Name = val;
                                        break;
                                    case "StartDate":
                                        promoRequest.StartDate = DateTime.Parse(val);
                                        break;
                                    case "EndDate":
                                        promoRequest.EndDate = DateTime.Parse(val);
                                        break;
                                    case "IsFreeDelivery":
                                        promoRequest.IsFreeDelivery = val;
                                        break;
                                    case "Discount":
                                        promoRequest.Discount = float.Parse(val);
                                        break;
                                    case "MinTransaction":
                                        promoRequest.MinTransaction = int.Parse(val);
                                        break;
                                    case "MaxDiscount":
                                        promoRequest.MaxDiscount = int.Parse(val);
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
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Promo/" + guidFileName);
                                postedFile.SaveAs(filePath);
                                promoRequest.FilePath = guidFileName;
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
                        ResponseModel responseModel = promoLogic.AddPromo(promoRequest);
                        if (responseModel.StatusCode == HttpStatusCode.Created)
                        {
                            // create promo
                            await Task.Run(() => repo.AddPromo(promoRequest));

                            // response success
                            var response = new ResponseWithoutData()
                            {
                                StatusCode = HttpStatusCode.Created,
                                Message = "Berhasil menambah promo"
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
        /// To get promo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/MA/Promo/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPromoById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = promoLogic.GetPromoById(id, "Admin Promosi");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
                    {
                        // get goods by id
                        Object promo = await Task.Run(() => repo.GetPromoById(id));

                        // response success
                        var response = new ResponseWithData<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = promo
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
        /// To get promoes
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/MA/Promo")]
        [HttpPost]
        public async Task<IHttpActionResult> GetPromoes(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Promosi"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Promo>> listPromoesPaging = await Task.Run(() => repo.GetPromoes(getListData.CurrentPage, getListData.LimitPage, getListData.Search));

                    // response success
                    var response = new ResponseDataWithPaging<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = listPromoesPaging
                            .Data
                            .Select(x => new
                            {
                                x.Id,
                                x.Code,
                                x.Name,
                                x.Image,
                                x.StartDate,
                                x.EndDate,
                                x.IsFreeDelivery,
                                x.Discount,
                                x.MinTransaction,
                                x.MaxDiscount
                            })
                            .ToList(),
                        CurrentPage = listPromoesPaging.CurrentPage,
                        TotalPage = listPromoesPaging.TotalPage
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
