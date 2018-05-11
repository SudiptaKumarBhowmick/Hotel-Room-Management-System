using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelRoomManagementSystem.Models;
using HotelRoomManagementSystem.Common;

namespace AdminPanel.Controllers
{
    public class UserListForAdminController : Controller
    {
        SampleDBContext db = new SampleDBContext();

        //User list
        public ActionResult UserList()
        {
            if (Session["Admin_Name"] != null)
            {
                List<User> user = new List<User>();
                user = db.user.ToList();
                return View(user);
            }
            return RedirectToAction("LogIn","User");
        }

        //Delete User Account// Get
        public ActionResult DeleteUserAccount(int id)
        {
            if (Session["Admin_Name"] != null)
            {
                User user = new User();
                user = db.user.Find(id);
                db.user.Remove(user);
                db.SaveChanges();
                TempData["AlertMessage"] = "Are you sure to delete this user record?";
                return RedirectToAction("Index");
            }
            return RedirectToAction("LogIn","User");
        }

        //Show User Details Of an user
        public ActionResult UserDetails(int id)
        {
            if (Session["Admin_Name"] != null)
            {
                User user = new User();
                user = db.user.Single(x => x.User_ID == id);
                return View(user);
            }
            return RedirectToAction("LogIn","User");
        }
    }
}