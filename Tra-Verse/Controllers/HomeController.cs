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

        public ActionResult EditTripButton()
        {
            User user = database.Users.Find(UserController.currentUser.UserID);
            VacationLog vacationToEdit = database.VacationLogs.Find(user.OrderID);
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError", "User");
            }
            if (user.OrderID <= 0)
            {
                ViewBag.EditError = "You dont have an order to edit - PLEASE LEAVE THIS SITE THANK YOU.";
                return View("Error");
            }
            if (vacationToEdit.ShipType == "Public")
            {
                ViewBag.ChooseNewVacation = "You can't customize cruise-style Vacations, Please choose a new trip";
                return RedirectToAction("PublicTripList");
            }
            else if (vacationToEdit.ShipType == "1" || vacationToEdit.ShipType == "2"|| vacationToEdit.ShipType == "3")
            {
                ViewBag.EditPrivate = "Please edit the Options Below";
                return RedirectToAction("PrivateEditPage");
            }
            else
            {
                ViewBag.SomethingHappened = "Something went wrong but I don't know how you made it here";
                return View("Error");
            }

        }
        public ActionResult EditTripInDB(//form collection)
        {
            //edit deets
            //vb to show updated order
            return RedirectToAction("ConfirmationPage");
        }
        public ActionResult PrivateEditPage()
        {
            User user = database.Users.Find(UserController.currentUser.UserID);
            VacationLog vacationToEdit = database.VacationLogs.Find(user.OrderID);
            ViewBag.Vacation = vacationToEdit;

            return View();
        }

        public ActionResult DeleteTrip()
        {
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError", "User");
            }

            User user = database.Users.Find(UserController.currentUser.UserID);

            if (user.OrderID < 0)
            {
                TempData["NoOrder"] = "This user does not have an order placed...YET";
                return RedirectToAction("ConfirmationPage");
            }
            //List<VacationLog> vacationList = database.VacationLogs.Where(x => x.OrderID == deleteOrder.OrderID).ToList();
            User thisDBUser = database.Users.Find(UserController.currentUser.UserID);
            VacationLog found = database.VacationLogs.Find(thisDBUser.OrderID);
            database.VacationLogs.Remove(found);
            thisDBUser.OrderID = 0;
            database.Entry(thisDBUser).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();

            UserController.currentUser.OrderID = 0;
            return RedirectToAction("Index");
        }

        public ActionResult Checkout(VacationLog order)
        {
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError", "User");
            }

            User user = database.Users.Find(UserController.currentUser.UserID);

            if (user.OrderID > 0)
            {
                TempData["OrderAlready"] = "This user already has the following order";
                return RedirectToAction("Error");
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
                ViewBag.DBError = e.EntityValidationErrors;
                return View("Error");
            }

            VacationLog currentVacation = database.VacationLogs.Find(UserController.currentUser.OrderID);
            TempData["CurrentVacation"] = currentVacation;

            return View();
        }

        public ActionResult RefreshForEdit(VacationLog order)
        {
            User user = database.Users.Find(UserController.currentUser.UserID);
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError", "User");
            }
            else if (user.OrderID <= 0)//Does this need to be here?????
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

        //public ActionResult Checkout(int price)
        //{
        //    ViewBag.NASAInfo = API.NASA("notSorted");
        //    ViewBag.Index = UserController.currentUser.CurrentIndex;

            
        //    //database.Entry(currentVacation).State = System.Data.Entity.EntityState.Modified;
        //    //database.SaveChanges();

        //    return View();
        //}

        public ActionResult ProcessPayment(FormCollection fc)
        {

            User findEmail = database.Users.Find(UserController.currentUser.UserID);
            string creditCard = fc["CreditCard"];
            // change card info as to not allow it to be retrievable from database (also for confirmation page)
            findEmail.CreditCard = "XXXX-XXXX-XXXX-" + creditCard.Substring(11, 4);
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
            ViewBag.Vacation = vacationInfo;
            ViewBag.User = user;

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
