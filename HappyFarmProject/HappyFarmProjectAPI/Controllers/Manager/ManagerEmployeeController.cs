using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HappyFarmProjectAPI.Controllers.Manager
{
    public class ManagerEmployeeController : ApiController
    {
        /// <summary>
        /// To create new employee
        /// </summary>
        /// <param name="employeeRequest"></param>
        /// <returns></returns>
        [Route("api/v1/Manager/Employee/Create")]
        [HttpPost]
        public IHttpActionResult AddEmployee(CreateEmployeeRequest employeeRequest)
        {
            try
            {
                using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
                {
                    // validate username
                    bool usernameAlreadyExists = db.UserLogins.Where(x => x.Username == employeeRequest.Username).FirstOrDefault() != null;
                    if (usernameAlreadyExists)
                    {
                        return BadRequest("Nama pengguna sudah tersedia");
                    }
                    else
                    {
                        // create new user login
                        UserLogin newUserLogin = new UserLogin()
                        {
                            Username = employeeRequest.Username,
                            Password = Helper.EncryptStringSha256Hash(employeeRequest.Password),
                            RoleId = employeeRequest.RoleId
                        };

                        db.UserLogins.Add(newUserLogin);
                        db.SaveChanges();

                        // get last id user login
                        int lastUserLoginId = db.UserLogins
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefault()
                            .Id;

                        System.Diagnostics.Debug.WriteLine("Id: " + lastUserLoginId);

                        // create new employee
                        Employee newEmployee = new Employee()
                        {
                            Name = employeeRequest.Name,
                            UserLoginId = lastUserLoginId,
                            PhoneNumber = employeeRequest.PhoneNumber,
                            Gender = employeeRequest.Gender,
                            Email = employeeRequest.Email,
                            Address = employeeRequest.Address
                        };
                        db.Employees.Add(newEmployee);
                        db.SaveChanges();

                        // response
                        var response = new ResponseWithoutData()
                        {
                            Message = "Berhasil menambahkan akun"
                        };

                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }
    }
}
