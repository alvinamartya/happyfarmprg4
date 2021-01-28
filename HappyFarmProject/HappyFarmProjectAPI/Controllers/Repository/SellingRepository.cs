using HappyFarmProjectAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyFarmProjectAPI.Controllers.Repository
{
    public class SellingRepository
    {
        public List<Selling> GetHistoryTransaction(int customerId)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                List<Selling> sellings = db.Sellings.Where(x => x.CustomerId == customerId).ToList();
                return sellings;
            }
        }

        public List<SellingDetail> GetSellingDetail(int sellingId)
        {
            using(HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                List<SellingDetail> details = db.SellingDetails.Where(x => x.SellingId == sellingId).ToList();
                return details;
            }
        }

        public void UploadBuktiTransfer(UploadBuktiTransfer request)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                Selling selling = db.Sellings.Where(x => x.Id == request.Id).FirstOrDefault();
                selling.PaymentImage = request.ImagePath;

                SellingActivity activity = new SellingActivity()
                {
                    SellingId = selling.Id,
                    SellingStatusid = 2,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = DateTime.Now
                };

                db.SellingActivities.Add(activity);
                db.SaveChanges();
            }
        }

        public void CreateCustomerFeedback(CreateCustomerFeedback createCustomerFeedback)
        {
            using (HappyFarmPRG4Entities db = new HappyFarmPRG4Entities())
            {
                CustomerFeedback customerFeedback = new CustomerFeedback()
                {
                    OrderId = createCustomerFeedback.Id,
                    Note = createCustomerFeedback.Note,
                    Rating = createCustomerFeedback.Rating
                };

                db.CustomerFeedbacks.Add(customerFeedback);

                SellingActivity activity = new SellingActivity()
                {
                    SellingId = createCustomerFeedback.Id,
                    SellingStatusid = 6,
                    CreatedBy = createCustomerFeedback.CreatedBy,
                    CreatedAt = DateTime.Now
                };

                db.SellingActivities.Add(activity);
                db.SaveChanges();
            }
        }
    }
}