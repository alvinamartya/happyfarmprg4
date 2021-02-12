using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class DashboardRepository
    {
        public List<DashboardModel> GetSumSelling(int month, int year)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                List<DashboardModel> dashboardModels = new List<DashboardModel>();
                List<Selling> sellings = db.Sellings.ToList();
                foreach (Selling sale in sellings)
                {
                    int totalSale = 0;
                    DateTime newDate = new DateTime(sale.DateTime.Year, sale.DateTime.Month, sale.DateTime.Day);
                    int discount = 0;
                    int shippingCharges = (int)sale.ShippingCharges;

                    // calculate promo
                    if (sale.PromoId != null)
                    {
                        discount = sale.Promo.Discount > 0 ? (int)((double)sale.TotalSalePrice * sale.Promo.Discount / 100) : 0;
                    }

                    // calculate total sale and shipping charges
                    totalSale = (int)sale.TotalSalePrice - (int)discount + (int)shippingCharges;
                    
                    // add to model
                    DashboardModel model = dashboardModels.Where(x => x.Date == newDate).FirstOrDefault();
                    if (model != null)
                    {
                        model.Total += totalSale;
                    }
                    else
                    {
                        dashboardModels.Add(new DashboardModel()
                        {
                            Date = newDate,
                            Total = totalSale
                        });
                    }
                }

                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                return dashboardModels.Where(x=>x.Date >= startDate && x.Date <= endDate).ToList();
            }
        }

        public List<DashboardModel> GetSumPurchasing(int month, int year)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                List<DashboardModel> dashboardModels = new List<DashboardModel>();
                var purchasing = (from s in db.Purchasings
                                select new
                                {
                                    Date = s.DateTime,
                                    TotalPurchase = s.TotalPurchasePrice
                                })
                                .ToList()
                                .GroupBy(x => x.Date)
                                .Select(x => new {
                                    Date = x.Key,
                                    TotalPurchase = (int)x.ToList().Select(z => z.TotalPurchase).Sum()
                                })
                                .ToList();

                foreach (var x in purchasing)
                {
                    DateTime newDate = new DateTime(x.Date.Year, x.Date.Month, x.Date.Day);
                    dashboardModels.Add(new DashboardModel()
                    {
                        Date = newDate,
                        Total = x.TotalPurchase
                    });
                }

                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);
                return dashboardModels.Where(x=>x.Date >= startDate && x.Date <= endDate).ToList();
            }
        }
    }
}