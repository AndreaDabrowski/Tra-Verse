using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tra_Verse.Models
{
    public class CurrentUser
    {
        public int UserID { get; set; }
        public bool LoggedIn { get; set; }
        public int? OrderID { get; set; }

        public CurrentUser()
        {
            UserID = 0;
            LoggedIn = false;
            OrderID = 0;
        }
    }
}