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

            foreach(var id in foundID)
            {
                if (logUser.Email == id.Email && logUser.Password == id.Password)
                {
                    if (currentUser.LoggedIn == false)
                    {
                        currentUser.LoggedIn = true;
                        currentUser.UserID = id.UserID;
                        currentUser.OrderID = id.OrderID;
                        TempData["LoggedIn"] = "You've successfully logged in!";
                        return RedirectToAction("Login", "Home");//, logUser
                    }
                }
                if (logUser.Email == currentUser.Email)
                {
                    TempData["CurrentlyLoggedIn"] = "You are already logged in";
                    return RedirectToAction("Login", "Home");
                }
            }
            TempData["InvalidLogin"] = "Invalid Username or Password";
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
                return RedirectToAction("TripList", "Home");
            }
            else
            {
                return RedirectToAction("LoginError", "Home");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoggedIn()
        {
            return View();
        }

        public ActionResult Registered()
        {
            return View();
        }
    }
}