using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class TripListObject
    {
        public int PlanetIndex { get; set; }
        public int TravelIndex { get; set; }
        public int CompanyIndex { get; set; }
        public int NumberOfDays { get; set; }

        public static TripListObject[] GenerateTrips()
        {
            //int delta = 1;
            //int spirit = 2;
            //int southwest = 3;
            Random rand = new Random();
            TripListObject[] tripsWithShips = new TripListObject[30];

            for (int i = 0; i < 29; i++)
            {
                TripListObject obj = new TripListObject();

                obj.PlanetIndex = rand.Next(0, 16);
                obj.TravelIndex = rand.Next(1, 30);
                obj.CompanyIndex = rand.Next(0, 3);
                obj.NumberOfDays = rand.Next(3, 11);

                tripsWithShips[i] = obj;
            }

            return tripsWithShips;
        }

    }
}