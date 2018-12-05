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
        UserController UC = new UserController();
        TraVerseEntities database = new TraVerseEntities();

        public ActionResult Index()
        {
            ViewBag.Title = "Always Moving Forward";
            return UC.Index();
        }

        public JArray NASA()
        {
            //string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            HttpWebRequest nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&format=json&select=pl_name,st_dist");
            HttpWebResponse response = (HttpWebResponse)nasaRequest.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JArray nasaJson = JArray.Parse(data);
            //ViewBag.Example = nasaJson;

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
            //ViewBag.YelpInfo = yelpJson;
            rd.Close();

            return yelpJson;
        }

        public ActionResult TripList()
        {
            ViewBag.YelpInfo = Yelp();
            ViewBag.NASAInfo = NASA();

            return View();
        }

        public ActionResult TripDetails() { return View(); }

        public ActionResult PrivateAccomodations(int index)
        {
            ViewBag.YelpInfo = Yelp();
            ViewBag.NASAInfo = NASA();
            UC.currentUser.CurrentIndex = index;
            ViewBag.Index = index;

            return View();
        }

        public ActionResult PublicAccomodations() { return View(); }

        public ActionResult EditTrip() { return View(); }


        public ActionResult CheckOut(VacationLog order)
        {
            try
            {
                VacationLog added = database.VacationLogs.Add(order);
                database.SaveChanges();

                UC.currentUser.OrderID = added.OrderID;
                User loggedInUser = database.Users.Find(UC.currentUser.UserID);
                loggedInUser.OrderID = added.OrderID;
                database.Entry(loggedInUser).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.Write(e.EntityValidationErrors);
                return View("Error");
            }
            ViewBag.Index = UC.currentUser.CurrentIndex;
            return View();//input order object here later
        } 
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult ConfirmationPage(User paymentInfo, int index)
        {
            paymentInfo.UserID = UC.currentUser.UserID;
            User findEmail = database.Users.Find(UC.currentUser.UserID);
            paymentInfo.Email = findEmail.Email;
            database.Entry(paymentInfo).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            ViewBag.EditedConfirmationPage = "The information on this Confirmation Page has been EDITED";
            ViewBag.Index = UC.currentUser.CurrentIndex;
            return View();
        } // confirms payment and sends an auto-email

        //public ActionResult EditConfirmationPage () { return View(); }   ???? Do we need this, or can we just use the ConfirmationPage()
    }
}