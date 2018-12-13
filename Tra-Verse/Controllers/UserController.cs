using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tra_Verse.Models;
using System.Security.Cryptography;
using System.Text;

namespace Tra_Verse.Controllers
{
    public class UserController : Controller
    {
        public static CurrentUser currentUser = new CurrentUser();

        public ActionResult LoginButton(User logUser)
        {
            TraVerseEntities database = new TraVerseEntities();

            logUser.Password = CurrentUser.HashPassword(logUser.Password);
            currentUser.Password = CurrentUser.HashPassword(currentUser.Password);
            List<User> userList = database.Users.ToList();

            foreach (var id in userList)
            {
                if (logUser.Email == id.Email && logUser.Password == id.Password)
                {
                    if (currentUser.LoggedIn == false)
                    {
                        currentUser.LoggedIn = true;
                        currentUser.UserID = id.UserID;
                        currentUser.OrderID = id.OrderID;
                        TempData["LoggedIn"] = "You've successfully logged in!";
                        return RedirectToAction("Login");//, logUser
                    }
                }
            }
            if (logUser.Email == currentUser.Email && logUser.Password == currentUser.Password)
            {
                TempData["CurrentlyLoggedIn"] = "You are already logged in";
                return RedirectToAction("Login");
            }
            if (logUser.Email != currentUser.Email || logUser.Password != currentUser.Password)
            {
                TempData["InvalidLogin"] = "Invalid Username or Password";
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            if (currentUser.LoggedIn == true)
            {
                currentUser.LoggedIn = false;
                currentUser.UserID = 0;
                currentUser.OrderID = 0;
                ViewBag.Logout = "You've been Logged out!";
                return RedirectToAction("Login");
            }
            ViewBag.LoggedOut = "You're not logged in";
            return RedirectToAction("Login");
        }

        public ActionResult RegisterUser(User newUser)
        {
            TraVerseEntities database = new TraVerseEntities();

            if (ModelState.IsValid)
            {
                List<User> foundID = database.Users.ToList();
                foreach (var user in foundID)
                {
                    if (newUser.Email == user.Email)
                    {
                        TempData["AlreadyRegistered"] = "These credentials already match an existing account";
                        return RedirectToAction("Login");
                    }
                }

                newUser.Password = CurrentUser.HashPassword(newUser.Password);

                User added = database.Users.Add(newUser);
                database.SaveChanges();
                currentUser.UserID = added.UserID;
                currentUser.LoggedIn = true;
                currentUser.OrderID = 0;
                
                return RedirectToAction("Registered");//, added
            }
            else
            {
                TempData["Error"] = "Error with adding user.";
                return RedirectToAction("Login");
            }
        }

        public ActionResult ValidateUser(UserBEValidation validUser)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("TripList", "Private");
            }
            else
            {
                return RedirectToAction("LoginError", "Home");
            }
        }
        public ActionResult TotalPrice(FormCollection variables)
        {
            ViewBag.ShipType = variables["ShipType"];
            ViewBag.Exosuit = variables["ExoSuit"];
            ViewBag.DateEnd = variables["DateEnd"];
            ViewBag.DateStart = variables["DateStart"];
            ViewBag.Rating = variables["Rating"];
            ViewBag.PlanetName = variables["PlanetName"];
            ViewBag.Refundable = variables["Refundable"];
            ViewBag.CompanyName = variables["CompanyName"];

            ViewBag.PlanetIndex = variables["PlanetIndex"];
            ViewBag.CompanyIndex = variables["CompanyIndex"];
            ViewBag.TravelIndex = variables["TravelIndex"];

            string shipType = ViewBag.ShipType;
            string exoSuit = ViewBag.Exosuit;
            string rating = ViewBag.Rating;

            string test = variables["BasePrice"].ToString();

            int pr = int.Parse(test);
            ViewBag.RefreshedTotal = Calculation.TotalPrice(variables["ShipType"], variables["ExoSuit"], pr, variables["Rating"]);
            //ViewBag.RefreshedTotal = Calculation.TotalPrice(shipType, exoSuit, 5, rating);
            ViewBag.PlanetPic = TripListObject.Planets();
            return View();//how to send trip indices
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
        public ActionResult LoginError()
        {
            return View();
        }
    }
}