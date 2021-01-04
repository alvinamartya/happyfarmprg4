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
    public class SuperAdminEmployeeController : ApiController
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
        /// To delete employee using super admin account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Employee/Delete/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.DeleteEmployee(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // delete employee
                        await Task.Run(() => repo.DeleteEmployee(id));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil menghapus data karyawan"
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
        /// To edit employee using super admin account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeRequest"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Employee/Edit/{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> EditEmployee(int id, EditEmployeeRequest employeeRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.EditEmployee(id, employeeRequest);
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // update employee
                        await Task.Run(() => repo.EditEmployee(id, employeeRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil mengubah data karyawan"
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
        /// To create new employee using super admin account
        /// </summary>
        /// <param name="employeeRequest"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Employee/Add")]
        [HttpPost]
        public async Task<IHttpActionResult> AddEmployee(AddEmployeeRequest employeeRequest)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.AddEmployee(employeeRequest);
                if (responseModel.StatusCode == HttpStatusCode.Created)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // create new employee
                        await Task.Run(() => repo.AddEmployee(employeeRequest));

                        // response success
                        var response = new ResponseWithoutData()
                        {
                            StatusCode = HttpStatusCode.Created,
                            Message = "Berhasil menambah data karyawan"
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
        [Route("api/v1/SA/Employee/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeById(int id)
        {
            try
            {
                // validate data
                ResponseModel responseModel = employeeLogic.GetEmployeeById(id, "Super Admin");
                if (responseModel.StatusCode == HttpStatusCode.OK)
                {
                    // validate token
                    if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                    {
                        // get employee by id
                        Object employee = await Task.Run(() => repo.GetEmployeeById(id));

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
        /// To get list employee
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("api/v1/SA/Employee")]
        [HttpPost]
        public async Task<IHttpActionResult> GetEmployees(GetListDataRequest getListData)
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Super Admin"))
                {
                    // get employee by id
                    ResponsePagingModel<List<Employee>> employeesPaging = await Task.Run(() => repo.GetEmployees(getListData.CurrentPage, getListData.LimitPage, getListData.Search, "Super Admin"));

                    System.Diagnostics.Debug.WriteLine(employeesPaging.Data.Count);
                    System.Diagnostics.Debug.WriteLine(getListData.CurrentPage);
                    System.Diagnostics.Debug.WriteLine(getListData.Search);
                    System.Diagnostics.Debug.WriteLine(getListData.LimitPage);

                    // response success
                    using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                    {
                        var response = new ResponseDataWithPaging<Object>()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Message = "Berhasil",
                            Data = employeesPaging
                            .Data
                            .Select(x => new
                            {
                                x.Id,
                                x.Name,
                                x.PhoneNumber,
                                x.Email,
                                x.Address,
                                x.Gender,
                                Role = x.UserLogin.Role.Name,
                                x.UserLogin.RoleId,
                                Region = db.Regions.Where(z=>z.Id == x.RegionId).FirstOrDefault() == null ? null : db.Regions.Where(z => z.Id == x.RegionId).FirstOrDefault().Name,
                                x.RegionId
                            })
                            .ToList(),
                            CurrentPage = employeesPaging.CurrentPage,
                            TotalPage = employeesPaging.TotalPage
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
