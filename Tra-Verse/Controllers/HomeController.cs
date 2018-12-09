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
            ViewBag.YelpInfo = API.Yelp();
            ViewBag.NASAInfo = API.NASA("notSorted");

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
            ViewBag.Ship = vacationToEdit.ShipOption;
            ViewBag.Start = vacationToEdit.DateStart;
            ViewBag.End = vacationToEdit.DateEnd;

            ViewBag.NASAInfo = API.NASA("notSorted");
            ViewBag.YelpInfo = API.Yelp();
            ViewBag.Index = UserController.currentUser.CurrentIndex;

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

            TempData["TotalPrice"] = Calculation.TotalPrice(order.ShipOption, UserController.currentUser.RandPrice);
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
            ViewBag.TotalPrice = Calculation.TotalPrice(vacationInfo.ShipOption, vacationInfo.Price);

            User user = database.Users.Find(UserController.currentUser.UserID);
            ViewBag.Planet = vacationInfo.PlanetName;
            ViewBag.Rating = vacationInfo.Rating;
            ViewBag.Price = vacationInfo.Price;
            ViewBag.Ship = vacationInfo.ShipOption;
            ViewBag.Start = vacationInfo.DateStart;
            ViewBag.End = vacationInfo.DateEnd;

            ViewBag.Name = user.NameOnCard;
            ViewBag.Card = user.CreditCard;

            return View();
        }

        public ActionResult DeleteTrip()
        {
            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);
            database.VacationLogs.Remove(vacationInfo);
            User userInfo = database.Users.Find(UserController.currentUser.UserID);
            UserController.currentUser.OrderID = 0;
            userInfo.OrderID = -1;

            database.Entry(userInfo).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();

            TempData["deleted"] = "Your stuff has been deleted, yo";
            return RedirectToAction("TripList");
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
                vacationToEdit.ShipOption = order.ShipOption;
                vacationToEdit.Price = Calculation.TotalPrice(order.ShipOption, UserController.currentUser.RandPrice);
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmationPageEmail()
        {
            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);
            User user = database.Users.Find(UserController.currentUser.UserID);
            //var body = $"{0}";
            var message = new MailMessage();
                message.To.Add(new MailAddress(UserController.currentUser.Email));  // replace with valid value 
                message.From = new MailAddress("TraVerseAlwaysMovingForward@outlook.com");  // replace with valid value
                message.Subject = "Confirmation of your vacation with Tra-Verse";
                message.Body = string.Format("<p>Confirmation of your vacation with Traverse</p>" +
                "<p>Trip Details: </p>" +
                "<p>Planet Name: " + vacationInfo.PlanetName.ToString() + "</p>" +
                "<p>Vacation Rating: " + vacationInfo.Rating.ToString() + "</p>"+
                "<p>Ship Choice: " + vacationInfo.ShipOption.ToString() +"</p>"+
                "Departure Date: " + vacationInfo.DateStart.ToString() + "</p>" +
                "Return Date: " + vacationInfo.DateEnd.ToString() + "</p>" +
                "</br>"+
                "TOTAL: " + vacationInfo.Price.ToString() + "</p>" +
                "</br>"+
                "<p>This amount was charged to: " + user.NameOnCard.ToString() + "</p>" +
                "<p>Card number: " + user.CreditCard.ToString() + "</p>" +
                "Thank you!");
                /*XXXX-XXXX-XXXX-*/
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "TraVerseAlwaysMovingForward@Outlook.com",  // replace with valid value
                        Password = "GucciBoi"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("ConfirmationPage");
                }
        }

        /* List<VacationLog> test = database.VacationLogs.ToList();
                    test.OrderBy(x => x.Price);
                    test.Reverse();*/
        /*if (!string.IsNullOrEmpty(price))
        {
        
        
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
        //    return RedirectToAction("TripList", "Home");
        //}
    }
}