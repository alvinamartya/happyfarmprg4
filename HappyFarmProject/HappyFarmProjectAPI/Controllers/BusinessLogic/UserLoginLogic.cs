using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class UserLoginLogic
    {
        /// <summary>
        /// Validate Data when Edit userlogin
        /// </summary>
        /// <param name="userloginRequest"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel EditUserLogin(int id, ChangePasswordRequest changePasswordRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employee
                var userLogin = db.UserLogins.Where(x => x.Id == id).FirstOrDefault();
                if (userLogin != null)
                {
                    // validate usernname 
                    bool usernameAlreadyExists = db.UserLogins
                        .Where(x => x.Username.ToLower().Equals(changePasswordRequest.Username.ToLower()) && x.Id != id)
                        .FirstOrDefault() != null;
                    if (usernameAlreadyExists)
                    {
                        // name is exists
                        return new ResponseModel()
                        {
                            Message = "Nama Pengguna sudah tersedia",
                            StatusCode = HttpStatusCode.BadRequest
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
                    // category is not found
                    return new ResponseModel()
                    {
                        Message = "Akun tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
            }
        }
    }
}