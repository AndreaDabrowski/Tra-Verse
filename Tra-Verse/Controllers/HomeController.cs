using System;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tra_Verse.Controllers;
using System.Linq;
using System.Collections.Generic;
using Tra_Verse.Models;

namespace Tra_Verse.Controllers
{
    public class HomeController : Controller
    {
        TraVerseEntities database = new TraVerseEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditTrip()
        {
            User user = database.Users.Find(UserController.currentUser.UserID);
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError");
            }
            if (user.OrderID <= 0)
            {
                ViewBag.EditError = "You dont have an order to edit - PLEASE LEAVE THIS SITE THANK YOU.";
                return View("Error");
            }
            VacationLog vacationToEdit = database.VacationLogs.Find(UserController.currentUser.OrderID);

            ViewBag.Planet = vacationToEdit.PlanetName;
            ViewBag.Rating = vacationToEdit.Rating;
            ViewBag.Price = vacationToEdit.Price;
            ViewBag.Ship = vacationToEdit.ShipType;
            ViewBag.Start = vacationToEdit.DateStart;
            ViewBag.End = vacationToEdit.DateEnd;

            ViewBag.NASAInfo = API.NASA("notSorted");
            ViewBag.YelpInfo = API.Yelp();
            ViewBag.Index = UserController.currentUser.CurrentIndex;

            return View();
        }

        public ActionResult DeleteTrip(VacationLog deleteOrder)
        {
            List<VacationLog> vacationList = database.VacationLogs.Where(x => x.OrderID == deleteOrder.OrderID).ToList();

            VacationLog found = database.VacationLogs.Find(deleteOrder.OrderID);
            database.VacationLogs.Remove(found);
            database.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Checkout(VacationLog order)
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

            int index = UserController.currentUser.CurrentIndex;

            return RedirectToAction("PrivateAccomodations", new { index });
        }

        public ActionResult RefreshForEdit(VacationLog order)
        {
            User user = database.Users.Find(UserController.currentUser.UserID);
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError");
            }
            else if (user.OrderID <= 0)
            {
                return View("Error");
            }

            try
            {
                VacationLog vacationToEdit = database.VacationLogs.Find(UserController.currentUser.OrderID);
                vacationToEdit.DateEnd = order.DateEnd;
                vacationToEdit.DateStart = order.DateStart;
                vacationToEdit.ShipType = order.ShipType;
                database.Entry(vacationToEdit).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.Write(e.EntityValidationErrors);
                return View("Error");
            }

            TempData["UpdatedOrder"] = "This is your updated order";
            return RedirectToAction("Confirmation Page");
        }

        public ActionResult Checkout(int price)
        {
            ViewBag.NASAInfo = API.NASA("notSorted");
            ViewBag.Index = UserController.currentUser.CurrentIndex;

            VacationLog currentVacation = database.VacationLogs.Find(UserController.currentUser.OrderID);
            TempData["CurrentVacation"] = currentVacation;
            //database.Entry(currentVacation).State = System.Data.Entity.EntityState.Modified;
            //database.SaveChanges();

            return View();
        }

        public ActionResult ProcessPayment(FormCollection fc)
        {

            User findEmail = database.Users.Find(UserController.currentUser.UserID);
            findEmail.CreditCard = fc["CreditCard"];
            findEmail.CRV = int.Parse(fc["CRV"]);
            findEmail.NameOnCard = fc["NameOnCard"];

            ViewBag.EditedConfirmationPage = "The information on this Confirmation Page has been EDITED";//used in edited method

            database.Entry(findEmail).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("ConfirmationPage");
        }

        public ActionResult ConfirmationPage()
        {
            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);

            User user = database.Users.Find(UserController.currentUser.UserID);
            ViewBag.Planet = vacationInfo.PlanetName;
            ViewBag.Rating = vacationInfo.Rating;
            ViewBag.Price = vacationInfo.Price;
            ViewBag.Ship = vacationInfo.ShipType;
            ViewBag.Start = vacationInfo.DateStart;
            ViewBag.End = vacationInfo.DateEnd;

            ViewBag.Name = user.NameOnCard;
            ViewBag.Card = user.CreditCard;

            return View();
        }
        
        public ActionResult Error()
        {

            return View();
        }

        public ActionResult LoginError()
        {
            return View();
        }
    }
}

/* public ActionResult TripList(string price)
     {
         List<VacationLog> test = database.VacationLogs.ToList();
         test.OrderBy(x => x.Price);
         test.Reverse();
         /*if (!string.IsNullOrEmpty(price))
         {
             travelprice = database.VacationLogs.Where(p => p.Price >= lesserPrice && p.Price <= greaterPrice);
         }*/
//return View();
//}

//public ActionResult TripListCruise(string sortOrder)
//{
//    List<VacationLog> test = database.VacationLogs.ToList();
//    test.OrderBy(x => x.Price);
//    test.Reverse();

/*switch (sortOrder)
{
    case "Start Date":
        sortOrder = sortOrder.OrderByDescending(x => x.DateStart);
        break;
    case "Distance":
        sortOrder = sortOrder.OrderByDescending(x => x.Distance);
        break;
    case "Rating":
        sortOrder = sortOrder.OrderByDescending(x => x.Rating);
        break;
    case "Price":
        sortOrder = sortOrder.OrderByDescending(x => x.price);
        break;
    case "date_desc":
        sortOrder = sortOrder.OrderByDescending(x => x.date);
        break;
    default:
        break;
}*/
//travelprice = database.VacationLogs.Where(p => p.Price >= lesserPrice && p.Price <= greaterPrice);
//}*/
//return View();}
