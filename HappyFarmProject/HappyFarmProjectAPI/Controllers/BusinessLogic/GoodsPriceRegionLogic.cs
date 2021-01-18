using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class GoodsPriceRegionLogic
    {
        /// <summary>
        /// Validate Data When Add GoodsPriceRegion
        /// </summary>
        /// <param name="goodsPriceRegionRequest"></param>
        /// <returns></returns>
        public ResponseModel AddGoodsPriceRegion(AddGoodsPriceRegionRequest goodsPriceRegionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // validate goods in same region
                bool goodsAlreadyExists = db.GoodsPriceRegions.Where(x => x.GoodsId == goodsPriceRegionRequest.GoodsId).FirstOrDefault() != null;
                if (goodsAlreadyExists)
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
                    return checkAuthorization(goodsPriceRegionRequest.CreatedBy, HttpStatusCode.Created);
                }
            }
        }

        /// <summary>
        /// Validate Data when Edit Goods Price Region
        /// </summary>
        /// <param name="goodsPriceRegionRequest"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel EditGoodsPriceRegion(int id, EditGoodsPriceRegionRequest goodsPriceRegionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employee
                var GoodsPriceRegion = db.GoodsPriceRegions.Where(x => x.Id == id).FirstOrDefault();
                if (GoodsPriceRegion != null)
                {
                    // validate name 
                    bool goodsAlreadyExists = db.GoodsPriceRegions
                        .Where(x => x.RegionId.Equals(goodsPriceRegionRequest.RegionId) && x.Id != id)
                        .FirstOrDefault() != null;
                    if (goodsAlreadyExists)
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
                        return checkAuthorization(goodsPriceRegionRequest.ModifiedBy, HttpStatusCode.OK);
                    }
                }
                else
                    // Goods Price Region is not found
                    return new ResponseModel()
                    {
                        Message = "Harga produk tidak tersedia",
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
                    if (employee.UserLogin.Role.Name != "Super Admin" && employee.UserLogin.Role.Name != "Admin Produksi")
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
        /// Validate data for getting goodspriceregion by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel GetGoodsPriceRegionById(int id, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get region
                var GoodsPriceRegions = db.GoodsPriceRegions.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if (GoodsPriceRegions != null)
                {
                    if (role != "Super Admin" && role != "Admin Produksi")
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
                    // Goods Price Region is not found
                    return new ResponseModel()
                    {
                        Message = "Harga produk tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}