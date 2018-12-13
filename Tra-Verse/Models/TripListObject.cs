using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class TripListObject
    {
        public int PlanetIndex { get; set; }
        public string DepartureDate { get; set; }
        public int CompanyIndex { get; set; }
        public int NumberOfDays { get; set; }

        public static TripListObject[] GenerateTrips()
        {
            
            Random rand = new Random();
            TripListObject[] tripsWithShips = new TripListObject[30];

            for (int i = 0; i < 29; i++)
            {
                TripListObject obj = new TripListObject();

                obj.PlanetIndex = rand.Next(0, 16);
                //obj.TravelIndex = rand.Next(1, 30);
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
            //string path = "/Content/Images/Planets/";
            string[] planetPhotos = new string[16] { "HD1461c.jpg", "HD142b.jpg", "HD142c.jpg", "HD564b.jpg",
                    "HD1461b.png", "HD1502b.png", "KELT1b.jpg", "GJ3021b.jpg", "WASP26b.png", "WASP32b.png",
                    "WASP44b.png", "WASP96b.PNG","WASP158b.PNG", "HATS34b.png", "WASP136b.png", "Qatar4b.png" };
            return planetPhotos;
        }
    }
}