using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class PurchasingRepository
    {
        /// <summary>
        /// Get purchasing
        /// </summary>
        /// <returns></returns>
        public List<Purchasing> GetPurchasingHistory(int employeeId)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get purchasing
                var purchasing = db.Purchasings.Where(x => x.EmployeeId == employeeId).ToList();

                // return purchasing
                return purchasing;
            }
        }

        public void CustomerPurchasing(CustomerPurchasingRequest request)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                int? promoId = null;

                if(request.PromoCode != "")
                {
                    Promo promo = db.Promoes.Where(x => x.Code == request.PromoCode).FirstOrDefault();
                    if (promo != null) promoId = promo.Id;
                }

                Selling selling = new Selling()
                {
                    CustomerId = request.CustomerId,
                    PromoId = promoId,
                    RecipientName = request.RecipientName,
                    RecipientAddress = request.RecipientAddress,
                    RecipientPhone = request.RecipientPhone,
                    SubDistrictId = request.SubdistrictId,
                    ShippingCharges = request.ShippingCharges,
                    TotalSalePrice = request.TotalPurchase,
                    DateTime = DateTime.Now
                };

                db.Sellings.Add(selling);
                db.SaveChanges();

                int lastSellingId = db.Sellings.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                foreach(var x in request.Products)
                {
                    SellingDetail details = new SellingDetail()
                    {
                        SellingId = lastSellingId,
                        GoodsId = x.GoodsId,
                        Qty = x.Qty,
                        GoodsPrice = x.Price
                    };

                    db.SellingDetails.Add(details);
                }

                SellingActivity activity = new SellingActivity()
                {
                    SellingId = lastSellingId,
                    SellingStatusid = 1,
                    CreatedAt = DateTime.Now
                };
                db.SellingActivities.Add(activity);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// delete purchasing
        /// </summary>
        /// <param name="deletePurchasing"></param>
        public void DeletePurchasing(DeletePurchasingRequest deletePurchasing)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // select purchasing details
                List<PurchasingDetail> details = db.PurchasingDetails.Where(x => x.PurchasingId == deletePurchasing.Id).ToList();
                int region = db.Employees.Where(x => x.Id == deletePurchasing.EmployeeId).FirstOrDefault().RegionId ?? db.Regions.FirstOrDefault().Id;
                foreach (PurchasingDetail x in details)
                {
                    GoodsStockRegion stockRegion = db.GoodsStockRegions.Where(z => z.GoodsId == x.GoodsId && z.RegionId == region).FirstOrDefault();
                    stockRegion.Stock -= x.Qty;

                    db.PurchasingDetails.Remove(x);
                    db.SaveChanges();
                }

                Purchasing purchasing = db.Purchasings.Where(x => x.Id == deletePurchasing.Id).FirstOrDefault();
                db.Purchasings.Remove(purchasing);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// add purchasing
        /// </summary>
        /// <param name="purchasingRequest"></param>
        public void AddPurchasing(AddPurchasingRequest purchasingRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // create purchasing
                Purchasing purchasing = new Purchasing()
                {
                    FarmerName = purchasingRequest.FarmerName,
                    EmployeeId = purchasingRequest.EmployeeId,
                    DateTime = DateTime.Now,
                    FarmerAddress = purchasingRequest.FarmerAddress,
                    FarmerPhone = purchasingRequest.FarmerPhone,
                    TotalPurchasePrice = 0
                };
                db.Purchasings.Add(purchasing);
                db.SaveChanges();

                long totalPurchasing = 0;
                int lastId = db.Purchasings.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                int regionId = db.Employees.Where(x => x.Id == purchasingRequest.EmployeeId).FirstOrDefault().RegionId ?? db.Regions.OrderBy(x => x.Id).FirstOrDefault().Id;
                foreach (PurchasingDetailRequest detail in purchasingRequest.PurchasingDetails)
                {
                    PurchasingDetail purchasingDetail = new PurchasingDetail()
                    {
                        PurchasingId = lastId,
                        GoodsId = detail.GoodsId,
                        Qty = detail.Qty,
                        Price = detail.Price
                    };

                    // add goods stock region
                    var goodsStokRegion = db.GoodsStockRegions.Where(x => x.GoodsId == detail.GoodsId && x.RegionId == regionId).FirstOrDefault();
                    if (goodsStokRegion == null)
                    {
                        GoodsStockRegion newGoodsStockRegion = new GoodsStockRegion()
                        {
                            GoodsId = detail.GoodsId,
                            RegionId = regionId,
                            Stock = detail.Qty
                        };
                        db.GoodsStockRegions.Add(newGoodsStockRegion);
                    }
                    else
                    {
                        goodsStokRegion.Stock += detail.Qty;
                    }

                    totalPurchasing += detail.Qty * detail.Price;

                    db.PurchasingDetails.Add(purchasingDetail);
                    db.SaveChanges();
                }

                Purchasing lastPurchasing = db.Purchasings.Where(x => x.Id == lastId).FirstOrDefault();
                lastPurchasing.TotalPurchasePrice = totalPurchasing;
                db.SaveChanges();
            }
        }
    }
}