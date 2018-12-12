using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public static string HashPassword(string password)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(password));
                return Convert.ToBase64String(data);
            }
        }
    }
}