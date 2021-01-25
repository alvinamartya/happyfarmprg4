using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Route("~/Home")]
        public ActionResult Index()
        {
            return View();
        }
    }
}