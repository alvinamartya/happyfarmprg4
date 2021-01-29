using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class ProfileRepository
    {
        public void ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                Customer customer = db.Customers.Where(x => x.Id == changePasswordRequest.UserId).FirstOrDefault();
                UserLogin login = db.UserLogins.Where(x => x.Id == customer.UserLoginId).FirstOrDefault();
                login.Password = Helper.EncryptStringSha256Hash(changePasswordRequest.NewPassword);
                db.SaveChanges();
            }
        }

        public string GetOldPassword(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                Customer customer = db.Customers.Where(x => x.Id == id).FirstOrDefault();
                UserLogin login = db.UserLogins.Where(x => x.Id == customer.UserLoginId).FirstOrDefault();
                return login.Password;
            }
        }

        public Object GetProfileById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var customer = db.Customers
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.PhoneNumber,
                        x.Email,
                        x.Gender
                    })
                    .FirstOrDefault();
                return customer;
            }
        }

        public void EditCustomer(ProfileRequest request)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                Customer custoemr = db.Customers.Where(x => x.Id == request.Id).FirstOrDefault();
                custoemr.Name = request.Name;
                custoemr.PhoneNumber = request.PhoneNumber;
                custoemr.Gender = request.Gender;
                custoemr.Email = request.Email;
                db.SaveChanges();
            }
        }
    }
}