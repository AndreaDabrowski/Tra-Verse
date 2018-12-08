using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Tra_Verse.Models
{
    public class API
    {

        public static JArray NASA(string sortOption)
        {
            //string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            if(sortOption == "notSorted")
            {
                HttpWebRequest nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&select=pl_name,st_dist&format=json");

            }
            else if (sortOption == "sortedByPlanet")
            {
                HttpWebRequest nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&select=pl_name,st_dist&format=json");
            }
            else if (sortOption == "sortedByDestination")
            {

            }
            HttpWebResponse response = (HttpWebResponse)nasaRequest.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JArray nasaJson = JArray.Parse(data);

            rd.Close();

            return nasaJson;
        }

        public static JObject Yelp()
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
        public static JObject Travel()
        {
        
            string travelAPIKey = System.Configuration.ConfigurationManager.AppSettings["Travel API Key"];
            HttpWebRequest travelRequest = WebRequest.CreateHttp("https://api.sandbox.amadeus.com/v1.2/flights/low-fare-search/v1.2/flights/low-fare-search?apikey=" + travelAPIKey + "&origin=BOS&destination=LON&departure_date=2018-12-25");
            HttpWebResponse response = (HttpWebResponse)travelRequest.GetResponse();

            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JObject travelJson = JObject.Parse(data);

            rd.Close();
            return travelJson;
        }
    }
}