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
        
        public static JArray NASA(string sortOption)//API has an order_by option built in
        {
            HttpWebRequest nasaRequest;
            //string nasaAPIKey = System.Configuration.ConfigurationManager.AppSettings["NASA API Header"];
            if(sortOption == "notSorted")
            {
                nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&select=pl_name,st_dist,pl_hostname,pl_discmethod,pl_masse,pl_orbper,pl_disc,pl_pelink,pl_edelink,pl_publ_date&count(*)&where=ra<5&format=json");

            }
            else if (sortOption == "sortedByPlanet")
            {
                nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&select=pl_name,st_dist,pl_hostname,pl_discmethod,pl_masse,pl_orbper,pl_disc,pl_pelink,pl_edelink,pl_publ_date&format=json&order_by=pl_name&count(*)&where=ra<5");
            }
            else if (sortOption == "sortedByDestination")
            {
                nasaRequest = WebRequest.CreateHttp("https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?table=exoplanets&select=pl_name,st_dist,pl_hostname,pl_discmethod,pl_masse,pl_orbper,pl_disc,pl_pelink,pl_edelink,pl_publ_date&format=json&order_by=st_dist&count(*)&where=ra<5");

            }
            else
            {
                throw new Exception();
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