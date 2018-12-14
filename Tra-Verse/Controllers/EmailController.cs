using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tra_Verse.Models;

namespace Tra_Verse.Controllers
{
    public class EmailController : Controller
    {
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmationPage()
        {
            TraVerseEntities database = new TraVerseEntities();
            VacationLog vacationInfo = database.VacationLogs.Find(UserController.currentUser.OrderID);
            User user = database.Users.Find(UserController.currentUser.UserID);
            var message = new MailMessage();
            message.To.Add(new MailAddress(user.Email));   
            message.From = new MailAddress("TraVerseAlwaysMovingForward@outlook.com");  
            message.Subject = "Confirmation of your vacation with TraVerse";
            message.Body = string.Format("<p>Confirmation of your vacation with TraVerse</p>" +
            "<p>Trip Details: </p>" +
            "<p>Planet Name: " + vacationInfo.PlanetName.ToString() + "</p>" +
            "<p>Vacation Rating: " + vacationInfo.Rating.ToString() + "</p>" +
            "<p>Space Suit Available? "+vacationInfo.Exosuit.ToString()+"</p>"+
            "Departure Date: " + vacationInfo.DateStart.ToString() + "</p>" +
            "Return Date: " + vacationInfo.DateEnd.ToString() + "</p>" +
            "</br>" +
            "TOTAL: " + vacationInfo.Price.ToString() + "</p>" +
            "</br>" +
            "<p>This amount was charged to: " + user.NameOnCard.ToString() + "</p>" +
            "<p>Card number: " + user.CreditCard.ToString() + "</p>" +
            "<p>Refunds allowed? " + vacationInfo.Refundable.ToString() + "</p>"+
            "Thank you! :)");
            
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "TraVerseAlwaysMovingForward@Outlook.com",  
                    Password = "GucciBoi"  
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                TempData["EmailSent!"] = "Your Email Has Been Sent!";
                return RedirectToAction("ConfirmationPage", "Home");
            }
            //return RedirectToAction("ConfirmationPage");
        }
    }
}