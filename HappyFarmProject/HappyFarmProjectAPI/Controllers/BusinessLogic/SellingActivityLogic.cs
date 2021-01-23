using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class SellingActivityLogic
    {
        /// <summary>
        /// Validate Data when Edit Selling Status
        /// </summary>
        /// <param name="sellingActivityRequest"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel EditSellingActivity(int id, EditSellingActivityRequest sellingActivityRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get selling status
                var sellingStatus = db.SellingActivities.Where(x => x.Id == id).FirstOrDefault();
                if (sellingStatus != null)
                {
                    // validate name cannot same with same id
                    bool nameAlreadyExists = db.SellingActivities
                        .Where(x => x.SellingStatusid.ToString().Equals(sellingActivityRequest.SellingStatusid.ToString()) && x.Id == id)
                        .FirstOrDefault() != null;
                    if (nameAlreadyExists)
                    {
                        // name is exists
                        return new ResponseModel()
                        {
                            Message = "Status harus berubah",
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
                    // region is not found
                    return new ResponseModel()
                    {
                        Message = "Status penjualan tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
            }
        }

        /// <summary>
        /// Validate data for getting selling status by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel GetSellingActivityById(int id, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get selling status
                var sellingStatus = db.SellingActivities.Where(x => x.Id == id).FirstOrDefault();
                if (sellingStatus != null)
                {
                    if (role != "Admin Penjualan")
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
                    // region is not found
                    return new ResponseModel()
                    {
                        Message = "Status penjualan tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}