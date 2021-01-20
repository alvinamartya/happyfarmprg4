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


namespace HappyFarmProjectAPI.Controllers.CustomerService
{
    public class CustomerServiceCustomerFeedbackController : ApiController
    {
        #region Variable
        // logic
        private CustomerFeedbackLogic customerFeedbackLogic = new CustomerFeedbackLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private CustomerFeedbackRepository repo = new CustomerFeedbackRepository();
        #endregion

        #region Action
        /// <summary>
        /// To get customer feedback by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/CS/CustomerFeedback/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCustomerFeedbackById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = customerFeedbackLogic.GetCustomerFeedbackById(id, "Customer Service");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // get data by id
                        Object data = await Task.Run(() => repo.GetCustomerFeedbackById(id));

                        // response success
                        var response = new ResponseWithData<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = data
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
        /// To get customer feedback
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/CS/CustomerFeedback")]
        [HttpPost]
        public async Task<IHttpActionResult> GetCustomerFeedback(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Customer Service"))
                {
                    // get employee by id
                    ResponsePagingModel<List<CustomerFeedback>> listCustomerFeedbackPaging = await Task.Run(() => repo.GetCustomerFeedbacks(getListData.CurrentPage, getListData.LimitPage, getListData.Search));

                    // response success
                    var response = new ResponseDataWithPaging<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = listCustomerFeedbackPaging
                            .Data
                            .Select(x => new
                            {
                                x.Id,
                                x.OrderId,
                                x.Note,
                                x.Rating
                            })
                            .ToList(),
                        CurrentPage = listCustomerFeedbackPaging.CurrentPage,
                        TotalPage = listCustomerFeedbackPaging.TotalPage
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
