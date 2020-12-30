using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class GoodsLogic
    {
        /// <summary>
        /// Validate data when Add Goods
        /// </summary>
        /// <returns></returns>
        public ResponseModel AddGoods(AddGoodsRequest goodsRequest)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // validate name must be not exists
                var nameAlreadyExists = db.Goods.Where(x => x.Name.ToLower().Equals(goodsRequest.Name.ToLower())).FirstOrDefault() != null;
                if(nameAlreadyExists)
                {
                    // name is exists
                    return new ResponseModel()
                    {
                        Message = "Nama produk sudah tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    // get employee
                    var employee = db.Employees.Where(x => x.Id == goodsRequest.CreatedBy && x.RowStatus == "A").FirstOrDefault();
                    if(employee != null)
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
                                StatusCode = HttpStatusCode.Created
                            };
                        }
                    }
                    else
                    {
                        return new ResponseModel()
                        {
                            Message = "Data karyawan tidak ditemukan",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Validate data when Edit Goods
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goodsRequest"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel EditGoods(int id, EditGoodsRequest goodsRequest)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get goods by id
                var goods = db.Goods.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if(goods != null)
                {
                    // validate name must be not exists
                    var nameAlreadyExists = db.Goods.Where(x => x.Name.ToLower().Equals(goodsRequest.Name.ToLower()) && x.Id != id).FirstOrDefault() != null;
                    if(nameAlreadyExists)
                    {
                        // name is exists
                        return new ResponseModel()
                        {
                            Message = "Nama produk sudah tersedia",
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                    else
                    {
                        // get employee
                        var employee = db.Employees.Where(x => x.Id == goodsRequest.ModifiedBy && x.RowStatus == "A").FirstOrDefault();
                        if(employee != null)
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
                                    StatusCode = HttpStatusCode.OK
                                };
                            }
                        }
                        else
                        {
                            return new ResponseModel()
                            {
                                Message = "Data karyawan tidak ditemukan",
                                StatusCode = HttpStatusCode.BadRequest
                            };
                        }
                    }
                }
                else
                {
                    // employee is not found
                    return new ResponseModel()
                    {
                        Message = "Produk tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }

        /// <summary>
        /// Validate data for getting goods by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel GetGoodsById(int id, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get goods by id
                var goods = db.Goods.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if (goods != null)
                {
                    if (role != "Super Admin" && role != "Admin Produksi")
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
                    // employee is not found
                    return new ResponseModel()
                    {
                        Message = "Produk tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}