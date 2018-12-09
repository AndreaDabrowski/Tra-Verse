using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class TripListObject
    {
        public int RandomPlanetIndex { get; set; }
        public int RandomTravelIndex { get; set; }
        public int RandomCompanyIndex { get; set; }

        public static TripListObject[] GenerateShips()
        {
            //int delta = 1;
            //int spirit = 2;
            //int southwest = 3;
            Random rand = new Random();
            TripListObject[] tripsWithShips = new TripListObject[30];

            for (int i = 0; i < 29; i++)
            {
                TripListObject obj = new TripListObject();

                obj.RandomPlanetIndex = rand.Next(0, 16);
                obj.RandomTravelIndex = rand.Next(1, 30);
                obj.RandomCompanyIndex = rand.Next(1, 4);

                tripsWithShips[i] = obj;
            }
            return tripsWithShips;
        }
    }
}