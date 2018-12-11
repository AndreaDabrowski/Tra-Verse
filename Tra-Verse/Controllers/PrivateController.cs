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
            ViewBag.Travel = API.Travel();//jobject
            ViewBag.NASA = API.NASA("notSorted");//jarray
            ViewBag.Yelp = API.Yelp();
            ViewBag.PlanetPic = TripListObject.Planets();
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
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult RefreshForTotal(FormCollection variables, int index)

        {
            TempData["ShipType"] = variables["ShipType"];
            TempData["ExoSuit"] = variables["ExoSuit"];
            TempData["Rating"] = variables["Rating"];
            TempData["DateEnd"] = variables["DateEnd"];
            TempData["DateStart"] = variables["DateStart"];
            int pr = int.Parse(variables["BasePrice"]);
            TempData["RefreshedTotal"] = Calculation.TotalPrice(variables["ShipType"], variables["ExoSuit"], pr, variables["Rating"]);

            //output the TLO back into the View
            TripListObject tripIndices = new TripListObject();
            tripIndices.PlanetIndex = int.Parse(variables["PlanetIndex"]);
            tripIndices.CompanyIndex = int.Parse(variables["CompanyIndex"]);
            tripIndices.NumberOfDays = int.Parse(variables["NumberOfDays"]);
            tripIndices.TravelIndex = int.Parse(variables["TravelIndex"]);

            return RedirectToAction("PrivateAccomodations", new { tripIndices, index });//how to send trip indices
        }
    }
}
