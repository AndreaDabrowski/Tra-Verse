using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tra_Verse.Models;

namespace Tra_Verse.Controllers
{
    public class PrivateController : Controller
    {
        //TraVerseEntities database = new TraVerseEntities();

        public ActionResult TripList()
        {
            ViewBag.Travel = API.Travel()["results"];//jobject
            ViewBag.NASA = API.NASA("notSorted");//jarray
            ViewBag.Yelp = API.Yelp();
            ViewBag.PlanetList = TripListObject.Planets();
            ViewBag.TripList = TripListObject.GenerateTrips();

            return View();
        }

        public ActionResult PrivateAccomodations(int index)
        {
            ViewBag.YelpInfo = API.Yelp();
            ViewBag.NASAInfo = API.NASA("notSorted");
            UserController.currentUser.CurrentIndex = index;
            ViewBag.Index = UserController.currentUser.CurrentIndex;
            int randPrice = Calculation.TripPriceRandomizer(index);
            UserController.currentUser.RandPrice = randPrice;
            ViewBag.PricePerDollarSign = randPrice;

            return View();
        }
    }
}