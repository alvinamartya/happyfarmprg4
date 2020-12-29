using Microsoft.VisualStudio.TestTools.UnitTesting;
using HappyFarmProjectAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyFarmProjectAPI.Models;
using System.Net;

namespace HappyFarmProjectAPI.Controllers.Tests
{
    [TestClass()]
    public class EmployeeLogicTest
    {
        #region Variable
        private EmployeeLogic employeeLogic = new EmployeeLogic();
        #endregion

        #region Unit Test
        [TestMethod()]
        public void AddEmployeeTestSuccess()
        {
            var employeeRequest = new AddEmployeeRequest()
            {
                Address = "Jakarta",
                Email = "manager1@happyfarm.id",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RoleId = 2,
                Username = "manager1"
            };

            Assert.AreEqual(HttpStatusCode.Created, employeeLogic.AddEmployee(employeeRequest, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void AddEmployeeTestUsernameIsExists()
        {
            var employeeRequest = new AddEmployeeRequest()
            {
                Address = "Jakarta",
                Email = "manager@happyfarm.id",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RoleId = 2,
                Username = "sa"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.AddEmployee(employeeRequest, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void AddEmployeeTestEmailIsExists()
        {
            var employeeRequest = new AddEmployeeRequest()
            {
                Address = "Jakarta",
                Email = "sa@happyfarm",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RoleId = 2,
                Username = "manager1"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.AddEmployee(employeeRequest, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void AddEmployeeTestUnAuthorized()
        {
            var employeeRequest = new AddEmployeeRequest()
            {
                Address = "Jakarta",
                Email = "manager1@happyfarm.id",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RoleId = 2,
                Username = "manager1"
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, employeeLogic.AddEmployee(employeeRequest, "Admin Promosi").StatusCode);
        }

        [TestMethod()]
        public void AddEmployeeTestEmailInvalidFormat()
        {
            var employeeRequest = new AddEmployeeRequest()
            {
                Address = "Jakarta",
                Email = "sa@happyfarm",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RoleId = 2,
                Username = "manager1"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.AddEmployee(employeeRequest, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void AddEmployeeTestPhoneInvalidFormat()
        {
            var employeeRequest = new AddEmployeeRequest()
            {
                Address = "Jakarta",
                Email = "sa@happyfarm.id",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821a",
                RoleId = 2,
                Username = "manager1"
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.AddEmployee(employeeRequest, "Super Admin").StatusCode);
        }

        [TestMethod]
        public void EditEmployeeTestSuccess()
        {
            var employeeRequest = new EditEmployeeRequest()
            {
                Email = "manager1@happyfarm.id",
                Address = "Jakarta",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RegionId = 1
            };

            Assert.AreEqual(HttpStatusCode.OK, employeeLogic.EditEmployee(2, employeeRequest, "Super Admin").StatusCode);
        }


        [TestMethod]
        public void EditEmployeeTestUserNotFound()
        {
            var employeeRequest = new EditEmployeeRequest()
            {
                Email = "sa@happyfarm.id",
                Address = "Jakarta",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RegionId = 1
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.EditEmployee(0, employeeRequest, "Manager").StatusCode);
        }

        [TestMethod]
        public void EditEmployeeTestRegionNotFound()
        {
            var employeeRequest = new EditEmployeeRequest()
            {
                Email = "sa@happyfarm.id",
                Address = "Jakarta",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RegionId = 0
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.EditEmployee(1, employeeRequest,"Manager").StatusCode);
        }

        [TestMethod]
        public void EditEmployeeTestUnAuthorized()
        {
            var employeeRequest = new EditEmployeeRequest()
            {
                Email = "sa@happyfarm.id",
                Address = "Jakarta",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RegionId = 1
            };

            Assert.AreEqual(HttpStatusCode.Unauthorized, employeeLogic.EditEmployee(1, employeeRequest, "Admin Promosi").StatusCode);
        }

        [TestMethod()]
        public void EditEmployeeTestEmailInvalidFormat()
        {
            var employeeRequest = new EditEmployeeRequest()
            {
                Email = "sa@happyfarm",
                Address = "Jakarta",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RegionId = 1
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.EditEmployee(1, employeeRequest, "Manager").StatusCode);
        }

        [TestMethod()]
        public void EditEmployeeTestEmailIsExists()
        {
            var employeeRequest = new EditEmployeeRequest()
            {
                Email = "sa@happyfarm",
                Address = "Jakarta",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821",
                RegionId = 1
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.EditEmployee(2, employeeRequest, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void EditEmployeeTestPhoneInvalidFormat()
        {
            var employeeRequest = new EditEmployeeRequest()
            {
                Email = "sa@happyfarm.id",
                Address = "Jakarta",
                Gender = "M",
                Name = "Manager",
                PhoneNumber = "0821a",
                RegionId = 1
            };

            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.EditEmployee(1, employeeRequest, "Manager").StatusCode);
        }

        [TestMethod()]
        public void DeleteEmployeeTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, employeeLogic.DeleteEmployee(2, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void DeleteEmployeeTestFailed()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.DeleteEmployee(0, "Manager").StatusCode);
        }

        [TestMethod()]
        public void DeleteEmployeeTestUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.Unauthorized, employeeLogic.DeleteEmployee(1, "Admin Promosi").StatusCode);
        }

        [TestMethod()]
        public void GetEmployeeByIdTestSuccess()
        {
            Assert.AreEqual(HttpStatusCode.OK, employeeLogic.GetEmployeeById(2, "Super Admin").StatusCode);
        }

        [TestMethod()]
        public void GetEmployeeByIdTestFailed()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest, employeeLogic.GetEmployeeById(0, "Manager").StatusCode);
        }

        [TestMethod()]
        public void GetEmployeeByIdTestUnAuthorized()
        {
            Assert.AreEqual(HttpStatusCode.Unauthorized, employeeLogic.GetEmployeeById(1, "Admin Promosi").StatusCode);
        }

        #endregion
    }
}