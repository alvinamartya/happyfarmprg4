using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class SubDistrictRepository
    {
        /// <summary>
        /// Delete SubDistrict using repository
        /// </summary>
        /// <param name="id"></param>
        public void DeleteSubDistrict(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var subDistrict = db.SubDistricts.Where(x => x.Id == id).FirstOrDefault();
                subDistrict.RowStatus = "D";
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Edit SubDistrict using repository
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subDistrictRequest"></param>
        public void EditSubDistrict(int id, EditSubDistrictRequest subDistrictRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var subDistricts = db.SubDistricts.Where(x => x.Id == id).FirstOrDefault();
                subDistricts.ModifiedBy = subDistrictRequest.ModifiedBy;
                subDistricts.Name = subDistrictRequest.Name;
                subDistricts.ModifiedAt = DateTime.Now;
                subDistricts.ShippingCharges = subDistrictRequest.ShippingCharges;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add Sub District using repository
        /// </summary>
        /// <param name="subDistrictRequest"></param>
        public void AddSubDistrict(AddSubDistrictRequest subDistrictRequest)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var subDistrict = new SubDistrict()
                {
                    Name = subDistrictRequest.Name,
                    RegionId = subDistrictRequest.RegionId,
                    ModifiedBy = subDistrictRequest.CreatedBy,
                    CreatedBy = subDistrictRequest.CreatedBy,
                    ModifiedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ShippingCharges = subDistrictRequest.ShippingCharges,
                    RowStatus = "A"
                };
                db.SubDistricts.Add(subDistrict);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get sub district with paging
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<SubDistrict>> GetSubDistrict(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get sub district
                var listSubDistricts = db.SubDistricts.ToList();

                if(search != null && search != "")
                {
                    listSubDistricts = listSubDistricts
                        .Where(x =>
                            x.Name.ToLower().Contains(search.ToLower()) ||
                            x.Region.Name.ToLower().Contains(search.ToLower())
                        )
                        .ToList();
                }

                // filter goods by row status
                // with paging
                //listSubDistricts = listSubDistricts
                //    .Where(x => x.RowStatus != "D")
                //    .OrderBy(x => x.Region.Name)
                //    .ThenBy(x => x.Name)
                //    .Skip((currentPage - 1) * limitPage)
                //    .Take(limitPage)
                //    .ToList();

                // without paging
                listSubDistricts = listSubDistricts
                   .Where(x => x.RowStatus != "D")
                   .OrderBy(x=>x.Region.Name)
                   .ThenBy(x=>x.Name)
                   .ToList();

                // get total list of goods
                var totalPages = Math.Ceiling((decimal)listSubDistricts.Count / limitPage);

                // return list of goods
                return new ResponsePagingModel<List<SubDistrict>>()
                {
                    Data = listSubDistricts,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get sub district by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetSubDistrictById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var subDistricts = db.SubDistricts
                    .Where(x => x.Id == id && x.RowStatus != "D")
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        RegionId = x.RegionId,
                        ShippingCharges = x.ShippingCharges
                    })
                    .FirstOrDefault();

                return subDistricts;
            }
        }
    }
}