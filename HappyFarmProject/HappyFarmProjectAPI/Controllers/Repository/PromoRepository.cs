using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class PromoRepository
    {
        /// <summary>
        /// Delete Promo using repository
        /// </summary>
        /// <param name="id"></param>
        public void DeletePromo(int id)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var promo = db.Promoes.Where(x => x.Id == id).FirstOrDefault();
                promo.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit Promo using repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="promoRequest"></param>
        public void EditPromo(int id, EditPromoRequest promoRequest)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var promo = db.Promoes.Where(x => x.Id == id).FirstOrDefault();
                promo.ModifiedBy = promoRequest.ModifiedBy;
                promo.ModifiedAt = DateTime.Now;
                promo.Name = promoRequest.Name;
                promo.Image = promoRequest.Image;
                promo.StartDate = promoRequest.StartDate;
                promo.EndDate = promoRequest.EndDate;
                promo.IsFreeDelivery = promoRequest.IsFreeDelivery;
                promo.Discount = promoRequest.Discount;
                promo.MinTransaction = promoRequest.MinTransaction;
                promo.MaxDiscount = promoRequest.MaxDiscount;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add Promo using repository
        /// </summary>
        /// <param name="promoRequest"></param>
        public void AddPromo(AddPromoRequest promoRequest)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var promo = new Promo()
                {
                    CreatedBy = promoRequest.CreatedBy,
                    ModifiedBy = promoRequest.CreatedBy,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Name = promoRequest.Name,
                    Image = promoRequest.Image,
                    StartDate = promoRequest.StartDate,
                    EndDate = promoRequest.EndDate,
                    IsFreeDelivery = promoRequest.IsFreeDelivery,
                    Discount = promoRequest.Discount,
                    MinTransaction = promoRequest.MinTransaction,
                    MaxDiscount = promoRequest.MaxDiscount
                };

                db.Promoes.Add(promo);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get promoes with paging
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<Promo>> GetPromoes(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get banners
                var promoes = search == null ? db.Promoes
                    .OrderBy(x => x.Id)
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList() : db.Promoes
                    .Where(x =>
                        x.Name.ToLower().Contains(search.ToLower())
                    )
                    .OrderBy(x => x.Id)
                    .Skip((currentPage - 1) * limitPage)
                    .Take(limitPage)
                    .ToList();

                // filter promoes by row status
                promoes = promoes.Where(x => x.RowStatus != "D").ToList();

                // get total banners
                var totalPages = Math.Ceiling((decimal)promoes.Count / limitPage);

                // return banners
                return new ResponsePagingModel<List<Promo>>()
                {
                    Data = promoes,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get Promo by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetPromoById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var promoes = db.Promoes
                    .Where(x => x.Id == id && x.RowStatus != "D")
                    .Select(x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name,
                        x.Image,
                        x.StartDate,
                        x.EndDate,
                        x.IsFreeDelivery,
                        x.Discount,
                        x.MinTransaction,
                        x.MaxDiscount
                    })
                    .FirstOrDefault();

                return promoes;
            }
        }
    }
}