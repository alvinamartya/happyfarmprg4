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
        /// Add GoodsPriceRegion using repository
        /// </summary>
        /// <param name="GoodsPriceRegionRequest"></param>
        public void AddGoodsPriceRegion(AddGoodsPriceRegionRequest goodsPriceRegionRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                GoodsPriceRegion goodsPrice = db.GoodsPriceRegions.Where(x => x.GoodsId == goodsPriceRegionRequest.GoodsId && x.RegionId == goodsPriceRegionRequest.RegionId).FirstOrDefault();
                if(goodsPrice != null)
                {
                    goodsPrice.Price = goodsPriceRegionRequest.Price;
                    goodsPrice.ModifiedBy = goodsPriceRegionRequest.CreatedBy;
                    goodsPrice.ModifiedAt = DateTime.Now;
                }
                else
                {
                    GoodsPriceRegion newGoodsPriceRegion = new GoodsPriceRegion()
                    {
                        Price = goodsPriceRegionRequest.Price,
                        ModifiedBy = goodsPriceRegionRequest.CreatedBy,
                        CreatedBy = goodsPriceRegionRequest.CreatedBy,
                        ModifiedAt = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        RegionId = goodsPriceRegionRequest.RegionId,
                        GoodsId = goodsPriceRegionRequest.GoodsId,
                        RowStatus = "A"
                    };
                    db.GoodsPriceRegions.Add(newGoodsPriceRegion);
                }
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
        public ResponsePagingModel<List<GoodsPriceRegion>> GetGoodsPriceRegion(int goodsId, int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get goodspriceregion
                var listGoodsPriceRegions = db.GoodsPriceRegions.Where(x=>x.GoodsId == goodsId).ToList();

                if (search != null && search != "")
                {
                    listGoodsPriceRegions = listGoodsPriceRegions
                        .Where(x =>
                            x.Good.Name.ToLower().Contains(search.ToLower()) ||
                            x.Region.Name.ToLower().Contains(search.ToLower())
                        )
                        .ToList();
                }

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