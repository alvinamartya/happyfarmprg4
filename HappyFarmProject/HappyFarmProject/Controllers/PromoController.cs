using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProject.Controllers
{
    public class PromoController : Controller
    {
        // GET: Promo
        [Route("~/Promo")]
        public ActionResult Index()
        {
            return View();
        }
    }
}