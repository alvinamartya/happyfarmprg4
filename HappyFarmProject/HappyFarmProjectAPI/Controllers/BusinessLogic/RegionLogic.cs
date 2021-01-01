using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class RegionLogic
    {
        /// <summary>
        /// Validate Data When Add Region
        /// </summary>
        /// <param name="regionRequest"></param>
        /// <returns></returns>
        public ResponseModel AddRegion(AddRegionRequest regionRequest)
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
                    // check authorization
                    return checkAuthorization(regionRequest.CreatedBy, HttpStatusCode.Created);
                }
            }
        }

        /// <summary>
        /// Validate Data when Edit Region
        /// </summary>
        /// <param name="regionRequest"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel EditRegion(int id, EditRegionRequest regionRequest)
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
                        // check authorization
                        return checkAuthorization(regionRequest.ModifiedBy, HttpStatusCode.OK);
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
                    if (employee.UserLogin.Role.Name != "Super Admin" && employee.UserLogin.Role.Name != "Manager")
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
        /// Validate data for getting region by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel GetRegionById(int id, string role)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get region
                var region = db.Regions.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if(region != null)
                {
                    if(role != "Super Admin" && role != "Manager")
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
                        Message = "Wilayah tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}