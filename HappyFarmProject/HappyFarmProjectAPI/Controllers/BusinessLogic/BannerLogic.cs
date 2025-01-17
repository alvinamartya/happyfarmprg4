﻿using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class BannerLogic
    {
        /// <summary>
        /// Validate data when Add Banner
        /// </summary>
        /// <param name="bannerRequest"></param>
        /// <returns></returns>
        public ResponseModel AddBanner(AddBannerRequest bannerRequest)
        {
            return checkAuthorization(bannerRequest.CreatedBy, HttpStatusCode.Created);
        }

        /// <summary>
        /// Validate data when Edit Banner
        /// </summary>
        /// <param name="bannerRequest"></param>
        /// <returns></returns>
        public ResponseModel EditBanner(int id, EditBannerRequest bannerRequest)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get banner
                var banner = db.Banners.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();

                // validate banner must be exists
                if(banner != null)
                {
                    return checkAuthorization(bannerRequest.ModifiedBy, HttpStatusCode.OK);
                }
                else
                {
                    return new ResponseModel()
                    {
                        Message = "Banner tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }

        /// <summary>
        /// Check Authorization
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ResponseModel checkAuthorization(int id, HttpStatusCode responseSuccess)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var employee = db.Employees.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if (employee != null)
                {
                    if (employee.UserLogin.Role.Name != "Super Admin" && employee.UserLogin.Role.Name != "Admin Promosi")
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
                            StatusCode = responseSuccess
                        };
                    }
                }
                else
                {
                    // employee not found
                    return new ResponseModel()
                    {
                        Message = "Data karyawan tidak ditemukan",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }

        /// <summary>
        /// Getting data by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel GetBannerById(int id, string role)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get banner
                var banner = db.Banners.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();

                // validate banner must be available
                if(banner != null)
                {
                    if (role != "Super Admin" && role != "Admin Promosi")
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
                    // banner not found
                    return new ResponseModel()
                    {
                        Message = "Banner tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}