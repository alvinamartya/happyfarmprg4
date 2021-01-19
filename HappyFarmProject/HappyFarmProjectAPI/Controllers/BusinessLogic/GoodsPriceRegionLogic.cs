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
                    if (role != "Admin Produksi")
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