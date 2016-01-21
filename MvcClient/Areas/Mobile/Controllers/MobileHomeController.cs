using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.Areas.Mobile.Controllers
{
    public class MobileHomeController : Controller
    {
        // GET: Mobile/MobileHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShoopingCar()
        {
            return View();
        }
    }
}