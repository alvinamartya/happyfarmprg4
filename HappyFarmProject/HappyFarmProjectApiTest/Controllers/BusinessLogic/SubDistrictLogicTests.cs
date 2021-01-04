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
    public class SubDistrictLogicTests
    {
        #region Variable
        private SubDistrictLogic subDistrictLogic = new SubDistrictLogic();
        #endregion

        #region Unit Test
        [TestMethod()]
        public void AddSubDistrictTestSuccess()
        {
            var subDistrictRequest = new AddSubDistrictRequest()
            {
                CreatedBy = 1,
                Name = "Gading Cempaka1",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.Created, subDistrictLogic.AddSubDistrict(subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void AddSubDistrictTestNameIsExists()
        {
            var subDistrictRequest = new AddSubDistrictRequest()
            {
                CreatedBy = 1,
                Name = "Gading Cempaka",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, subDistrictLogic.AddSubDistrict(subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void AddSubDistrictTestUnAuthorization()
        {
            var subDistrictRequest = new AddSubDistrictRequest()
            {
                CreatedBy = 3, // Neither SA or Manager
                Name = "Gading Cempaka1",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, subDistrictLogic.AddSubDistrict(subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void AddSubDistrictTestEmployeeNotFound()
        {
            var subDistrictRequest = new AddSubDistrictRequest()
            {
                CreatedBy = 0,
                Name = "Gading Cempaka1",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, subDistrictLogic.AddSubDistrict(subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void EditSubDistrictTestSuccess()
        {
            var subDistrictRequest = new EditSubDistrictRequest()
            {
                ModifiedBy = 1,
                Name = "Gading Cempaka",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.OK, subDistrictLogic.EditSubDistrict(1, subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void EditSubDistrictTestNameIsExists()
        {
            var subDistrictRequest = new EditSubDistrictRequest()
            {
                ModifiedBy = 1,
                Name = "Teluk Segara",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, subDistrictLogic.EditSubDistrict(1, subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void EditSubDistrictTestUnAuthorization()
        {
            var subDistrictRequest = new EditSubDistrictRequest()
            {
                ModifiedBy = 3, // Neither SA or Manager
                Name = "Gading Cempaka",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, subDistrictLogic.EditSubDistrict(1, subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void EditSubDistrictTestNotFound()
        {
            var subDistrictRequest = new EditSubDistrictRequest()
            {
                ModifiedBy = 1,
                Name = "Gading Cempaka",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, subDistrictLogic.EditSubDistrict(0, subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void EditSubDistrictTestEmployeeNotFound()
        {
            var subDistrictRequest = new EditSubDistrictRequest()
            {
                ModifiedBy = 0,
                Name = "Gading Cempaka",
                RegionId = 1,
                ShippingCharges = 10000
            };

            Assert.AreEqual(HttpStatusCode.OK, subDistrictLogic.EditSubDistrict(1, subDistrictRequest).StatusCode);
        }

        [TestMethod()]
        public void GetSubDistrictByIdTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, subDistrictLogic.GetSubDistrictById(1, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetSubDistrictByIdTestNotFound()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, subDistrictLogic.GetSubDistrictById(0, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetSubDistrictByIdUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, subDistrictLogic.GetSubDistrictById(1, "Admin Promosi").StatusCode);
        }
        #endregion
    }
}