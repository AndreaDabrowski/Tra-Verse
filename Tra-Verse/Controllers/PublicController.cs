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
            ViewBag.Travel = API.Travel();
            ViewBag.NASA = API.NASA("notSorted");
            return View();
        }
        public ActionResult PublicTripList()
        {
            ViewBag.Travel = API.Travel();//jobject
            ViewBag.NASA = API.NASA("notSorted");//jarray
            
            //ViewBag.CompanyShipOption = companyShipOption;

            return View();
        }

    }
}