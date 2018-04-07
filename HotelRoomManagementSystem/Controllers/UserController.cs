using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelRoomManagementSystem.Models;
using System.Data.Entity;
using HotelRoomManagementSystem.Common;
using PagedList;
using PagedList.Mvc;

namespace HotelRoomManagementSystem.Controllers
{
    public class UserController : Controller
    {
        SampleDBContext db = new SampleDBContext();

        // GET: User
        public ActionResult Index(string SearchBy, string SearchTerm, int? page)
        {
            if (Session["User_Name"] != null)
            {
                var hotel = db.hotel_Info.AsQueryable();
                //var guest = db.guest_Info.AsQueryable();

                if (SearchBy == "Room_No")
                {
                    int check = 0;
                    //bool result = int.TryParse(SearchTerm, out check);
                    if (int.TryParse(SearchTerm, out check))
                    {
                        var ST = int.Parse(SearchTerm);
                        hotel = hotel.Where(x => x.Room_No == ST || SearchTerm == null);
                        return View(hotel.OrderBy(x => x.Room_No).ToPagedList(page ?? 1, 8));
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    hotel = hotel.Where(x => x.Room_type.StartsWith(SearchTerm) || SearchTerm == null);
                    return View(hotel.OrderBy(x => x.Room_type).ToPagedList(page ?? 1, 8));
                }
                //else
                //{
                //    var guest_Info = guest.Where(x => x.Guest_name.StartsWith(SearchTerm) || SearchTerm == null);
                //    return View(guest_Info.OrderBy(x => x.Guest_name).ToPagedList(page ?? 1, 3));
                //}
            }
            else
            {
                return RedirectToAction("LogIn");
            }

        }

        //Get Book
        [HttpGet]
        public ActionResult Book()
        {
            if (Session["User_Name"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //Post Book
        [HttpPost]
        public ActionResult Book(Guest_Info guest)
        {
            if (ModelState.IsValid)
            {
                db.guest_Info.Add(guest);
                Hotel_Info hotel = db.hotel_Info.Where(x => x.Room_No == guest.Room_No).FirstOrDefault();
                hotel.IsBooked = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        //Http Leave
        [HttpGet]
        public ActionResult Leave(int? id)
        {
            if (Session["User_Name"] != null)
            {
                Guest_Info guest = db.guest_Info.Where(x => x.Room_No == id).FirstOrDefault();
                return View(guest);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //Http Leave
        [HttpPost]
        public ActionResult Leave(Guest_Info guest)
        {
            UserClass user = new UserClass();
            int id = guest.Guest_Id;
            if (user.removeUser(id))
            {
                Hotel_Info hotel = db.hotel_Info.Where(x => x.Room_No == guest.Room_No).FirstOrDefault();
                hotel.IsBooked = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guest);
        }
        
        //Details
        public ActionResult Details(int? id)
        {
            if (Session["User_Name"] != null)
            {
                Guest_Info guest = db.guest_Info.Where(x => x.Room_No == id).FirstOrDefault();
                return View(guest);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //LogIn Get
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        //LogIn Post
        [HttpPost]
        public ActionResult LogIn(User userModel)
        {
            var user = db.user.Where(x => x.User_Email == userModel.User_Email && x.password == userModel.password).FirstOrDefault();
            if (user != null)
            {
                Session["User_Name"] = user.User_Name.ToString();
                Session["User_ID"] = user.User_ID.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Email or password is invalid");
            }
            return View();
        }

        //Registration Get
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //registration Post
        [HttpPost]
        public ActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                User userClass = db.user.Where(x => x.User_Email == user.User_Email).FirstOrDefault();
                if (userClass == null)
                {
                    db.user.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Email is already exist");
                }
            }
            ModelState.Clear();
            ViewBag.Message = "Registration Successfull";
            return RedirectToAction("LogIn");
        }

        //LogOut
        public ActionResult LogOut()
        {
            Session.Abandon();

            return RedirectToAction("LogIn");
        }

        //http Get for profile
        [HttpGet]
        public ActionResult profile()
        {
            if (Session["User_ID"] != null)
            {
                string User_ID = Session["User_ID"].ToString();
                int ID = int.Parse(User_ID);
                Models.User user = db.user.Where(x => x.User_ID == ID).FirstOrDefault();
                return View(user);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //http post for profile
        [HttpPost]
        public ActionResult profile(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "your profile is updated";
                return RedirectToAction("profile");
            }
            else
            {
                return View(user);
            }
        }

    }
}