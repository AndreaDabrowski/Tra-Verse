using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tra_Verse.Models;

namespace Tra_Verse.Controllers
{
    public class PublicController : Controller
    {
        // GET: Public
        public ActionResult PublicAccomodations()
        {
            ViewBag.YelpInfo = API.Travel();
            ViewBag.NASAInfo = API.NASA();
            return View();
        }
        public ActionResult PublicTripList()
        {
            ViewBag.Travel = API.Travel();//jobject
            ViewBag.NASA = API.NASA();//jarray
            Random rand = new Random();
            int companyShipOption = rand.Next(1, 4);
            ViewBag.CompanyShipOption = companyShipOption;

            return View();
        }

    }
}