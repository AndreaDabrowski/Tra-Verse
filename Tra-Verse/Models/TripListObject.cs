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

        public static string[] Planets()
        {
            string path = "~/Content/Images/Planets/";
            string[] planetPhotos = new string[16] { path + "GJ 3021 b.jpg", path+"HATS-34 b.png", path+"HD 142 b.png", path+"HD 142 c.jpg", path+"HD 1461 b.png", path+"HD 1502 b.png", path+"HD 564 b.jpg", path+"HD1461c.jpg", path+"KELT-1 b.jpg", path+"Qatar-4 b.jpg", path+"WASP-136 b.png", path+"WASP-158 b.PNG", path+"WASP-26 b.png", path+"WASP-32 b.png", path+"WASP-44 b.png", path+"WASP-96 b.PNG" };
            //string[] planetPhotos = new string[16] { "GJ3021b.jpg", "HATS34b.png", "HD142b.png", "HD142c.jpg", "HD1461b.png", "HD1502b.png", "HD564b.jpg", "HD1461c.jpg", "KELT1b.jpg", "Qatar4b.jpg", "WASP136b.png", "WASP158b.PNG", "WASP26b.png", "WASP32b.png", "WASP44b.png", "WASP96b.PNG" };

            return planetPhotos;
        }

    }
}