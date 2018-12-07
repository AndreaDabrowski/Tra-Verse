using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tra_Verse.Controllers
{
    public class PublicController : Controller
    {
        HomeController HC = new HomeController();
        // GET: Public
        public ActionResult PublicAccomodations()
        {
            HC.Travel();
            HC.NASA();
            return View();
        }
        public ActionResult PublicTripList()
        {
            ViewBag.Travel = HC.Travel();
            ViewBag.NASA = HC.NASA();
            return View();
        }
    }
}