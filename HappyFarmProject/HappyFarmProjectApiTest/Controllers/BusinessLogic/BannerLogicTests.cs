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
    public class BannerLogicTests
    {
        #region Variable
        private BannerLogic bannerLogic = new BannerLogic();
        #endregion

        #region Unit Test
        [TestMethod()]
        public void AddBannerTestSuccess()
        {
            var bannerRequest = new AddBannerRequest()
            {
                Name = "Test",
                CreatedBy = 1
            };

            Assert.AreEqual(HttpStatusCode.Created, bannerLogic.AddBanner(bannerRequest).StatusCode);
        }

        [TestMethod()]
        public void AddBannerTestUnAuthorized()
        {
            var bannerRequest = new AddBannerRequest()
            {
                Name = "Test",
                CreatedBy = 2 // Neither Super Admin or Promotion Admin
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, bannerLogic.AddBanner(bannerRequest).StatusCode);
        }

        [TestMethod()]
        public void AddBannerTestEmployeeNotFound()
        {
            var bannerRequest = new AddBannerRequest()
            {
                Name = "Test",
                CreatedBy = 0
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, bannerLogic.AddBanner(bannerRequest).StatusCode);
        }

        [TestMethod()]
        public void EditBannerTestSuccess()
        {
            var bannerRequest = new EditBannerRequest()
            {
                Name = "Test",
                ModifiedBy = 1
            };

            Assert.AreEqual(HttpStatusCode.OK, bannerLogic.EditBanner(1, bannerRequest).StatusCode);
        }

        [TestMethod()]
        public void EditBannerTestUnAuthorized()
        {
            var bannerRequest = new EditBannerRequest()
            {
                Name = "Test",
                ModifiedBy = 2 // Neither Super Admin or Promotion Admin
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, bannerLogic.EditBanner(1, bannerRequest).StatusCode);
        }

        [TestMethod()]
        public void EditBannerTestEmployeeNotFound()
        {
            var bannerRequest = new EditBannerRequest()
            {
                Name = "Test",
                ModifiedBy = 0
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, bannerLogic.EditBanner(1, bannerRequest).StatusCode);
        }


        [TestMethod()]
        public void EditBannerTestNotFound()
        {
            var bannerRequest = new EditBannerRequest()
            {
                Name = "Test",
                ModifiedBy = 1
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, bannerLogic.EditBanner(0, bannerRequest).StatusCode);
        }

        [TestMethod()]
        public void GetBannerByIdTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, bannerLogic.GetBannerById(1, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetBannerByIdTestNotFound()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, bannerLogic.GetBannerById(0, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetBannerByIdUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, bannerLogic.GetBannerById(1, "Manager").StatusCode);
        }
        #endregion
    }
}