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
    public class ProductionAdminGoodsController : ApiController
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
        /// To delete goods using production admin account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/PA/Goods/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteGoods(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = goodsLogic.GetGoodsById(id, "Admin Produksi");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                    {
                        // delete goods
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
        /// To edit goods using production admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goodsRequest"></param>
        /// <returns></returns>
        [Route("api/v1/PA/Goods/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditGoods(int id)
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
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                    {
                        // get request from multipart/form-data
                        var httpRequest = HttpContext.Current.Request;
                        EditGoodsRequest goodsRequest = new EditGoodsRequest();
                        foreach (string key in httpRequest.Form.AllKeys)
                        {
                            foreach (string val in httpRequest.Form.GetValues(key))
                            {
                                switch (key)
                                {
                                    case "CategoryId":
                                        goodsRequest.CategoryId = int.Parse(val);
                                        break;
                                    case "Name":
                                        goodsRequest.Name = val;
                                        break;
                                    case "ModifiedBy":
                                        goodsRequest.ModifiedBy = int.Parse(val);
                                        break;
                                    case "Description":
                                        goodsRequest.Description = val;
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
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Goods/" + guidFileName);
                                postedFile.SaveAs(filePath);
                                goodsRequest.FilePath = guidFileName;
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                                goodsRequest.FilePath = "";
                            }
                        }

                        // validate data
                        ResponseModel responseModel = goodsLogic.EditGoods(id, goodsRequest);
                        if (responseModel.StatusCode == HttpStatusCode.OK)
                        {
                            // update goods
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
        /// To create new goods using production admin account
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("api/v1/PA/Goods/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddGoods()
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
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                    {
                        // get request from multipart/form-data
                        var httpRequest = HttpContext.Current.Request;
                        AddGoodsRequest goodsRequest = new AddGoodsRequest();
                        foreach (string key in httpRequest.Form.AllKeys)
                        {
                            foreach (string val in httpRequest.Form.GetValues(key))
                            {
                                switch (key)
                                {
                                    case "CategoryId":
                                        goodsRequest.CategoryId = int.Parse(val);
                                        break;
                                    case "Name":
                                        goodsRequest.Name = val;
                                        break;
                                    case "CreatedBy":
                                        goodsRequest.CreatedBy = int.Parse(val);
                                        break;
                                    case "Description":
                                        goodsRequest.Description = val;
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
                                var filePath = HttpContext.Current.Server.MapPath("~/Images/Goods/" + guidFileName);
                                postedFile.SaveAs(filePath);
                                goodsRequest.FilePath = guidFileName;
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
                        ResponseModel responseModel = goodsLogic.AddGoods(goodsRequest);
                        if (responseModel.StatusCode == HttpStatusCode.Created)
                        {
                            // create new goods
                            await Task.Run(() => repo.AddGoods(goodsRequest));

                            // response success
                            var response = new ResponseWithoutData()
                            {
                                StatusCode = HttpStatusCode.Created,
                                Message = "Berhasil menambah produk"
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
        /// To get goods by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/PA/Goods/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetGoodsById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = goodsLogic.GetGoodsById(id, "Admin Produksi");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                    {
                        // get goods by id
                        Object goods = await Task.Run(() => repo.GetGoodsById(id));

                        // response success
                        var response = new ResponseWithData<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = goods
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
        [Route("api/v1/PA/Goods")]
        [HttpPost]
        public async Task<IHttpActionResult> GetGoods(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Admin Produksi"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Good>> listGoodsPaging = await Task.Run(() => repo.GetGoods(getListData.CurrentPage, getListData.LimitPage, getListData.Search));

                    using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                    {
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
                                    CategoryName = db.Categories.Where(z => z.Id == x.CategoryId).FirstOrDefault().Name
                                })
                                .ToList(),
                            CurrentPage = listGoodsPaging.CurrentPage,
                            TotalPage = listGoodsPaging.TotalPage
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
