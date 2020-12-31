using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace HappyFarmProjectAPI.Controllers
{
    public class RegionLogic
    {
        /// <summary>
        /// Validate Data When Add Region
        /// </summary>
        /// <param name="regionRequest"></param>
        /// <returns></returns>
        public ResponseModel AddRegion(AddRegionRequests regionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // validate name
                bool nameAlreadyExists = db.Regions.Where(x => x.Name.ToLower() == regionRequest.Name.ToLower()).FirstOrDefault() != null;
                if (nameAlreadyExists)
                {
                    // name is exists
                    return new ResponseModel()
                    {
                        Message = "Nama region sudah tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    return new ResponseModel()
                    {
                        Message = "Berhasil",
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }
        }

        /// <summary>
        /// Validate Data when Edit Region
        /// </summary>
        /// <param name="regionRequest"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel EditEmployee(int id, EditRegionRequests regionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employee
                var region = db.Regions.Where(x => x.Id == id).FirstOrDefault();
                if (region != null)
                {
                    // validate name 
                    bool nameAlreadyExists = db.Regions
                        .Where(x => x.Name.ToLower().Equals(regionRequest.Name.ToLower()) && x.Id != id)
                        .FirstOrDefault() != null;
                    if (nameAlreadyExists)
                    {
                        // name is exists
                        return new ResponseModel()
                        {
                            Message = "Nama sudah tersedia",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                    else
                    {
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
                    Message = "Wilayah tidak tersedia",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }
        }
    }
}