﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyFarmProjectWebAdmin.Controllers
{
    public class ManagerDashboardController : Controller
    {
        // GET: ManagerDashboard
        [Route("~/Dashboard")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}