using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class PromoLogic
    {
        /// <summary>
        /// Validate data when Add Promo
        /// </summary>
        /// <param name="promoRequest"></param>
        /// <returns></returns>
        public ResponseModel AddPromo(AddPromoRequest promoRequest)
        {
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if(promoRequest.StartDate > now)
            {
                if(promoRequest.EndDate > promoRequest.StartDate)
                {
                    return checkAuthorization(promoRequest.CreatedBy, HttpStatusCode.Created);
                }
                else
                {
                    return new ResponseModel()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Tanggal berakhir promo tidak valid"
                    };
                }
            }
            else
            {
                return new ResponseModel()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Tanggal mulai promo tidak valid"
                };
            }
        }

        /// <summary>
        /// Validate data when Edit Banner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="promoRequest"></param>
        /// <returns></returns>
        public ResponseModel EditPromo(int id, EditPromoRequest promoRequest)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get promo
                var promo = db.Promoes.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if(promo != null)
                {
                    DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    if (promoRequest.StartDate > now)
                    {
                        if (promoRequest.EndDate > promoRequest.StartDate)
                        {
                            return checkAuthorization(promoRequest.ModifiedBy, HttpStatusCode.OK);
                        }
                        else
                        {
                            return new ResponseModel()
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                Message = "Tanggal berakhir promo tidak valid"
                            };
                        }
                    }
                    else
                    {
                        return new ResponseModel()
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Tanggal mulai promo tidak valid"
                        };
                    }
                }
                else
                {
                    return new ResponseModel()
                    {
                        Message = "Promo tidak tersedia",
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
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
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
        public ResponseModel GetPromoById(int id, string role)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get promo
                var promo = db.Promoes.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();

                // validate promo must be available
                if(promo != null)
                {
                    if(role != "Super Admin" && role != "Admin Promosi")
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
                    // promo not found
                    return new ResponseModel()
                    {
                        Message = "Promo tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}