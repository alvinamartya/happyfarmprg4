using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class ChangePasswordRepository
    {
        /// <summary>
        /// Edit Password Repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changePasswordRequest"></param>
        public void ChangePassword(int id, ChangePasswordRequest changePasswordRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var userLogin = db.UserLogins.Where(x => x.Id == id).FirstOrDefault();
                userLogin.Username = changePasswordRequest.Username;
                string newPassword = changePasswordRequest.Password;
                string ecryptPassword = Helper.EncryptStringSha256Hash(newPassword);
                userLogin.Password = ecryptPassword;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get region
        /// </summary>
        /// <returns></returns>
        public List<UserLogin> GetUserLogin()
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employees
                var userLogin = db.UserLogins.ToList();

                // return employees
                return userLogin;
            }
        }

        /// <summary>
        /// Get userlogin By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetUserLoginById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var userLogin = db.UserLogins
                    .Where(x => x.Id == id)
                     .Select(x => new
                     {
                         x.Id,
                         x.Password,
                         x.Username
                     })
                    .FirstOrDefault();
                return userLogin;
            }
        }
    }
}