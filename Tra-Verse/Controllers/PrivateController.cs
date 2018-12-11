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

        public ActionResult RefreshForTotal(VacationLog order)
        {
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError");
            }

            User user = database.Users.Find(UserController.currentUser.UserID);

            if (user.OrderID > 0)
            {
                TempData["OrderAlready"] = "This user already has the following order";
                return RedirectToAction("ConfirmationPage");
            }

            try
            {
                VacationLog added = database.VacationLogs.Add(order);
                database.SaveChanges();

                UserController.currentUser.OrderID = added.OrderID;
                User loggedInUser = database.Users.Find(UserController.currentUser.UserID);
                loggedInUser.OrderID = added.OrderID;
                database.Entry(loggedInUser).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.Write(e.EntityValidationErrors);
                return View("Error");
            }

            TempData["TotalPrice"] = Calculation.TotalPrice(order.ShipType, UserController.currentUser.RandPrice);
            int index = UserController.currentUser.CurrentIndex;

            return RedirectToAction("PrivateAccomodations", new { index });
        }
    }
}