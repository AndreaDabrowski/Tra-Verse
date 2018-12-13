using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tra_Verse.Controllers;

namespace Tra_Verse.Models
{
    public class Calculation
    {
        public static int TripPriceRandomizer(string dollar)
        {
            int pricePerDollarSign = 0;
            Random rand = new Random();

            switch (dollar)
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

        public static int TotalPrice(string ship, string suit, int basePrice, string dollar)
        {
            int suitCharge = 0;
            switch (suit)
            {
                case "Yes":
                    suitCharge = 500001;
                    break;
                case "No":
                    suitCharge = 01;
                    break;
                default:
                    suitCharge = 0;
                    break;
            }
            
            int pricePerDollar = TripPriceRandomizer(dollar);

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
            
            int totalPrice = basePrice + priceShipOption + suitCharge + pricePerDollar;
            return totalPrice;
        }
    }
}