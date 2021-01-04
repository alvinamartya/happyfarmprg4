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
    public class ManagerRoleController : ApiController
    {
        #region Variable
        // logic
        private TokenLogic tokenLogic = new TokenLogic();

        // repo
        private RoleRepository repo = new RoleRepository();
        #endregion
        #region Action
        /// <summary>
        /// To get roles
        /// </summary>
        /// <param name="getListData"></param>
        /// <returns></returns>
        [Route("api/v1/Manager/Role")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRegions()
        {
            try
            {
                // validate token
                if (tokenLogic.ValidateTokenInHeader(Request, "Manager"))
                {
                    // get employee by id
                    List<Role> roles = await Task.Run(() => repo.GetRoles());

                    // response success
                    var response = new ResponseWithData<Object>()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Berhasil",
                        Data = roles
                            .Select(x => new
                            {
                                x.Id,
                                x.Name
                            })
                            .ToList()
                    };

                    return Ok(response);
                }
                else
                {
                    // unauthorized
                    var unAuthorizedResponse = new ResponseWithoutData()
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "Anda tidak memiliki hak akses"
                    };

                    return Ok(unAuthorizedResponse);
                }
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
