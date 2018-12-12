using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class Date
    {
        //public DateTime DateStart { get; set; }
        //public DateTime DateEnd { get; set; }
        //public static  { get; set; }

        public static DateTime[] DepartureDate(int totalTrips)
        {
            DateTime orderDate = new DateTime(8018, 12, 14);
            Random random = new Random();
            DateTime[] startDate = new DateTime[totalTrips];


            for (int i = 0; i < totalTrips - 1; i++)
            {
                int randNum = random.Next(30, 280);
                int hourNum = random.Next(6, 20);
                DateTime createStartDate = orderDate.AddDays(randNum);
                DateTime finalStartDate = createStartDate.AddHours(hourNum);
                startDate[i] = finalStartDate;
            }

            return startDate;
            //int range = (start).Days;            
        }
    }
}