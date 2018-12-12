using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class Date
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        //public static  { get; set; }

        public Date()
        {
            DateStart = new DateTime(8018, 12, 14);
            Random random = new Random();
            int randNum = random.Next(30, 280);
            DateEnd = DateStart.AddDays(randNum);
            //int range = (start).Days;            
        }
    }
}