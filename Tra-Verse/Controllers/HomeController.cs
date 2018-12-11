using System;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using Tra_Verse.Models;
using System.Linq;
using System.Collections.Generic;


namespace Tra_Verse.Controllers
{
    public class HomeController : Controller
    {
        //UserController UC = new UserController();
        TraVerseEntities database = new TraVerseEntities();

        public ActionResult Index()
        {
            ViewBag.Title = "Always Moving Forward";
            return View();
        }

        public ActionResult TripList()
        {
            ViewBag.Travel = API.Travel()["results"];//jobject
            ViewBag.NASA = API.NASA("notSorted");//jarray
            ViewBag.Yelp = API.Yelp();
            ViewBag.PlanetPic = TripListObject.Planets();
            ViewBag.TripList = TripListObject.GenerateTrips();

            return View();
            //ViewBag.YelpInfo = API.Yelp();
            //ViewBag.NASAInfo = API.NASA("notSorted");

            //return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoggedIn()
        {
            return View();
        }

        public ActionResult Registered()
        {
            return View();
        }

        public ActionResult PrivateAccomodations(TripListObject tripIndices)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Travel = API.Travel();
                ViewBag.NASA = API.NASA("notSorted");
                ViewBag.Yelp = API.Yelp();
                ViewBag.TripIndices = tripIndices;
                ViewBag.PlanetPic = TripListObject.Planets();

                return View();
            }
            else
            {
                ViewBag.ModelNotValid = "Model Not Valid";
                return View("Error", "Home");
            }
            //ViewBag.YelpInfo = API.Yelp();
            //ViewBag.NASAInfo = API.NASA("notSorted");
            //UserController.currentUser.CurrentIndex = index;
            //ViewBag.Index = UserController.currentUser.CurrentIndex;
            //int randPrice = Calculation.TripPriceRandomizer(index);
            //UserController.currentUser.RandPrice = randPrice;
            //ViewBag.PricePerDollarSign = randPrice;

            //return View();
        }

        public ActionResult EditTrip()
        {
            User user = database.Users.Find(UserController.currentUser.UserID);
            if(UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError");
            }
            if(user.OrderID <=0)
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

        public ActionResult DeleteTrip()
        {
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
            //int randPrice = UserController.currentUser.RandPrice;

            TempData["TotalPrice"] = Calculation.TotalPrice(order.ShipType, UserController.currentUser.RandPrice);
            int index = UserController.currentUser.CurrentIndex;

            return RedirectToAction("PrivateAccomodations", new { index });
        }

        public ActionResult Checkout(int price)
        {
            ViewBag.NASAInfo = API.NASA("notSorted");
            ViewBag.Index = UserController.currentUser.CurrentIndex;

            VacationLog currentVacation = database.VacationLogs.Find(UserController.currentUser.OrderID);
            currentVacation.Price = price;
            database.Entry(currentVacation).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();

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
            //ViewBag.EditedConfirmationPage = "The information on this Confirmation Page has been EDITED";//used in edited method

            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);
            ViewBag.TotalPrice = Calculation.TotalPrice(vacationInfo.ShipType, vacationInfo.Price);

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

        public ActionResult RefreshForEdit(VacationLog order)
        {
            User user = database.Users.Find(UserController.currentUser.UserID);
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError");
            }
            else if (user.OrderID<=0)
            {
                return View("Error");
            }
            try
            {
                VacationLog vacationToEdit = database.VacationLogs.Find(UserController.currentUser.OrderID);
                vacationToEdit.DateEnd = order.DateEnd;
                vacationToEdit.DateStart = order.DateStart;
                vacationToEdit.ShipType = order.ShipType;
                vacationToEdit.Price = Calculation.TotalPrice(order.ShipType, UserController.currentUser.RandPrice);
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
        //return View();
        //}