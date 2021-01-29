using HappyFarmProjectWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [Route("~/User/Profil")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //[Route("~/User/Profil")]
        //[HttpPost]
        //public ActionResult Index(EmployeeProfileModelView employeeProfile)
        //{

        //}
    }
}