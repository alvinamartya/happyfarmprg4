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
    public class RegionLogicTests
    {
        #region Variable
        private RegionLogic regionLogic = new RegionLogic();
        #endregion

        #region Unit Test
        [TestMethod()]
        public void AddRegionTestSuccess()
        {
            var regionRequest = new AddRegionRequest()
            {
                CreatedBy = 1,
                Name = "Jakarta1"
            };

            Assert.AreEqual(HttpStatusCode.Created, regionLogic.AddRegion(regionRequest).StatusCode);
        }

        [TestMethod()]
        public void AddRegionTestNameIsExists()
        {
            var regionRequest = new AddRegionRequest()
            {
                CreatedBy = 1,
                Name = "Jakarta"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, regionLogic.AddRegion(regionRequest).StatusCode);
        }

        [TestMethod()]
        public void AddRegionTestUnAuthorization()
        {
            var regionRequest = new AddRegionRequest()
            {
                CreatedBy = 3, // Neither sa or manager
                Name = "Jakarta"
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, regionLogic.AddRegion(regionRequest).StatusCode);
        }

        [TestMethod()]
        public void AddRegionTestEmployeeNotFound()
        {
            var regionRequest = new AddRegionRequest()
            {
                CreatedBy = 0,
                Name = "Jakarta"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, regionLogic.AddRegion(regionRequest).StatusCode);
        }

        [TestMethod()]
        public void EditRegionTestSuccess()
        {
            var regionRequest = new EditRegionRequest()
            {
                ModifiedBy = 1,
                Name = "Jakarta"
            };

            Assert.AreEqual(HttpStatusCode.OK, regionLogic.EditRegion(1, regionRequest).StatusCode);
        }

        [TestMethod()]
        public void EditRegionTestNameIsExists()
        {
            var regionRequest = new EditRegionRequest()
            {
                ModifiedBy = 1,
                Name = "Bengkulu"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, regionLogic.EditRegion(1, regionRequest).StatusCode);
        }

        [TestMethod()]
        public void EditRegionTestUnAuthorization()
        {
            var regionRequest = new EditRegionRequest()
            {
                ModifiedBy = 3, // Neither sa or manager
                Name = "Jakarta"
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, regionLogic.EditRegion(1, regionRequest).StatusCode);
        }

        [TestMethod()]
        public void EditRegionTestNotFound()
        {
            var regionRequest = new EditRegionRequest()
            {
                ModifiedBy = 1, 
                Name = "Jakarta"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, regionLogic.EditRegion(0, regionRequest).StatusCode);
        }

        [TestMethod()]
        public void EditRegionTestEmployeeNotFound()
        {
            var regionRequest = new EditRegionRequest()
            {
                ModifiedBy = 0,
                Name = "Jakarta"
            };

            Assert.AreEqual(HttpStatusCode.OK, regionLogic.EditRegion(1, regionRequest).StatusCode);
        }

        [TestMethod()]
        public void GetRegionByIdTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, regionLogic.GetRegionById(1, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetRegionByIdTestNotFound()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, regionLogic.GetRegionById(0, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetRegionByIdUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, regionLogic.GetRegionById(1, "Admin Promosi").StatusCode);
        }
        #endregion
    }
}