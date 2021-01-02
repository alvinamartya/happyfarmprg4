using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class SubDistrictLogic
    {
        /// <summary>
        /// Validate Data When Add Sub District
        /// </summary>
        /// <param name="subDistrictRequest"></param>
        /// <returns></returns>
        public ResponseModel AddSubDistrict(AddSubDistrictRequest subDistrictRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // validate name
                bool nameAlreadyExists = db.SubDistricts.Where(x => x.Name.ToLower() == subDistrictRequest.Name.ToLower()).FirstOrDefault() != null;
                if (nameAlreadyExists)
                {
                    // name is exists
                    return new ResponseModel()
                    {
                        Message = "Nama kecamatan sudah tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    // check authorization
                    return checkAuthorization(subDistrictRequest.CreatedBy, HttpStatusCode.Created);
                }
            }
        }

        /// <summary>
        /// Validate Data when Edit Sub District
        /// </summary>
        /// <param name="subDistrictRequest"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel EditSubDistrict(int id, EditSubDistrictRequest subDistrictRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employee
                var subDistrict = db.SubDistricts.Where(x => x.Id == id).FirstOrDefault();
                if (subDistrict != null)
                {
                    // validate name 
                    bool nameAlreadyExists = db.SubDistricts
                        .Where(x => x.Name.ToLower().Equals(subDistrictRequest.Name.ToLower()) && x.Id != id)
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
                        return checkAuthorization(subDistrictRequest.ModifiedBy, HttpStatusCode.OK);
                    }
                }
                else
                    // sub district is not found
                    return new ResponseModel()
                    {
                        Message = "Kecamatan tidak tersedia",
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
        /// Validate data for getting sub district by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel GetSubDistrictById(int id, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get region
                var subDistricts = db.SubDistricts.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if (subDistricts != null)
                {
                    if (role != "Super Admin" && role != "Manager")
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
                    // sub district is not found
                    return new ResponseModel()
                    {
                        Message = "Kecamatan tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}