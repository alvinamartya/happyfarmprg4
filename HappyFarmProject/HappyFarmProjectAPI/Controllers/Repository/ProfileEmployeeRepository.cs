﻿using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class ProfileEmployeeRepository
    {
        public void ChangePasswordEmployee(ChangePasswordRequest changePasswordRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                Employee employee = db.Employees.Where(x => x.Id == changePasswordRequest.UserId).FirstOrDefault();
                UserLogin login = db.UserLogins.Where(x => x.Id == employee.UserLoginId).FirstOrDefault();
                login.Password = Helper.EncryptStringSha256Hash(changePasswordRequest.NewPassword);
                db.SaveChanges();
            }
        }

        public string GetOldPassword(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                Employee employee = db.Employees.Where(x => x.Id == id).FirstOrDefault();
                UserLogin login = db.UserLogins.Where(x => x.Id == employee.UserLoginId).FirstOrDefault();
                return login.Password;
            }
        }

        public Object GetEmployeeById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                Object employee = db.Employees
                    .Where(x => x.Id == id)
                    .Select(x=> new { 
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