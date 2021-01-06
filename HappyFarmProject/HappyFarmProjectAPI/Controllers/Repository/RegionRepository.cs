using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class RegionRepository
    {
        /// <summary>
        /// Delete Region Repository
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRegion(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var region = db.Regions.Where(x => x.Id == id).FirstOrDefault();
                region.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit Region Repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regionRequest"></param>
        public void EditRegion(int id, EditRegionRequest regionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var region = db.Regions.Where(x => x.Id == id).FirstOrDefault();
                region.Name = regionRequest.Name;
                region.ModifiedAt = DateTime.Now;
                region.ModifiedBy = regionRequest.ModifiedBy;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add Region Repository
        /// </summary>
        /// <param name="regionRequest"></param>
        public void AddRegion(AddRegionRequest regionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {

                // get last id user login
                int lastUserLoginId = db.UserLogins
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault()
                    .Id;

                // create new region
                Region newRegion = new Region()
                {
                    Name = regionRequest.Name,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    CreatedBy = regionRequest.CreatedBy,
                    ModifiedBy = regionRequest.CreatedBy,
                    RowStatus = "A"
                };
                db.Regions.Add(newRegion);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get region with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<Region>> GetRegions(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employees
                var regions = search == null ? db.Regions
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList() : db.Regions
                    .Where(x =>
                        x.Name.ToLower().Contains(search.ToLower())
                    )
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList();

                // get total regions
                var totalPages = Math.Ceiling((decimal)db.Regions.Count() / limitPage);

                // return employees
                return new ResponsePagingModel<List<Region>>()
                {
                    Data = regions,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get Region By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetRegionById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var region = db.Regions
                    .Where(x => x.Id == id)
                     .Select(x => new
                     {
                         x.Id,
                         x.Name
                     })
                    .FirstOrDefault();
                return region;
            }
        }
    }
}