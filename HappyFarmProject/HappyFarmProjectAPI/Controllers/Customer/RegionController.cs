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
    public class RegionController : ApiController
    {
        #region Variable
        private RegionRepository regionRepo = new RegionRepository();
        private SubDistrictRepository subDistrictRepo = new SubDistrictRepository();
        #endregion
        #region Action
        [Route("api/v1/Region")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRegions()
        {
            try
            {
                List<Region> regions = await Task.Run(() => regionRepo.GetRegions());

                // response success
                var response = new ResponseWithData<Object>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Berhasil",
                    Data = regions
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
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

        [Route("api/v1/SubDistrict/{regionId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSubDistricts(int regionId)
        {
            try
            {
                List<SubDistrict> subdistricts = await Task.Run(() => subDistrictRepo.GetSubDistrictByRegionId(regionId));

                // response success
                var response = new ResponseWithData<Object>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Berhasil",
                    Data = subdistricts
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.ShippingCharges
                    })
                    .ToList()
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
