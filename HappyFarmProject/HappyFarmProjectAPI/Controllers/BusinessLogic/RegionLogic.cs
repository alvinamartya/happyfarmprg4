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
        public ResponseModel AddRegion(AddRegionRequests regionRequest, string role)
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
    }
}