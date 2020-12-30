using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class EmployeeRepository
    {
        /// <summary>
        /// Delete Employee using repository
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEmployee(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var employee = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                employee.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit Employee using repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeRequest"></param>
        public void EditEmployee(int id, EditEmployeeRequest employeeRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var employee = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                employee.RegionId = employeeRequest.RegionId;
                employee.Name = employeeRequest.Name;
                employee.PhoneNumber = employeeRequest.PhoneNumber;
                employee.Email = employeeRequest.Email;
                employee.Address = employeeRequest.Address;
                employee.Gender = employeeRequest.Gender;
                employee.ModifiedAt = DateTime.Now;
                employee.ModifiedBy = employeeRequest.ModifiedBy;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add Employee using repository
        /// </summary>
        /// <param name="employeeRequest"></param>
        public void AddEmployee(AddEmployeeRequest employeeRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                string newPassword = Helper.RandomPassword();

                // create new user login
                UserLogin newUserLogin = new UserLogin()
                {
                    Username = employeeRequest.Username,
                    Password = Helper.EncryptStringSha256Hash(newPassword),
                    RoleId = employeeRequest.RoleId
                };

                db.UserLogins.Add(newUserLogin);
                db.SaveChanges();

                // get last id user login
                int lastUserLoginId = db.UserLogins
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault()
                    .Id;

                // create new employee
                Employee newEmployee = new Employee()
                {
                    Name = employeeRequest.Name,
                    UserLoginId = lastUserLoginId,
                    PhoneNumber = employeeRequest.PhoneNumber,
                    Gender = employeeRequest.Gender,
                    Email = employeeRequest.Email,
                    Address = employeeRequest.Address,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    CreatedBy = employeeRequest.CreatedBy,
                    ModifiedBy = employeeRequest.CreatedBy,
                    RegionId = employeeRequest.RegionId,
                    RowStatus = "A"
                };

                db.Employees.Add(newEmployee);
                db.SaveChanges();

                // send email async
                Helper.SendMailAsync(employeeRequest.Email, "Pendaftaran Akun Karyawan Happy Farm",
                    "<div>" +
                    "Hai <span style = \"font-weight: bold;\">" + employeeRequest.Name + "</span>," +
                    "<br>" +
                    "Akun karyawan kamu telah berhasil didaftarkan, login ke aplikasi <span style=\"font-weight: bold;\">HappyFarm</span> segera dengan menggunakan akun anda:" +
                    "<br><br>" +
                    "<div>Nama Pengguna : " + employeeRequest.Username + "<br>Kata Sandi : " + newPassword + "</div><br>" +
                    "<div>Segala bentuk informasi seperti nomor kontak, alamat e-mail, atau password kamu bersifat rahasia. Jangan " +
                    "menginformasikan data - data tersebut kepada siapapun, termasuk kepada pihak yang mengatasnamakan perusahaan.</div>" +
                    "</div>");
            }
        }

        /// <summary>
        /// Get employee with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<Employee>> GetEmployees(int currentPage, int limitPage, string search, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employees
                var employees = search == null ? db.Employees
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList() : db.Employees
                    .Where(x =>
                        x.Name.ToLower().Contains(search.ToLower()) ||
                        x.PhoneNumber.ToLower().Contains(search.ToLower()) ||
                        x.Email.ToLower().Contains(search.ToLower()) ||
                        x.Address.ToLower().Contains(search.ToLower())
                    )
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList();

                // filter employees by role
                if (role == "Super Admin")
                {
                    employees = employees.Where(x => x.UserLogin.Role.Name != "Super Admin").ToList();
                }
                else if (role == "Manager")
                {
                    employees = employees.Where(x => x.UserLogin.Role.Name != "Super Admin" && x.UserLogin.Role.Name != "Manager").ToList();
                }

                // filter employees by row status
                employees = employees.Where(x => x.RowStatus != "D").ToList();

                // get total employees
                var totalPages = Math.Ceiling((decimal)employees.Count / limitPage);

                // return employees
                return new ResponsePagingModel<List<Employee>>()
                {
                    Data = employees,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get Employee By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetEmployeeById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var employee = db.Employees
                    .Where(x => x.Id == id && x.RowStatus != "D")
                     .Select(x => new
                     {
                         x.Id,
                         x.Name,
                         x.PhoneNumber,
                         x.Email,
                         x.Address,
                         x.Gender
                     })
                    .FirstOrDefault();
                return employee;
            }
        }
    }
}