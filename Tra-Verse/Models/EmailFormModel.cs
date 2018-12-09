using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Tra_Verse.Controllers;

namespace Tra_Verse.Models
{
    public class EmailFormModel
    {
        public static string ConfirmationEmail()
        {
            TraVerseEntities database = new TraVerseEntities();
            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);
            User user = database.Users.Find(UserController.currentUser.UserID);
            //List<char> lastFourDigits = new List<char>();
            //for (int i = 12; i <16; i++)
            //{
            //    lastFourDigits.Add(user.CreditCard[i]);
            //}
            string message = "Confirmation of your vacation with Traverse" +
                "Trip Details: " + Environment.NewLine+
                "\nPlanet Name: " + vacationInfo.PlanetName.ToString()+
                "\nVacation Rating: " + vacationInfo.Rating.ToString() + Environment.NewLine+
                "Ship Choice: " + vacationInfo.ShipOption.ToString() + Environment.NewLine+
                "Departure Date: " + vacationInfo.DateStart.ToString() + Environment.NewLine+
                "Return Date: " + vacationInfo.DateEnd.ToString() + Environment.NewLine+
                "" + Environment.NewLine+
                "TOTAL: " + vacationInfo.Price.ToString() + Environment.NewLine+
                "" + Environment.NewLine +
                "This amount was charged to: " + user.NameOnCard.ToString() + Environment.NewLine +
                "Card number: XXXX-XXXX-XXXX-" + user.CreditCard.ToString() + Environment.NewLine +
                "Thank you!";

                return message;
        }
    }
}