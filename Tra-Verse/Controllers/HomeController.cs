
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
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
            //1. request and response, setting up to make the call out to the API
            string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            HttpWebRequest request = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&format=json");
            //+nasaAPIKey
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //2. Reading the reponse into a streamreader
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            //nothing works from here on down

            //3.JSON object, Json token list
            JArray nasaJson = JArray.Parse(data);// must parse else would throw the JSON reader exception, its not in JObject format at all
            ViewBag.Example = nasaJson;//this one doesnt have children, results are formatted differently
                                       //you know what key to choose if your website has good JSON documentation (its the manual that tells you what youre looking for, where it's nested and how to find it

            rd.Close();

            //4. foreach to cycle through list
            //List<GiphyModel> output = new List<GiphyModel>(); //dont forget your output



            //5. return the view with an object
            return View();
        }

        public ActionResult SkyScanner()
        {
            string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            HttpWebRequest request = WebRequest.CreateHttp("https://images-api.nasa.gov");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JObject nasaJson = JObject.Parse(data);
            ViewBag.Example = nasaJson;
            rd.Close();

            return View();
        }
    }
}