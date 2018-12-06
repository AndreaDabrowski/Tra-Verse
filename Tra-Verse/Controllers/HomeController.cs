using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity.Validation;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Tra_Verse.Models;


//testing this commit
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
            HttpWebRequest nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&format=json&select=pl_name,st_dist");
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

            return View();
        }

        public ActionResult PublicAccomodations() { return View(); }

        public ActionResult EditTrip() { return View(); }

        public ActionResult RefreshForTotal(VacationLog order)
        {
            if (UserController.currentUser.LoggedIn == false)
            {
                return View("LoginError");
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
            TempData["TotalPrice"] = TotalPrice(order.ShipOption, order.Price);

            return RedirectToAction("PrivateAccomodations", new { index });
        }

        private int TotalPrice(string ship, int price)
        {
            var Nasa = NASA();
            string placeholder = Nasa[UserController.currentUser.CurrentIndex]["st_dist"].ToString();
            int PH = int.Parse(placeholder);

            int pricePerDistance = PH / 100 * 200000;
            int pricePerDollarSign = price * 10000;
            int priceShipOption = 0;

            switch (ship)
            {
                case "1":
                    priceShipOption = 100000;
                    break;
                case "2":
                    priceShipOption = 200000;
                    break;
                case "3":
                    priceShipOption = 300000;
                    break;
                default:
                    break;
            }

            int totalPrice = pricePerDistance + pricePerDollarSign + priceShipOption;

            return totalPrice;
        }

        public ActionResult Checkout(VacationLog order)
        {
            ViewBag.NASAInfo = NASA();
            ViewBag.Index = UserController.currentUser.CurrentIndex;
            return View();//input order object here later
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult LoginError()
        {
            return View();
        }

        public ActionResult ConfirmationPage(User paymentInfo)
        {
            paymentInfo.UserID = UserController.currentUser.UserID;
            User findEmail = database.Users.Find(UserController.currentUser.UserID);
            paymentInfo.Email = findEmail.Email;
            database.Entry(paymentInfo).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();

            ViewBag.EditedConfirmationPage = "The information on this Confirmation Page has been EDITED";
            ViewBag.NASAInfo = NASA();
            ViewBag.YelpInfo = Yelp();
            ViewBag.Index = UserController.currentUser.CurrentIndex;
            //method to send email automatically

            return View();
        } 

        //public ActionResult EditConfirmationPage () { return View(); }   ???? Do we need this, or can we just use the ConfirmationPage()
    }
}