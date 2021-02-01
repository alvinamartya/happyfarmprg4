using HappyFarmProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProject
{
    public class Purchasing
    {
        private static List<PurchasingDetailRequest> detailPurchasing;

        public static List<PurchasingDetailRequest> GetDetailPurchasing()
        {
            if (detailPurchasing == null)
            {
                detailPurchasing =  new List<PurchasingDetailRequest>();
            }
            return detailPurchasing;
        }

        public static void EditDetailPurchasing(PurchasingDetailRequest detailRequest)
        {
            var purchasing = detailPurchasing.Where(x => x.GoodsId == detailRequest.GoodsId).FirstOrDefault();
            purchasing.Qty = detailRequest.Qty;
        }

        public static PurchasingDetailRequest GetDetailPurchasingByID(int goodsId)
        {
            return detailPurchasing.Where(x => x.GoodsId == goodsId).FirstOrDefault();
        }

        public static void AddDetailPurchasing(PurchasingDetailRequest detailRequest)
        {
            detailPurchasing.Add(detailRequest);
        }

        public static void DeleteDetailPurchasing(int id)
        {
            PurchasingDetailRequest detailRequest = detailPurchasing.Where(x => x.GoodsId == id).FirstOrDefault();
            detailPurchasing.Remove(detailRequest);
        }

        public static List<GoodsListModelView> GetGoodsListModel(List<GoodsListModelView> goodsLists)
        {
            GetDetailPurchasing();
            List<GoodsListModelView> newGoodsList = new List<GoodsListModelView>();
            foreach (GoodsListModelView x in goodsLists)
            {
                var isAvailable = detailPurchasing.Where(z => z.GoodsId == x.Id).FirstOrDefault() != null;
                if (!isAvailable) newGoodsList.Add(x);
            }
            return newGoodsList;
        }

        public static void ClearDetailPurchasing()
        {
            detailPurchasing = new List<PurchasingDetailRequest>();
        }
    }
}