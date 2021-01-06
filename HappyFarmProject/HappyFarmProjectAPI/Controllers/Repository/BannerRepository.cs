using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class BannerRepository
    {
        /// <summary>
        /// Delete Banner using repository
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBanner(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var banner = db.Banners.Where(x => x.Id == id).FirstOrDefault();
                banner.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit Banner using repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bannerRequest"></param>
        public void EditBanner(int id, EditBannerRequest bannerRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var banner = db.Banners.Where(x => x.Id == id).FirstOrDefault();
                banner.PromoId = bannerRequest.PromoId;
                banner.Name = bannerRequest.Name;
                banner.Image = bannerRequest.Image;
                banner.ModifiedBy = bannerRequest.ModifiedBy;
                banner.ModifiedAt = DateTime.Now;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add Banner using repository
        /// </summary>
        /// <param name="bannerRequest"></param>
        public void AddBanner(AddBannerRequest bannerRequest)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var banner = new Banner()
                {
                    PromoId = bannerRequest.PromoId,
                    Name = bannerRequest.Name,
                    CreatedBy = bannerRequest.CreatedBy,
                    ModifiedBy = bannerRequest.CreatedBy,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    RowStatus = "A",
                    Image = bannerRequest.Image
                };
                db.Banners.Add(banner);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get banner with paging
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<Banner>> GetBanners(int currentPage, int limitPage, string search)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get banners
                var banners = db.Banners.ToList();

                if(search != null && search != "")
                {
                    banners = banners
                        .Where(x =>
                            x.Name.ToLower().Contains(search.ToLower())
                        )
                        .ToList();
                }

                // filter banners by row status
                //// with paging
                //banners = banners
                //    .Where(x => x.RowStatus != "D")
                //    .Skip((currentPage - 1) * limitPage)
                //    .Take(limitPage)
                //    .ToList();

                // without paging
                banners = banners
                    .Where(x => x.RowStatus != "D")
                    .OrderBy(x=>x.Name)
                    .ToList();

                // get total banners
                var totalPages = Math.Ceiling((decimal)banners.Count / limitPage);

                // return banners
                return new ResponsePagingModel<List<Banner>>()
                {
                    Data = banners,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get Banner by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetBannerById(int id)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var banners = db.Banners
                    .Where(x => x.Id == id && x.RowStatus != "D")
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.PromoId,
                        PromoName = x.PromoId == null ? "-" : x.Promo.Name,
                        x.Image
                    })
                    .FirstOrDefault();

                return banners;
            }
        }
    }
}