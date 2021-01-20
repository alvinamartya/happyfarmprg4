using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class CustomerFeedbackRepository
    {
        /// <summary>
        /// Get banner with paging
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="limitPage"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponsePagingModel<List<CustomerFeedback>> GetCustomerFeedbacks(int currentPage, int limitPage, string search)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                // get customerFeedbacks
                var customerFeedbacks = db.CustomerFeedbacks.ToList();

                // get total customerFeedbacks
                var totalPages = Math.Ceiling((decimal)customerFeedbacks.Count / limitPage);

                // return customerFeedbacks
                return new ResponsePagingModel<List<CustomerFeedback>>()
                {
                    Data = customerFeedbacks,
                    CurrentPage = currentPage,
                    TotalPage = (int)totalPages
                };
            }
        }

        /// <summary>
        /// Get Customer Feedback by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetCustomerFeedbackById(int id)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                var customerFeedbacks = db.CustomerFeedbacks
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        x.OrderId,
                        x.Rating,
                        x.Note
                    })
                    .FirstOrDefault();

                return customerFeedbacks;
            }
        }
    }
}