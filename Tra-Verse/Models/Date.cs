using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class Date
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public static  { get; set; }

        public Date()
        {
            StartDate = new DateTime(8018, 12, 14);
            Random random = new Random();
            int randNum = random.Next(30, 280);
            EndDate = StartDate.AddDays(randNum);
            //int range = (start).Days;            
        }
    }
}