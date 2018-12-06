using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class CurrentUser : User
    {
        public bool LoggedIn { get; set; }
        public int CurrentIndex { get; set; }
        public int RandPrice { get; set; }

        public CurrentUser()
        {
            UserID = 0;
            LoggedIn = false;
            OrderID = 0;
            CurrentIndex = -1;
            RandPrice = 0;
        }
    }
}