using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class SellingActivityRepository
    {
        /// <summary>
        /// Edit SellingStatus Repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sellingActivityRequest"></param>
        public void EditSellingActivity(int id, EditSellingActivityRequest sellingActivityRequest)
        {
            
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var sellingStatus = db.SellingActivities.Where(x => x.Id == id).FirstOrDefault();
                sellingStatus.SellingStatusid = sellingActivityRequest.SellingStatusid;
                sellingStatus.CreatedBy = sellingActivityRequest.CreatedBy;
                sellingStatus.CreatedAt = DateTime.Now;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add selling activity Repository
        /// </summary>
        /// <param name="sellingActivityRequest"></param>
        public void AddSellingActivity(AddSellingActivityRequest sellingActivityRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {

                // get last id user login
                int lastUserLoginId = db.UserLogins
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault()
                    .Id;

                string sellingId = sellingActivityRequest.SellingId.Replace("ORD", "");
                int sellingId2 = int.Parse(sellingId.TrimStart('0'));

                // create new selling activity
                SellingActivity newSellingActivity = new SellingActivity()
                {
                    SellingId = sellingId2,
                    SellingStatusid = sellingActivityRequest.SellingStatusid,
                    CreatedAt = DateTime.Now,
                    CreatedBy = sellingActivityRequest.CreatedBy,
                };
                db.SellingActivities.Add(newSellingActivity);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get Selling Activity with paging
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponsePagingModel<Object> GetSellingActivityPaging(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get SellingStatus
                var sellingStatus = db.Sellings
                    .Select(x => new
                    {
                        SellingId = x.Id,
                        SellingStatusName = db.SellingActivities.Where(z => z.SellingId == x.Id).OrderByDescending(z => z.Id).FirstOrDefault().SellingStatu.Name,
                        CreatedAt = db.SellingActivities.Where(z => z.SellingId == x.Id).OrderByDescending(z => z.Id).FirstOrDefault().CreatedAt
                    })
                    .ToList();

                if (search != null && search != "")
                {
                    try
                    {
                        search = int.Parse(search.Replace("ORD", "")).ToString();
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }

                    sellingStatus = sellingStatus
                        .Where(x =>
                            x.SellingId.ToString().Contains(search.ToLower())
                        )
                        .ToList();
                }

                // filter by row status
                // with paging
                //SellingStatuss = SellingStatuss
                //    .Where(x=>x.RowStatus == "A")
                //    .OrderBy(x=>x.Name)
                //    .Skip((currentPage - 1) * limitPage)
                //    .Take(limitPage)
                //    .ToList();

                // without paging
                sellingStatus = sellingStatus
                    .OrderBy(x => x.SellingId)
                    .ToList();

                // get total SellingStatuss
                var totalPages = Math.Ceiling((decimal)db.SellingStatus.Count() / limitPage);

                // return employees
                return new ResponsePagingModel<Object>()
                {
                    Data = sellingStatus,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get SellingStatus
        /// </summary>
        /// <returns></returns>
        public List<SellingActivity> GetSellingActivity()
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get employees
                var sellingStatus = db.SellingActivities.ToList();

                // return employees
                return sellingStatus;
            }
        }

        /// <summary>
        /// Get SellingStatus By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetSellingActivityById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var sellingStatus = db.SellingActivities
                    .Where(x => x.Id == id)
                     .Select(x => new
                     {
                         x.Id,
                         x.SellingId,
                         SellingStatus = x.SellingStatu.Name,
                         x.SellingStatusid
                     })
                    .FirstOrDefault();
                return sellingStatus;
            }
        }
    }
}