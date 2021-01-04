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
    public class CategoryLogicTests
    {
        #region Variable
        private CategoryLogic categoryLogic = new CategoryLogic();
        #endregion

        #region Unit Test
        [TestMethod()]
        public void AddCategoryTestSuccess()
        {
            var categoryRequest = new AddCategoryRequest()
            {
                CreatedBy = 1,
                Name = "Buah1"
            };

            Assert.AreEqual(HttpStatusCode.Created, categoryLogic.AddCategory(categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void AddCategoryTestNameIsExists()
        {
            var categoryRequest = new AddCategoryRequest()
            {
                CreatedBy = 1,
                Name = "Buah"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, categoryLogic.AddCategory(categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void AddCategoryTestUnAuthorization()
        {
            var categoryRequest = new AddCategoryRequest()
            {
                CreatedBy = 2, // Neither SA or Production Admin
                Name = "Buah1"
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, categoryLogic.AddCategory(categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void AddCategoryTestEmployeeNotFound()
        {
            var categoryRequest = new AddCategoryRequest()
            {
                CreatedBy = 0,
                Name = "Buah1"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, categoryLogic.AddCategory(categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void EditCategoryTestSuccess()
        {
            var categoryRequest = new EditCategoryRequest()
            {
                ModifiedBy = 1,
                Name = "Buah"
            };

            Assert.AreEqual(HttpStatusCode.OK, categoryLogic.EditCategory(1, categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void EditCategoryTestNameIsExists()
        {
            var categoryRequest = new EditCategoryRequest()
            {
                ModifiedBy = 1,
                Name = "Sayur"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, categoryLogic.EditCategory(1, categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void EditCategoryTestUnAuthorization()
        {
            var categoryRequest = new EditCategoryRequest()
            {
                ModifiedBy = 2, // Neither SA or Production Admin
                Name = "Buah"
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, categoryLogic.EditCategory(1, categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void EditCategoryTestNotFound()
        {
            var categoryRequest = new EditCategoryRequest()
            {
                ModifiedBy = 1,
                Name = "Buah"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, categoryLogic.EditCategory(0, categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void EditCategoryTestEmployeeNotFound()
        {
            var categoryRequest = new EditCategoryRequest()
            {
                ModifiedBy = 0,
                Name = "Buah"
            };

            Assert.AreEqual(HttpStatusCode.OK, categoryLogic.EditCategory(1, categoryRequest).StatusCode);
        }

        [TestMethod()]
        public void GetCategoryByIdTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, categoryLogic.GetCategoryById(1, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetCategoryByIdTestNotFound()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, categoryLogic.GetCategoryById(0, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetCategoryByIdUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, categoryLogic.GetCategoryById(1, "Manager").StatusCode);
        }
        #endregion
    }
}