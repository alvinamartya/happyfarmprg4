using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class DashboardRepository
    {
        public List<DashboardModel> GetSumSelling()
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                List<DashboardModel> dashboardModels = new List<DashboardModel>();
                var sellings = (from s in db.Sellings
                                join sa in db.SellingActivities on s.Id equals sa.SellingId
                                where sa.SellingStatusid == 6
                                select new
                                {
                                    Date = s.DateTime,
                                    TotalSalePrice = s.TotalSalePrice
                                })
                                .ToList()
                                .GroupBy(x=>x.Date)
                                .Select(x=> new { 
                                    Date = x.Key,
                                    TotalSale = (int)x.ToList().Select(z=>z.TotalSalePrice).Sum()
                                })
                                .ToList();

                foreach(var x in sellings)
                {
                    DateTime newDate = new DateTime(x.Date.Year, x.Date.Month, x.Date.Day);
                    dashboardModels.Add(new DashboardModel()
                    {
                        Date = newDate,
                        Total = x.TotalSale
                    });
                }

                return dashboardModels;
            }
        }

        public List<DashboardModel> GetSumPurchasing()
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

                return dashboardModels;
            }
        }
    }
}