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
    public class SuperAdminCategoryController : ApiController
    {
        #region Variable
        // logic
        private CategoryLogic categoryLogic = new CategoryLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private CategoryRepository repo = new CategoryRepository();
        #endregion

        #region Action
        /// <summary>
        /// To delete category using Super Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Category/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = categoryLogic.GetCategoryById(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // delete region
                        await Task.Run(() => repo.DeleteCategory(id));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil menghapus kategori"
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
        /// To edit category using Super Admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryRequest"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Category/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditCategory(int id, EditCategoryRequest categoryRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = categoryLogic.EditCategory(id, categoryRequest);
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // update region
                        await Task.Run(() => repo.EditCategory(id, categoryRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil mengubah kategori"
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
        /// To create new category using Super Admin account
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [Route("api/v1/SA/Category/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddCategory(AddCategoryRequest categoryRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = categoryLogic.AddCategory(categoryRequest);
                if (responseModel.StatusCode == HttpStatusCode.Created)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // create region
                        await Task.Run(() => repo.AddCategory(categoryRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
                            Message = "Berhasil menambah kategori"
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
        /// To get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Category/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCategoryById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = categoryLogic.GetCategoryById(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // get goods by id
                        Object region = await Task.Run(() => repo.GetCategoryById(id));

                        // response success
                        var response = new ResponseWithData<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = region
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
        /// To get categories
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Category")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCategories(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Category>> listCategoryPaging = await Task.Run(() => repo.GetCategories(getListData.CurrentPage, getListData.LimitPage, getListData.Search));

                    // response success
                    var response = new ResponseDataWithPaging<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = listCategoryPaging
                            .Data
                            .Select(x => new
                            {
                                x.Id,
                                x.Name
                            })
                            .ToList(),
                        CurrentPage = listCategoryPaging.CurrentPage,
                        TotalPage = listCategoryPaging.TotalPage
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
