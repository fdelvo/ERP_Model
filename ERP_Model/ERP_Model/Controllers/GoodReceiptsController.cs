﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERP_Model.Controllers
{
    public class GoodReceiptsController : Controller
    {
        // GET: GoodReceipts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult GoodReceiptItems()
        {
            return View();
        }
    }
}