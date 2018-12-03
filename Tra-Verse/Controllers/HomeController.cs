using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;


//testing this commit
namespace Tra_Verse.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Always Moving Forward";
            return View();
        }

        public ActionResult NASA()
        {
            string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            HttpWebRequest request = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&format=json");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JArray nasaJson = JArray.Parse(data);
            ViewBag.Example = nasaJson;

            
            rd.Close();

            return View();
        }

        public ActionResult Yelp()
        {
            HttpClient headerToken = new HttpClient();
            headerToken.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
            
            string request = "https://api.yelp.com/v3/businesses/search?location=bos";
            StreamReader rd = new StreamReader(headerToken.GetStreamAsync(request).Result);
            string data = rd.ReadToEnd();

            JObject yelpJson = JObject.Parse(data);
            ViewBag.YelpInfo = yelpJson;
            rd.Close();

            return View();
        }

        public ActionResult TripList() { return View(); }

        public ActionResult TripDetails() { return View(); }

        public ActionResult PrivateAccomodations() { return View(); }

        public ActionResult PublicAccomodations() { return View(); }

        public ActionResult EditTrip() { return View(); }

        public ActionResult CheckOut() { return View(); } // takes and submits payment information

        public ActionResult ConfirmationPage() { return View(); } // confirms payment and sends an auto-email

        //public ActionResult EditConfirmationPage () { return View(); }   ???? Do we need this, or can we just use the ConfirmationPage()

    }
}