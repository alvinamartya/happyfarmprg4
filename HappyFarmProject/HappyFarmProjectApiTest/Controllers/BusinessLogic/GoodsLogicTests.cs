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
    public class GoodsLogicTests
    {
        #region Variable
        private GoodsLogic goodsLogic = new GoodsLogic();
        #endregion

        #region Unit Test
        [TestMethod()]
        public void AddGoodsTestSuccess()
        {
            var goodsRequest = new AddGoodsRequest()
            {
                CategoryId = 1,
                CreatedBy = 1,
                Description = "Test",
                Name = "Test1"
            };

            Assert.AreEqual(HttpStatusCode.Created, goodsLogic.AddGoods(goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void AddGoodsTestNameIsExists()
        {
            var goodsRequest = new AddGoodsRequest()
            {
                CategoryId = 1,
                CreatedBy = 1,
                Description = "Test",
                Name = "Test"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, goodsLogic.AddGoods(goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void AddGoodsTestNameUnAuthorized()
        {
            var goodsRequest = new AddGoodsRequest()
            {
                CategoryId = 1,
                CreatedBy = 2, // Neither sa or marketing admin
                Description = "Test",
                Name = "Test"
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, goodsLogic.AddGoods(goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void AddGoodsTestNameEmployeeNotFound()
        {
            var goodsRequest = new AddGoodsRequest()
            {
                CategoryId = 1,
                CreatedBy = 0,
                Description = "Test",
                Name = "Test1"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, goodsLogic.AddGoods(goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void EditGoodsTestSuccess()
        {
            var goodsRequest = new EditGoodsRequest()
            {
                CategoryId = 1,
                ModifiedBy = 1,
                Description = "Test",
                Name = "Test1"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, goodsLogic.EditGoods(1, goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void EditGoodsTestNameIsExists()
        {
            var goodsRequest = new EditGoodsRequest()
            {
                CategoryId = 1,
                ModifiedBy = 1,
                Description = "Test",
                Name = "Test2"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, goodsLogic.EditGoods(1, goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void EditGoodsTestUnAuthorized()
        {
            var goodsRequest = new EditGoodsRequest()
            {
                CategoryId = 1,
                ModifiedBy = 2,  // Neither sa or marketing admin
                Description = "Test",
                Name = "Test1"
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, goodsLogic.EditGoods(1, goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void EditGoodsTestEmployeeNotFound()
        {
            var goodsRequest = new EditGoodsRequest()
            {
                CategoryId = 1,
                ModifiedBy = 0,
                Description = "Test",
                Name = "Test1"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, goodsLogic.EditGoods(1, goodsRequest).StatusCode);
        }

        [TestMethod()]
        public void GetGoodsByIdTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, goodsLogic.GetGoodsById(1, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetGoodsByIdTestNotFound()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, goodsLogic.GetGoodsById(0, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetGoodsByIdUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, goodsLogic.GetGoodsById(1, "Manager").StatusCode);
        }
        #endregion
    }
}