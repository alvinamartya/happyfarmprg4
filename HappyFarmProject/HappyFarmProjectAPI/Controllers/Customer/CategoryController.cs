using HappyFarmProjectAPI.Controllers.BusinessLogic;
using HappyFarmProjectAPI.Controllers.Repository;
using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HappyFarmProjectAPI.Controllers
{
    public class CategoryController : ApiController
    {
        #region Variable
        // repo
        private CategoryRepository repo = new CategoryRepository();
        #endregion

        #region Action
        /// <summary>
        /// To get categories
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/Category")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCategories()
        {
            try
            {
                // get employee by id
                List<Category> listCategoryPaging = await Task.Run(() => repo.GetCategories());

                // response success
                var response = new ResponseWithData<Object>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Berhasil",
                    Data = listCategoryPaging
                        .Select(x => new
                        {
                            x.Id,
                            x.Name
                        })
                        .ToList(),
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write("Error: " + ex.Message);
                return InternalServerError(ex);
            }
        }
        #endregion
    }
}
