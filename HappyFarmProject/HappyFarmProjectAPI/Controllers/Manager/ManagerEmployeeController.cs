using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HappyFarmProjectAPI.Controllers.Manager
{
    public class ManagerEmployeeController : ApiController
    {
        #region Variable
        // logic
        private EmployeeLogic employeeLogic = new EmployeeLogic();
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private EmployeeRepository repo = new EmployeeRepository();
        #endregion

        #region Action
        /// <summary>
        /// To delete employee using manager account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/Manager/Employee/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.DeleteEmployee(id, "Manager");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Manager"))
                    {
                        // delete employee
                        await Task.Run(() => repo.DeleteEmployee(id));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
                            Message = "Berhasil menghapus akun"
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
        /// To edit employee using manager account
        /// </summary>
        /// <param name="employeeRequest"></param>
        /// <returns></returns>
        [Route("api/v1/Manager/Employee/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditEmployee(int id, EditEmployeeRequest employeeRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.EditEmployee(id, employeeRequest, "Manager");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Manager"))
                    {
                        // update employee
                        await Task.Run(() => repo.EditEmployee(id, employeeRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
                            Message = "Berhasil mengubah akun"
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
        /// To create new employee using manager account
        /// </summary>
        /// <param name="employeeRequest"></param>
        /// <returns></returns>
        [Route("api/v1/Manager/Employee/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddEmployee(AddEmployeeRequest employeeRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.AddEmployee(employeeRequest, "Manager");
                if (responseModel.StatusCode == HttpStatusCode.Created)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Manager"))
                    {
                        // create new employee
                        await Task.Run(() => repo.AddEmployee(employeeRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
                            Message = "Berhasil menambah akun"
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
        /// To get employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/Manager/Employee/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.GetEmployeeById(id, "Manager");
                if(responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Manager"))
                    {
                        // get employee by id
                        Employee employee = await Task.Run(() => repo.GetEmployeeById(id));

                        // response success
                        var response = new ResponseWithData<Employee>()
                        {
                            StatusCode = HttpStatusCode.Created,
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
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// To get list employee
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("api/v1/Manager/Employee/{currentPage}/{limitPage}/{search}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployees(int currentPage, int limitPage, string search)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Manager"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Employee>> employeesPaging = await Task.Run(() => repo.GetEmployees(currentPage, limitPage, search));

                    // response success
                    var response = new ResponseDataWithPaging<List<Employee>>()
                    {
                        StatusCode = HttpStatusCode.Created,
                        Message = "Berhasil",
                        Data = employeesPaging.Data,
                        CurrentPage = employeesPaging.CurrentPage,
                        TotalPage = employeesPaging.TotalPage
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
            catch(Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
