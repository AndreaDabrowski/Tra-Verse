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
                return RedirectToAction("Login", "Home");
            }
            ViewBag.LoggedOut = "You're not logged in";
            return RedirectToAction("Login", "Home");
        }

        public ActionResult LoginButton(User logUser)
        {
            List<User> foundID = database.Users.ToList();
            foreach (User userDB in foundID)
            {
                if (logUser.Email == userDB.Email)
                {
                    if (currentUser.LoggedIn == false && currentUser.Password == userDB.Password)
                    {
                        currentUser.LoggedIn = true;
                        currentUser.UserID = userDB.UserID;
                        currentUser.OrderID = userDB.OrderID;
                        TempData["LoggedIn"] = "You've successfully logged in!";
                        return RedirectToAction("TripList", "Home");//, logUser
                    }
                    if (logUser.Password != userDB.Password || logUser.Email != userDB.Email)
                    {
                        TempData["InvalidLogin"] = "Invalid Username or Password";
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        return View("LoggedIn");
                    }
                }
            }

            ViewBag.Error = "This is not a valid email address";
            return RedirectToAction("Login", "Home");
        }

        public ActionResult RegisterUser(User newUser)
        {
            
            if (ModelState.IsValid)
            {
                List<User> foundID = database.Users.ToList();
                foreach (var user in foundID)
                {
                    if (newUser.Email == user.Email)
                    {
                        TempData["AlreadyRegistered"] = "These credentials already match an existing account";
                        return RedirectToAction("Login", "Home");
                    }
                }

                User added = database.Users.Add(newUser);
                database.SaveChanges();
                currentUser.UserID = added.UserID;
                currentUser.LoggedIn = true;
                currentUser.OrderID = 0;
                
                return RedirectToAction("Registered", "Home");//, added
            }
            else
            {
                TempData["Error"] = "Error with adding user.";
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult ValidateUser(UserBEValidation validUser)
        {

            if (ModelState.IsValid)
            {
                return View("TripList", "Home");
            }
            else
            {
                return View("LoginError", "Home");
            }
        }
    }
}