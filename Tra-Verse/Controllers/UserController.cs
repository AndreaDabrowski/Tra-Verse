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
                return View("Index");
            }
            ViewBag.LoggedOut = "You're not logged in";
            return View("Index");
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
                    return View("TripList", "Home");
                }
            }

            ViewBag.Error = "This is not a valid email address";
            return View("Index");
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
                return RedirectToAction("Index");//, added
            }
            else
            {
                TempData["Error"] = "Error with adding user.";
                return View("Index");
            }

        }

        //public ActionResult TaskList()//User CurrentUser
        //{

        //    ViewBag.CurrentUserUserID = currentUser.UserID;


        //    return View();
        //}

        //    public ActionResult UpdateComplete(int taskID)
        //    {
        //        TaskListDBEntities ORM = new TaskListDBEntities();
        //        Task oldTask = ORM.Tasks.Find(taskID);
        //        if (oldTask.Complete == false)
        //        {
        //            oldTask.Complete = true;
        //            ORM.Entry(oldTask).State = System.Data.Entity.EntityState.Modified;
        //            ORM.SaveChanges();
        //            return RedirectToAction("TaskList");
        //        }
        //        else
        //        {
        //            return RedirectToAction("TaskList");
        //        }
        //    }

        //    public ActionResult DeleteTask(int taskID)
        //    {
        //        TaskListDBEntities ORM = new TaskListDBEntities();
        //        Task found = ORM.Tasks.Find(taskID);
        //        ORM.Tasks.Remove(found);
        //        ORM.SaveChanges();
        //        return RedirectToAction("TaskList");

        //    }

        //    public ActionResult AddTask()
        //    {
        //        return View();
        //    }

        //    public ActionResult AddNewTask(Task newTask)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            TaskListDBEntities ORM = new TaskListDBEntities();
        //            newTask.UserID = currentUser.ID;
        //            ORM.Tasks.Add(newTask);
        //            ORM.SaveChanges();
        //            return RedirectToAction("TaskList");

        //        }
        //        else
        //        {
        //            ViewBag.Error = "Error with adding task.";
        //            return View("AddTask");
        //        }
        //    }
    }
}