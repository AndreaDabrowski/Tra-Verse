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
        TraVerseEntities database = new TraVerseEntities();

        public ActionResult TripList()
        {
            ViewBag.Travel = API.Travel()["results"];//jobject
            ViewBag.NASA = API.NASA("notSorted");//jarray
            ViewBag.Yelp = API.Yelp();
            ViewBag.PlanetList = TripListObject.Planets();
            ViewBag.TripList = TripListObject.GenerateTrips();

            return View();
        }

        public ActionResult PrivateAccomodations(TripListObject tripIndices, int index)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Travel = API.Travel();
                ViewBag.NASA = API.NASA("notSorted");
                ViewBag.Yelp = API.Yelp();
                ViewBag.TripIndices = tripIndices;
                ViewBag.PlanetPic = TripListObject.Planets();
                ViewBag.Index = index;
                return View();
            }
            else
            {
                ViewBag.ModelNotValid = "Model Not Valid";
                return View("Error", "Home");
            }
        }
    }
}
