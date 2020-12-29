using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace HappyFarmProjectAPI.Controllers
{
    public class EmployeeLogic
    {
        /// <summary>
        /// Validate data when Delete Employee
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel DeleteEmployee(int id, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get user
                bool userIsAvailable = db.Employees.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault() != null;

                // validate user must be available
                if (userIsAvailable)
                {
                    if (role != "Manager" && role != "Super Admin")
                    {
                        // unauthroized
                        return new ResponseModel()
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            Message = "Anda tidak memiliki hak akses"
                        };
                    }
                    else
                    {
                        // return ok
                        return new ResponseModel()
                        {
                            Message = "Berhasil",
                            StatusCode = HttpStatusCode.OK
                        };
                    }
                }
                else
                {
                    // employee is not found
                    return new ResponseModel()
                    {
                        Message = "Akun karyawan tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }

        /// <summary>
        /// Validate Data when Edit Employee
        /// </summary>
        /// <param name="employeeRequest"></param>
        /// <returns></returns>
        public ResponseModel EditEmployee(int id, EditEmployeeRequest employeeRequest, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get user
                bool userIsAvailable = db.UserLogins.Where(x => x.Id == id).FirstOrDefault() != null;

                // validate user must be available
                if (userIsAvailable)
                {
                    // get region
                    bool regionIsAvailable = db.Regions.Where(x => x.Id == employeeRequest.RegionId).FirstOrDefault() != null;

                    // validate region must be available
                    if (regionIsAvailable)
                    {
                        if (Helper.ValidateEmail(employeeRequest.Email))
                        {
                            if (Helper.ValidatePhoneNumber(employeeRequest.PhoneNumber))
                            {
                                if (role != "Manager" && role != "Super Admin")
                                {
                                    // unauthroized
                                    return new ResponseModel()
                                    {
                                        StatusCode = HttpStatusCode.Unauthorized,
                                        Message = "Anda tidak memiliki hak akses"
                                    };
                                }
                                else
                                {
                                    // return ok
                                    return new ResponseModel()
                                    {
                                        Message = "Berhasil",
                                        StatusCode = HttpStatusCode.OK
                                    };
                                }
                            }
                            else
                            {
                                // phone number is not valid
                                return new ResponseModel()
                                {
                                    Message = "Format No Hp tidak valid",
                                    StatusCode = HttpStatusCode.BadRequest
                                };
                            }
                        }
                        else
                        {
                            // email is not valid
                            return new ResponseModel()
                            {
                                Message = "Format Email tidak valid",
                                StatusCode = HttpStatusCode.BadRequest
                            };
                        }
                    }
                    else
                    {
                        // region is not found
                        return new ResponseModel()
                        {
                            Message = "Wilayah tidak tersedia",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }
                else
                {
                    // user is not found
                    return new ResponseModel()
                    {
                        Message = "Pengguna tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }

        /// <summary>
        /// Validate Data When Add Employee
        /// </summary>
        /// <param name="employeeRequest"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel AddEmployee(AddEmployeeRequest employeeRequest, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // validate username
                bool usernameAlreadyExists = db.UserLogins.Where(x => x.Username == employeeRequest.Username).FirstOrDefault() != null;
                if (usernameAlreadyExists)
                {
                    // username is exists
                    return new ResponseModel()
                    {
                        Message = "Nama pengguna sudah tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    // validate of accress rights
                    var getRole = db.Roles.Where(x => x.Id == employeeRequest.RoleId).FirstOrDefault();
                    var isSuperAdmin = getRole.Name == "Super Admin";
                    var isCustomer = getRole.Name == "Customer";

                    if (role == "Manager" || role == "Super Admin")
                    {
                        if (isSuperAdmin || isCustomer)
                        {
                            // unauthorized
                            return new ResponseModel()
                            {
                                StatusCode = HttpStatusCode.Unauthorized,
                                Message = "Anda tidak memiliki hak akses"
                            };
                        }
                    }
                    else if (role != "Manager" && role != "Super Admin")
                    {
                        // unautorized
                        return new ResponseModel()
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            Message = "Anda tidak memiliki hak akses"
                        };
                    }

                    if (Helper.ValidateEmail(employeeRequest.Email))
                    {
                        if (Helper.ValidatePhoneNumber(employeeRequest.PhoneNumber))
                        {
                            // response created
                            return new ResponseModel()
                            {
                                Message = "Berhasil",
                                StatusCode = HttpStatusCode.Created
                            };
                        }
                        else
                        {
                            // phone number is not valid
                            return new ResponseModel()
                            {
                                Message = "Format No Hp tidak valid",
                                StatusCode = HttpStatusCode.BadRequest
                            };
                        }
                    }
                    else
                    {
                        // email is not valid
                        return new ResponseModel()
                        {
                            Message = "Format Email tidak valid",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Validate data for getting employee by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel GetEmployeeById(int id, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get user
                bool userIsAvailable = db.UserLogins.Where(x => x.Id == id).FirstOrDefault() != null;

                if (userIsAvailable)
                {
                    if (role != "Manager" && role != "Super Admin")
                    {
                        // unauthroized
                        return new ResponseModel()
                        {
                            StatusCode = HttpStatusCode.Unauthorized,
                            Message = "Anda tidak memiliki hak akses"
                        };
                    }
                    else
                    {
                        // return ok
                        return new ResponseModel()
                        {
                            Message = "Berhasil",
                            StatusCode = HttpStatusCode.OK
                        };
                    }
                }
                else
                {
                    // user is not found
                    return new ResponseModel()
                    {
                        Message = "Pengguna tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}