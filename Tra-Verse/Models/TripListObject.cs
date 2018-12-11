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

        public static string[] PlanetImagingSystem()
        {
            string[] planetImageSystem = new string[] { "https://exoplanets.nasa.gov/newworldsatlas/4255/hd-1461-c/",
            "https://exoplanets.nasa.gov/newworldsatlas/1269/hd-142-b/", "https://exoplanets.nasa.gov/newworldsatlas/1270/hd-142-c/",
            "https://exoplanets.nasa.gov/newworldsatlas/5722/hd-564-b/", "https://exoplanets.nasa.gov/newworldsatlas/285/hd-1461-b/",
            "https://exoplanets.nasa.gov/newworldsatlas/1271/hd-1502-b/", "https://exoplanets.nasa.gov/newworldsatlas/1713/kelt-1-b/",
            "https://exoplanets.nasa.gov/newworldsatlas/1136/gj-3021-b/", "https://exoplanets.nasa.gov/newworldsatlas/5665/wasp-26-b/",
            "https://exoplanets.nasa.gov/newworldsatlas/5668/wasp-32-b/", "https://exoplanets.nasa.gov/newworldsatlas/5676/wasp-44-b/",
            "https://exoplanets.nasa.gov/newworldsatlas/5152/wasp-96-b/", "https://exoplanets.nasa.gov/newworldsatlas/6372/wasp-158-b/",
            "https://exoplanets.nasa.gov/newworldsatlas/3426/hats-34-b/", "https://exoplanets.nasa.gov/newworldsatlas/3433/wasp-136-b/",
            "https://exoplanets.nasa.gov/newworldsatlas/3487/qatar-4-b/"};
            return planetImageSystem;
        }

        public static string[] Planets()
        {
            string path = "~/Content/Images/Planets/";
            string[] planetPhotos = new string[16] { path + "HD1461c.jpg", path + "HD142b.png", path + "HD142c.jpg",
                    path + "HD564b.jpg", path + "HD1461b.png", path + "HD1502b.png", path + "KELT1b.jpg", path + "GJ3021b.jpg",
                    path + "WASP26b.png", path + "WASP32b.png", path + "WASP44b.png", path + "WASP96b.PNG", path + "WASP158b.PNG",
                    path + "HATS34b.png", path + "WASP136b.png", path + "Qatar4b.jpg" };
            return planetPhotos;
        }
    }
}