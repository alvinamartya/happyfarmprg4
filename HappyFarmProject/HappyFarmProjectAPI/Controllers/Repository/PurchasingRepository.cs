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
                foreach (PurchasingDetailRequest detail in purchasingRequest.PurchasingDetails)
                {
                    PurchasingDetail purchasingDetail = new PurchasingDetail()
                    {
                        PurchasingId = lastId,
                        GoodsId = detail.GoodsId,
                        Qty = detail.Qty,
                        Price = detail.Price
                    };

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