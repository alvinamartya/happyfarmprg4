using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class SellingStatusRepository
    {
        /// <summary>
        /// Get selling status
        /// </summary>
        /// <returns></returns>
        public List<SellingStatu> GetSellingStatus()
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get selling status
                var sellingStatus = db.SellingStatus.ToList();

                sellingStatus = sellingStatus.Where(x => x.Name != "Keranjang" && x.Name != "Menunggu Pembayaran" && x.Name != "Pembayaran Sedang Diproses" && x.Name != "Pesanan menunggu diriview").ToList();

                // return employees
                return sellingStatus;
            }
        }
    }
}