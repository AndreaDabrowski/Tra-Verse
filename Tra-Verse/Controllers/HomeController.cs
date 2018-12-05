using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;


//testing this commit
namespace Tra_Verse.Controllers
{
    public class HomeController : Controller
    {
        UserController UC = new UserController();
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
            ViewBag.Index = index;

            return View();
        }

        public ActionResult PublicAccomodations() { return View(); }

        public ActionResult EditTrip() { return View(); }

        public ActionResult CheckOut() { return View(); } // takes and submits payment information

        public ActionResult ConfirmationPage() { return View(); } // confirms payment and sends an auto-email

        //public ActionResult EditConfirmationPage () { return View(); }   ???? Do we need this, or can we just use the ConfirmationPage()
    }
}