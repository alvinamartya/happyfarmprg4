using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic
{
    public class CategoryLogic
    {
        /// <summary>
        /// Validate Data When Add Category
        /// </summary>
        /// <param name="categoryRequest"></param>
        /// <returns></returns>
        public ResponseModel AddCategory(AddCategoryRequest categoryRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // validate name
                bool nameAlreadyExists = db.Categories.Where(x => x.Name.ToLower() == categoryRequest.Name.ToLower()).FirstOrDefault() != null;
                if (nameAlreadyExists)
                {
                    // name is exists
                    return new ResponseModel()
                    {
                        Message = "Nama kategori sudah tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                else
                {
                    // check authorization
                    return checkAuthorization(categoryRequest.CreatedBy, HttpStatusCode.Created);
                }
            }
        }

        /// <summary>
        /// Validate Data when Edit Category
        /// </summary>
        /// <param name="categoryRequest"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel EditCategory(int id, EditCategoryRequest categoryRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employee
                var category = db.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (category != null)
                {
                    // validate name 
                    bool nameAlreadyExists = db.Categories
                        .Where(x => x.Name.ToLower().Equals(categoryRequest.Name.ToLower()) && x.Id != id)
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
                        return checkAuthorization(categoryRequest.ModifiedBy, HttpStatusCode.OK);
                    }
                }
                else
                    // category is not found
                    return new ResponseModel()
                    {
                        Message = "Kategori tidak tersedia",
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
        /// Validate data for getting category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponseModel GetCategoryById(int id, string role)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get category
                var category = db.Categories.Where(x => x.Id == id && x.RowStatus == "A").FirstOrDefault();
                if (category != null)
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
                    // category is not found
                    return new ResponseModel()
                    {
                        Message = "Kategori tidak tersedia",
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }
    }
}