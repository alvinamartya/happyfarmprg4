using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            Session["UserId"] = null;
            Session["Token"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}