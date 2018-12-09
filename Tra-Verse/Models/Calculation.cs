using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tra_Verse.Controllers;

namespace Tra_Verse.Models
{
    public class Calculation
    {
        public static int TripPriceRandomizer(int index)
        {
            var yelp = API.Yelp();
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

        public static int TotalPrice(string ship, int dollarSign)
        {
            var Nasa = API.NASA("notSorted");
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

        //public static int 

        //public static Array CreateTripList()
        //{
        //    Random rand = new Random();
        //    int companyShipOption = rand.Next(1, 4);
        //    //int destPlanetOption = rand.Next(0, 17);
        //    //int numOfDestinations = rand.Next(1, 4);
        //    var nasa = API.NASA("notSorted");
        //    var yelp = API.Yelp();
        //    var API.Travel();
        //    //List<T> publicTripList = new List<T>();
        //}
    }
}