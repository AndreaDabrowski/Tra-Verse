using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tra_Verse.Models;

namespace Tra_Verse.Controllers
{
    public class UserController : Controller
    {
        TraVerseEntities database = new TraVerseEntities();
        public static CurrentUser currentUser = new CurrentUser();

        public ActionResult Logout()
        {
            if (currentUser.LoggedIn == true)
            {
                currentUser.LoggedIn = false;
                currentUser.UserID = 0;
                currentUser.OrderID = 0;
                ViewBag.Logout = "You've been Logged out!";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LoggedOut = "You're not logged in";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoginButton(User logUser)
        {
            List<User> foundID = database.Users.ToList();
            foreach (User user in foundID)
            {
                if (logUser.Email == user.Email)
                {
                    if (currentUser.LoggedIn == false)
                    {
                        currentUser.LoggedIn = true;
                        currentUser.UserID = user.UserID;
                        currentUser.OrderID = user.OrderID;
                        TempData["LoggedIn"] = "You've successfully logged in!";
                        return RedirectToAction("TripList", "Home");//, logUser
                    }
                    return RedirectToAction("TripList", "Home");
                }
            }

            ViewBag.Error = "This is not a valid email address";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RegisterUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                User added = database.Users.Add(newUser);
                database.SaveChanges();
                currentUser.UserID = added.UserID;
                currentUser.LoggedIn = true;
                currentUser.OrderID = 0;
                TempData["AddedUser"] = "You've been registered";
                return RedirectToAction("Index", "Home");//, added
            }
            else
            {
                TempData["Error"] = "Error with adding user.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}