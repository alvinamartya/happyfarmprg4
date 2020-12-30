using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class GoodsRepository
    {
        /// <summary>
        /// Delete Goods using repository
        /// </summary>
        /// <param name="id"></param>
        public void DeleteGoods(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var goods = db.Goods.Where(x => x.Id == id).FirstOrDefault();
                goods.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit Goods using repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="goodsRequest"></param>
        public void EditGoods(int id, EditGoodsRequest goodsRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var goods = db.Goods.Where(x => x.Id == id).FirstOrDefault();
                goods.ModifiedBy = goodsRequest.ModifiedBy;
                goods.Name = goodsRequest.Name;
                goods.ModifiedAt = DateTime.Now;
                goods.CategoryId = goodsRequest.CategoryId;
                goods.Description = goodsRequest.Description;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add goods using repository
        /// </summary>
        /// <param name="goodsRequest"></param>
        public void AddGoods(AddGoodsRequest goodsRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var goods = new Good()
                {
                    CategoryId = goodsRequest.CategoryId,
                    Name = goodsRequest.Name,
                    ModifiedBy = goodsRequest.CreatedBy,
                    CreatedBy = goodsRequest.CreatedBy,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    Description = goodsRequest.Description,
                    RowStatus = "A"
                };
                db.Goods.Add(goods);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get goods with paging
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<Good>> GetGoods(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get goods
                var listGoods = search == null ? db.Goods
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList() : db.Goods
                    .Where(x =>
                        x.Name.ToLower().Contains(search.ToLower()) ||
                        x.Description.ToLower().Contains(search.ToLower())
                    )
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList();

                // filter goods by row status
                listGoods = listGoods.Where(x => x.RowStatus != "D").ToList();

                // get total list of goods
                var totalPages = Math.Ceiling((decimal)listGoods.Count / limitPage);

                // return list of goods
                return new ResponsePagingModel<List<Good>>()
                {
                    Data = listGoods,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get Goods by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetGoodsById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var goods = db.Goods
                    .Where(x => x.Id == id && x.RowStatus != "D")
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        CategoryId = x.CategoryId,
                        CategoryName = x.Category.Name
                    })
                    .FirstOrDefault();

                return goods;
            }
        }
    }
}