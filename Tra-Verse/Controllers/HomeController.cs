using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Tra_Verse.Models;
using System.Net.Mail;
using System.Threading.Tasks;
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

        public JArray NASA()
        {
            //string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            HttpWebRequest nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&select=pl_name,st_dist&format=json");
            HttpWebResponse response = (HttpWebResponse)nasaRequest.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JArray nasaJson = JArray.Parse(data);

            rd.Close();

            return nasaJson;
        }

        public JObject Yelp()
        {
            string yelpAPIKey = System.Configuration.ConfigurationManager.AppSettings["Yelp API Key"];
            HttpClient headerToken = new HttpClient();

            headerToken.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", yelpAPIKey);

            string yelpRequest = "https://api.yelp.com/v3/businesses/search?location=bos";
            StreamReader rd = new StreamReader(headerToken.GetStreamAsync(yelpRequest).Result);
            string data = rd.ReadToEnd();

            JObject yelpJson = JObject.Parse(data);

            rd.Close();
            return yelpJson;
        }

        public ActionResult TripList()
        {
            ViewBag.YelpInfo = Yelp();
            ViewBag.NASAInfo = NASA();

            return View();
        }

        public ActionResult PrivateAccomodations(int index)
        {
            ViewBag.YelpInfo = Yelp();
            ViewBag.NASAInfo = NASA();
            UserController.currentUser.CurrentIndex = index;
            ViewBag.Index = UserController.currentUser.CurrentIndex;
            int randPrice = TripPriceRandomizer(index);
            UserController.currentUser.RandPrice = randPrice;
            ViewBag.PricePerDollarSign = randPrice;

            return View();
        }

        public ActionResult PublicAccomodations() { return View(); }

        public ActionResult EditTrip()
        {
            VacationLog vacationToEdit = database.VacationLogs.Find(UserController.currentUser.OrderID);

            ViewBag.Planet = vacationToEdit.PlanetName;
            ViewBag.Rating = vacationToEdit.Rating;
            ViewBag.Price = vacationToEdit.Price;
            ViewBag.Ship = vacationToEdit.ShipOption;
            ViewBag.Start = vacationToEdit.DateStart;
            ViewBag.End = vacationToEdit.DateEnd;

            ViewBag.NASAInfo = NASA();
            ViewBag.YelpInfo = Yelp();
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
                return RedirectToAction("PreOrderError");
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

            TempData["TotalPrice"] = TotalPrice(order.ShipOption, UserController.currentUser.RandPrice);
            int index = UserController.currentUser.CurrentIndex;

            return RedirectToAction("PrivateAccomodations", new { index });
        }

        int TripPriceRandomizer(int index)
        {
            var yelp = Yelp();
            string randomPrice = yelp["businesses"][index]["price"].ToString();
            int pricePerDollarSign = 0;
            Random rand = new Random();

            switch (randomPrice)
            {
                case "$":
                    pricePerDollarSign = rand.Next(10000, 15000);
                    break;
                case "$$":
                    pricePerDollarSign = rand.Next(15001, 20000);
                    break;
                case "$$$":
                    pricePerDollarSign = rand.Next(20001, 25000);
                    break;
                case "$$$$":
                    pricePerDollarSign = rand.Next(25001, 30000);
                    break;
                case "$$$$$":
                    pricePerDollarSign = rand.Next(30001, 40000);
                    break;
                default:
                    break;
            }

            return pricePerDollarSign;
        }

        int TotalPrice(string ship, int dollarSign)
        {
            var Nasa = NASA();
            var takeDistance = Nasa[UserController.currentUser.CurrentIndex]["st_dist"];
            int distance = Convert.ToInt32(takeDistance);

            int pricePerDistance = distance / 100 * 2500;
            int pricePerDollarSign = dollarSign * 1000;
            int priceShipOption = 0;

            switch (ship)
            {
                case "1":
                    priceShipOption = 10000;
                    break;
                case "2":
                    priceShipOption = 20000;
                    break;
                case "3":
                    priceShipOption = 30000;
                    break;
                default:
                    break;
            }

            int totalPrice = pricePerDistance + pricePerDollarSign + priceShipOption;
            return totalPrice;
        }

        public ActionResult Checkout(int price)
        {
            ViewBag.NASAInfo = NASA();
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

        public ActionResult ConfirmationPage(FormCollection fc)
        {

            User user = database.Users.Find(UserController.currentUser.UserID);
            User findEmail = user;
            findEmail.CreditCard = fc["CreditCard"];
            findEmail.CRV = int.Parse(fc["CRV"]);
            findEmail.NameOnCard = fc["NameOnCard"];
            
            //paymentInfo.Email = findEmail.Email;
            database.Entry(findEmail).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            
            //ViewBag.EditedConfirmationPage = "The information on this Confirmation Page has been EDITED";//used in edited method

            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);
            ViewBag.TotalPrice = TotalPrice(vacationInfo.ShipOption, vacationInfo.Price);

            ViewBag.Planet = vacationInfo.PlanetName;
            ViewBag.Rating = vacationInfo.Rating;
            ViewBag.Price = vacationInfo.Price;
            ViewBag.Ship = vacationInfo.ShipOption;
            ViewBag.Start = vacationInfo.DateStart;
            ViewBag.End = vacationInfo.DateEnd;
            ViewBag.Name = findEmail.NameOnCard;
            ViewBag.Card = findEmail.CreditCard;
            //method to send email automatically needs to be implemented still

            //if (!(user.OrderID == -1 || (user.OrderID == (user.OrderID - 10))))
            //{
            //    return RedirectToAction("PreOrderError");
            //}

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

        public ActionResult PreOrderError()
        {
            User findEmail = database.Users.Find(UserController.currentUser.UserID);
            //findEmail.CreditCard = fc["CreditCard"];
            //findEmail.CRV = int.Parse(fc["CRV"]);
            //findEmail.NameOnCard = fc["NameOnCard"];
            //paymentInfo.Email = findEmail.Email;
            //database.Entry(findEmail).State = System.Data.Entity.EntityState.Modified;
            //database.SaveChanges();

            //ViewBag.EditedConfirmationPage = "The information on this Confirmation Page has been EDITED";//used in edited method

            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);
            ViewBag.TotalPrice = TotalPrice(vacationInfo.ShipOption, vacationInfo.Price);

            ViewBag.Planet = vacationInfo.PlanetName;
            ViewBag.Rating = vacationInfo.Rating;
            ViewBag.Price = vacationInfo.Price;
            ViewBag.Ship = vacationInfo.ShipOption;
            ViewBag.Start = vacationInfo.DateStart;
            ViewBag.End = vacationInfo.DateEnd;
            ViewBag.Name = findEmail.NameOnCard;
            ViewBag.Card = findEmail.CreditCard;

            // FIX THIS LAZY CODE..........

            return View();
        }

        public ActionResult RefreshForEdit(VacationLog order)
        {
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError");
            }
            try
            {
                VacationLog vacationToEdit = database.VacationLogs.Find(UserController.currentUser.OrderID);
                vacationToEdit.DateEnd = order.DateEnd;
                vacationToEdit.DateStart = order.DateStart;
                vacationToEdit.ShipOption = order.ShipOption;
                vacationToEdit.Price = TotalPrice(order.ShipOption, UserController.currentUser.RandPrice);
                database.Entry(vacationToEdit).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.Write(e.EntityValidationErrors);
                return View("Error");
            }
            return RedirectToAction("Redo");
        }

        public ActionResult Redo()
        {
            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);

            ViewBag.Planet = vacationInfo.PlanetName;
            ViewBag.Rating = vacationInfo.Rating;
            ViewBag.Price = vacationInfo.Price;
            ViewBag.Ship = vacationInfo.ShipOption;
            ViewBag.Start = vacationInfo.DateStart;
            ViewBag.End = vacationInfo.DateEnd;
            //ViewBag.Name = findEmail.NameOnCard;
            //ViewBag.Card = findEmail.CreditCard;

            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ConfirmationPage(EmailFormModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var body = $"{0}";
        //        var message = new MailMessage();
        //        message.To.Add(new MailAddress(UserController.currentUser.Email));  // replace with valid value 
        //        message.From = new MailAddress("TraVerseAlwaysMovingForward@outlook.com");  // replace with valid value
        //        message.Subject = "Confirmation of your vacation with Tra-Verse";
        //        message.Body = string.Format(model.Message);
        //        message.IsBodyHtml = true;

        //        using (var smtp = new SmtpClient())
        //        {
        //            var credential = new NetworkCredential
        //            {
        //                UserName = "TraVerseAlwaysMovingForward@Outlook.com",  // replace with valid value
        //                Password = "GucciBoi"  // replace with valid value
        //            };
        //            smtp.Credentials = credential;
        //            smtp.Host = "smtp-mail.outlook.com";
        //            smtp.Port = 587;
        //            smtp.EnableSsl = true;
        //            await smtp.SendMailAsync(message);
        //            return RedirectToAction("ConfirmationPage");
        //        }
        //    }
        //    return View(model);
        //}


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

        public ActionResult TripListDist(string distance)
        {
            List<VacationLog> test = database.VacationLogs.ToList();
            test.OrderBy(x => x.Price);
            test.Reverse();

            return RedirectToAction("TripList", "Home");
        }
    }
}