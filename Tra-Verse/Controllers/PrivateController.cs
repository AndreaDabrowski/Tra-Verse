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
        public ActionResult RefreshForTotal(FormCollection variables)
        {
            //current user rand price for YELP $$$ calc
            TempData["ShipType"] = variables["ShipType"];
            TempData["ExoSuit"] = variables["ExoSuit"];
            TempData["Rating"] = variables["Rating"];
            TempData["DateEnd"] = variables["DateEnd"];
            TempData["DateStart"] = variables["DateStart"];
            int pr = int.Parse(variables["BasePrice"]);
            TempData["RefreshedTotal"] = Calculation.TotalPrice(variables["ShipType"], variables["ExoSuit"], pr, variables["Rating"]);
            //int index = UserController.currentUser.CurrentIndex;

            return RedirectToAction("PrivateAccomodations");//how to send trip indices
        }
    }
}
