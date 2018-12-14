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
        public ActionResult PublicAccomodations(TripListObject tripIndices)
        {

            if (ModelState.IsValid)
            {
                ViewBag.Travel = API.Travel();
                ViewBag.NASA = API.NASA("notSorted");
                ViewBag.Yelp = API.Yelp();
                ViewBag.TripIndices = tripIndices;
                ViewBag.PlanetPic = TripListObject.Planets();
                ViewBag.PlanetNasaLink = TripListObject.PlanetImagingSystem();
                DateTime DTDeparture = DateTime.Parse(tripIndices.DepartureDate);
                DateTime returnDate = DTDeparture.AddDays(tripIndices.NumberOfDays);
                string format = returnDate.ToString("MM/dd/yyyy hh:mm");
                ViewBag.ReturnDate = format;

                return View();
            }
            else
            {
                ViewBag.ModelNotValid = "Model Not Valid";
                return View("Error", "User");
            }
        }

        public ActionResult PublicTripList()
        {
            ViewBag.Travel = API.Travel()["results"];//jobject
            ViewBag.NASA = API.NASA("notSorted");//jarray
            ViewBag.Yelp = API.Yelp();
            ViewBag.PlanetPic = TripListObject.Planets();
            ViewBag.TripList = TripListObject.GenerateTrips();
            ViewBag.PlanetNasaLink = TripListObject.PlanetImagingSystem();
            ViewBag.DepartureDate = Date.DepartureDate(30);
            return View();
        }
    }
}