using Microsoft.VisualStudio.TestTools.UnitTesting;
using HappyFarmProjectAPI.Controllers.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyFarmProjectAPI.Models;
using System.Net;

namespace HappyFarmProjectAPI.Controllers.BusinessLogic.Tests
{
    [TestClass()]
    public class PromoLogicTests
    {
        #region Variable
        private PromoLogic promoLogic = new PromoLogic();
        #endregion

        #region Unit Test
        [TestMethod()]
        public void AddPromoTestSuccess()
        {
            var promoRequest = new AddPromoRequest()
            {
                CreatedBy = 1,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.Created, promoLogic.AddPromo(promoRequest).StatusCode);
        }

        [TestMethod()]
        public void AddPromoTestStartDateLessThanNow()
        {
            var promoRequest = new AddPromoRequest()
            {
                CreatedBy = 1,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.AddPromo(promoRequest).StatusCode);
        }

        [TestMethod()]
        public void AddPromoTestEndDateLessThanStartDate()
        {
            var promoRequest = new AddPromoRequest()
            {
                CreatedBy = 1,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(1)
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.AddPromo(promoRequest).StatusCode);
        }

        [TestMethod()]
        public void AddPromoTestUnAuthorized()
        {
            var promoRequest = new AddPromoRequest()
            {
                CreatedBy = 2,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, promoLogic.AddPromo(promoRequest).StatusCode);
        }

        [TestMethod()]
        public void AddPromoTestEmployeeNotFound()
        {
            var promoRequest = new AddPromoRequest()
            {
                CreatedBy = 0,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.AddPromo(promoRequest).StatusCode);
        }

        [TestMethod()]
        public void EditPromoTestSuccess()
        {
            var promoRequest = new EditPromoRequest()
            {
                ModifiedBy = 1,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.OK, promoLogic.EditPromo(1, promoRequest).StatusCode);
        }

        [TestMethod()]
        public void EditPromoTestStartDateLessThanNow()
        {
            var promoRequest = new EditPromoRequest()
            {
                ModifiedBy = 1,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.EditPromo(1, promoRequest).StatusCode);
        }

        [TestMethod()]
        public void EditPromoTestEndDateLessThanStartDate()
        {
            var promoRequest = new EditPromoRequest()
            {
                ModifiedBy = 1,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(2),
                EndDate = DateTime.Now.AddDays(1)
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.EditPromo(1, promoRequest).StatusCode);
        }

        [TestMethod()]
        public void EditPromoTestUnAuthorized()
        {
            var promoRequest = new EditPromoRequest()
            {
                ModifiedBy = 2,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, promoLogic.EditPromo(1, promoRequest).StatusCode);
        }

        [TestMethod()]
        public void EditPromoTestEmployeeNotFound()
        {
            var promoRequest = new EditPromoRequest()
            {
                ModifiedBy = 0,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.EditPromo(1,promoRequest).StatusCode);
        }

        [TestMethod()]
        public void EditPromoTestNotFound()
        {
            var promoRequest = new EditPromoRequest()
            {
                ModifiedBy = 1,
                Name = "Test",
                Discount = 0,
                IsFreeDelivery = "Y",
                MinTransaction = 0,
                MaxDiscount = 0,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2)
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.EditPromo(0, promoRequest).StatusCode);
        }

        [TestMethod()]
        public void GetPromoByIdTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, promoLogic.GetPromoById(1, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetPromoByIdTestNotFound()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.GetPromoById(0, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetPromoByIdUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, promoLogic.GetPromoById(1, "Manager").StatusCode);
        }
        #endregion
    }
}