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
            ViewBag.TripPrice = TripPriceRandomizer(index);
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

            var yelp = Yelp();
            int index = UserController.currentUser.CurrentIndex;
            int yelpPrice = Convert.ToInt32(yelp["businesses"][index]["price"]);
            TempData["TotalPrice"] = TotalPrice(order.ShipOption, yelpPrice);

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

        int TotalPrice(string ship, int price)
        {
            var Nasa = NASA();
            var placeholder = Nasa[UserController.currentUser.CurrentIndex]["st_dist"];
            int PH = Convert.ToInt32(placeholder);

            int pricePerDistance = PH / 100 * 25000;
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

        public ActionResult Checkout(int price)
        {
            ViewBag.NASAInfo = NASA();
            ViewBag.Index = UserController.currentUser.CurrentIndex;

            VacationLog currentVacation = database.VacationLogs.Find(UserController.currentUser.OrderID);
            currentVacation.Price = price;
            database.Entry(currentVacation).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();

            return View();//input order object here later??
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

            ViewBag.EditedConfirmationPage = "The information on this Confirmation Page has been EDITED";//used in edited method
            ViewBag.NASAInfo = NASA();
            ViewBag.YelpInfo = Yelp();
            ViewBag.Index = UserController.currentUser.CurrentIndex;

            VacationLog vacationInfo = database.VacationLogs.Find(paymentInfo.OrderID);
            ViewBag.TotalPrice = TotalPrice(vacationInfo.ShipOption, vacationInfo.Price);

            ViewBag.VacationLogDBInfo = vacationInfo;
            ViewBag.UserDBInfo = paymentInfo;
            //method to send email automatically??

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmationPage(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                //var body = $"{0}";
                var message = new MailMessage();
                message.To.Add(new MailAddress(UserController.currentUser.Email));  // replace with valid value 
                message.From = new MailAddress("TraVerseAlwaysMovingForward@outlook.com");  // replace with valid value
                message.Subject = "Confirmation of your vacation with Tra-Verse";
                message.Body = string.Format(model.Message);
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
            return View(model);
        }

        public ActionResult ConfirmationEmailFormat()
        {
            return View();
        }
    }
}