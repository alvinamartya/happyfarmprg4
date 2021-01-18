using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class GoodsPriceRegionRepository
    {
        /// <summary>
        /// Delete GoodsPriceRegion using repository
        /// </summary>
        /// <param name="id"></param>
        public void DeleteGoodsPriceRegion(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var goodsPriceRegion = db.GoodsPriceRegions.Where(x => x.Id == id).FirstOrDefault();
                goodsPriceRegion.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit GoodsPriceRegion using repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="GoodsPriceRegionRequest"></param>
        public void EditGoodsPriceRegion(int id, EditGoodsPriceRegionRequest GoodsPriceRegionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var goodsPriceRegions = db.GoodsPriceRegions.Where(x => x.Id == id).FirstOrDefault();
                goodsPriceRegions.ModifiedBy = GoodsPriceRegionRequest.ModifiedBy;
                goodsPriceRegions.GoodsId = GoodsPriceRegionRequest.GoodsId;
                goodsPriceRegions.ModifiedAt = DateTime.Now;
                goodsPriceRegions.RegionId = GoodsPriceRegionRequest.RegionId;
                goodsPriceRegions.Price = GoodsPriceRegionRequest.Price;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add GoodsPriceRegion using repository
        /// </summary>
        /// <param name="GoodsPriceRegionRequest"></param>
        public void AddGoodsPriceRegion(AddGoodsPriceRegionRequest GoodsPriceRegionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var GoodsPriceRegion = new GoodsPriceRegion()
                {
                    GoodsId = GoodsPriceRegionRequest.GoodsId,
                    RegionId = GoodsPriceRegionRequest.RegionId,
                    ModifiedBy = GoodsPriceRegionRequest.CreatedBy,
                    CreatedBy = GoodsPriceRegionRequest.CreatedBy,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    Price = GoodsPriceRegionRequest.Price,
                    RowStatus = "A"
                };
                db.GoodsPriceRegions.Add(GoodsPriceRegion);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get goodspriceregion with paging
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<GoodsPriceRegion>> GetGoodsPriceRegion(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get goodspriceregion
                var listGoodsPriceRegions = db.GoodsPriceRegions.ToList();

                if (search != null && search != "")
                {
                    listGoodsPriceRegions = listGoodsPriceRegions
                        .Where(x =>
                            x.Good.Name.ToLower().Contains(search.ToLower()) ||
                            x.Region.Name.ToLower().Contains(search.ToLower())
                        )
                        .ToList();
                }

                // filter goods by row status
                // with paging
                //listGoodsPriceRegions = listGoodsPriceRegions
                //    .Where(x => x.RowStatus != "D")
                //    .OrderBy(x => x.Region.Name)
                //    .ThenBy(x => x.Name)
                //    .Skip((currentPage - 1) * limitPage)
                //    .Take(limitPage)
                //    .ToList();

                // without paging
                listGoodsPriceRegions = listGoodsPriceRegions
                   .Where(x => x.RowStatus != "D")
                   .OrderBy(x => x.Good.Name)
                   .ToList();

                // get total list of goods
                var totalPages = Math.Ceiling((decimal)listGoodsPriceRegions.Count / limitPage);

                // return list of goods
                return new ResponsePagingModel<List<GoodsPriceRegion>>()
                {
                    Data = listGoodsPriceRegions,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get sub district by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetGoodsPriceRegionById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var GoodsPriceRegions = db.GoodsPriceRegions
                    .Where(x => x.Id == id && x.RowStatus != "D")
                    .Select(x => new
                    {
                        x.Id,
                        GoodsId = x.GoodsId,
                        Good = db.Goods.Where(y => y.Id == x.GoodsId).FirstOrDefault().Name,
                        RegionId = x.RegionId,
                        Region = db.Regions.Where(z => z.Id == x.RegionId).FirstOrDefault().Name,
                        Price = x.Price
                    })
                    .FirstOrDefault();

                return GoodsPriceRegions;
            }
        }
    }
}