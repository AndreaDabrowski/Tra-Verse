using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tra_Verse.Models;

namespace Tra_Verse.Controllers
{
    public class PrivateController : Controller
    {

        public ActionResult TripList()
        {
            ViewBag.Travel = API.Travel();//jobject
            ViewBag.NASA = API.NASA("notSorted");//jarray
            ViewBag.Yelp = API.Yelp();
            ViewBag.PlanetNasaLink = TripListObject.PlanetImagingSystem();
            ViewBag.PlanetPic = TripListObject.Planets();
            ViewBag.TripList = TripListObject.GenerateTrips();

            return View();
        }

        public ActionResult PrivateAccomodations(TripListObject tripIndices, int index)
        {
            if (ModelState.IsValid)
            {
                Date test = new Date();
                ViewBag.Travel = API.Travel();
                ViewBag.NASA = API.NASA("notSorted");
                ViewBag.Yelp = API.Yelp();
                ViewBag.TripIndices = tripIndices;
                ViewBag.PlanetPic = TripListObject.Planets();
                ViewBag.PlanetNasaLink = TripListObject.PlanetImagingSystem();
                ViewBag.Index = index;
                ViewBag.Date = test;
                return View();
            }
            else
            {
                ViewBag.ModelNotValid = "Model Not Valid";
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
