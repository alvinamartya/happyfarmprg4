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
                var user = db.Employees.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();

                // validate user must be available
                if (user != null)
                {
                    if ((role != "Manager" && role != "Super Admin") ||
                        (role == "Manager" && (user.UserLogin.Role.Name == "Super Admin" || user.UserLogin.Role.Name == "Manager")) ||
                        (role == "Super Admin" && user.UserLogin.Role.Name == "Super Admin"))
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
                        Message = "Data karyawan tidak tersedia",
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
        public ResponseModel EditEmployee(int id, EditEmployeeRequest employeeRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employee
                var employee = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                if (employee != null)
                {
                    // get user
                    var user = db.UserLogins.Where(x => x.Id == employee.UserLoginId).FirstOrDefault();

                    // validate user must be available
                    if (user != null)
                    {
                        // get region
                        bool regionIsAvailable = user.Role.Name == "Manager" ? true : db.Regions.Where(x => x.Id == employeeRequest.RegionId).FirstOrDefault() != null;

                        // validate region must be available
                        if (regionIsAvailable)
                        {
                            // validate email 
                            bool emailAlreadyExists = db.Employees
                                .Where(x => x.Email.ToLower().Equals(employeeRequest.Email.ToLower()) && x.Id != id)
                                .FirstOrDefault() != null;
                            if (emailAlreadyExists)
                            {
                                // email is exists
                                return new ResponseModel()
                                {
                                    Message = "Email sudah tersedia",
                                    StatusCode = HttpStatusCode.BadRequest
                                };
                            }
                            else
                            {
                                if (Helper.ValidateEmail(employeeRequest.Email))
                                {
                                    if (Helper.ValidatePhoneNumber(employeeRequest.PhoneNumber))
                                    {
                                        var modifiedEmployee = db.Employees.Where(x => x.Id == employeeRequest.ModifiedBy).FirstOrDefault();
                                        if (modifiedEmployee != null)
                                        {
                                            if ((modifiedEmployee.UserLogin.Role.Name != "Manager" && modifiedEmployee.UserLogin.Role.Name != "Super Admin") ||
                                            (modifiedEmployee.UserLogin.Role.Name == "Manager" && (user.Role.Name == "Super Admin" || user.Role.Name == "Manager")) ||
                                            (modifiedEmployee.UserLogin.Role.Name == "Super Admin" && user.Role.Name == "Super Admin"))
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
                                            return new ResponseModel()
                                            {
                                                Message = "Data karyawan tidak ditemukan",
                                                StatusCode = HttpStatusCode.BadRequest
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
                else
                {
                    // user is not found
                    return new ResponseModel()
                    {
                        Message = "Data karyawan tidak tersedia",
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
        public ResponseModel AddEmployee(AddEmployeeRequest employeeRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // validate username and email
                bool usernameAlreadyExists = db.UserLogins.Where(x => x.Username.ToLower() == employeeRequest.Username.ToLower()).FirstOrDefault() != null;
                bool emailAlreadyExists = db.Employees.Where(x => x.Email.ToLower() == employeeRequest.Email.ToLower()).FirstOrDefault() != null;
                if (usernameAlreadyExists)
                {
                    // username is exists
                    return new ResponseModel()
                    {
                        Message = "Nama pengguna sudah tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                else if (emailAlreadyExists)
                {
                    // email is exists
                    return new ResponseModel()
                    {
                        Message = "Email sudah tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    // get region

                    // validate of accress rights
                    var getRole = db.Roles.Where(x => x.Id == employeeRequest.RoleId).FirstOrDefault();
                    var isSuperAdmin = getRole.Name == "Super Admin";
                    var isCustomer = getRole.Name == "Customer";

                    bool regionIsAvailable = getRole.Name == "Manager" ? true : db.Regions.Where(x => x.Id == employeeRequest.RegionId).FirstOrDefault() != null;
                    if (regionIsAvailable)
                    {
                        var createdEmployee = db.Employees.Where(x => x.Id == employeeRequest.CreatedBy).FirstOrDefault();
                        if (createdEmployee != null)
                        {
                            if (((createdEmployee.UserLogin.Role.Name == "Manager" || createdEmployee.UserLogin.Role.Name == "Super Admin") && (isSuperAdmin || isCustomer)) ||
                                (createdEmployee.UserLogin.Role.Name != "Manager" && createdEmployee.UserLogin.Role.Name != "Super Admin"))
                            {
                                // unauthorized
                                return new ResponseModel()
                                {
                                    StatusCode = HttpStatusCode.Unauthorized,
                                    Message = "Anda tidak memiliki hak akses"
                                };
                            }
                            else
                            {
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
                        else
                        {
                            return new ResponseModel()
                            {
                                Message = "Data karyawan tidak ditemukan",
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
                var employee = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                if (employee != null)
                {
                    var user = db.UserLogins.Where(x => x.Id == employee.UserLoginId).FirstOrDefault();
                    if (user != null)
                    {
                        if ((role != "Manager" && role != "Super Admin") ||
                            (role == "Manager" && (user.Role.Name == "Super Admin" || user.Role.Name == "Manager")) ||
                            (role == "Super Admin" && user.Role.Name == "Super Admin"))
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
                else
                {
                    // user is not found
                    return new ResponseModel()
                    {
                        Message = "Data karyawan tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}