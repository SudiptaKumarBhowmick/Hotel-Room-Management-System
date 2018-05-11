using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HotelRoomManagementSystem.Models;
using PagedList;
using PagedList.Mvc;
using HotelRoomManagementSystem.Common;
using System.Data.Entity;

namespace AdminPanel.Controllers
{
    public class AdminController : Controller
    {
        SampleDBContext db = new SampleDBContext();

        // GET: Admin
        public ActionResult Index(string SearchBy, string SearchTerm, int? page)
        {
            if (Session["Admin_Name"] != null)
            {
                var hotel = db.hotel_Info.AsQueryable();
                int pageNumber = page ?? 1;

                if (SearchBy == "Room_No")
                {
                    int check = 0;
                    if (int.TryParse(SearchTerm, out check))
                    {
                        var ST = int.Parse(SearchTerm);
                        hotel = hotel.Where(x => x.Room_No == ST || SearchTerm == null);
                        int pageSize = hotel.Count();
                        return View(hotel.OrderBy(x => x.Room_No).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    hotel = hotel.Where(x => x.Room_type.StartsWith(SearchTerm) || SearchTerm == null);
                    int pageSize = hotel.Count();
                    return View(hotel.OrderBy(x => x.Room_type).ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                return RedirectToAction("LogIn","User");
            }
        }

        //Registration Get
        //[HttpGet]
        //public ActionResult Registration()
        //{
        //    return View();
        //}

        //registration Post
        //[HttpPost]
        //public ActionResult Registration(HotelModelClass admin)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        HotelModelClass adminClass = db.hotelModelClass.Where(x => x.Email == admin.Email).FirstOrDefault();
        //        if (adminClass == null)
        //        {
        //            db.hotelModelClass.Add(admin);
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Email is already exist");
        //        }
        //    }
        //    ModelState.Clear();
        //    ViewBag.Message = "Registration Successfull";
        //    return RedirectToAction("LogIn");
        //}

        //LogOut

        public ActionResult LogOut()
        {
            Session.Abandon();

            return RedirectToAction("LogIn","User");
        }

        //Details
        public ActionResult Details(int? id)
        {
            if (Session["Admin_Name"] != null)
            {
                Hotel_Info hotel = db.hotel_Info.Single(x => x.Room_No == id);
                return View(hotel);
            }
            else
            {
                return RedirectToAction("LogIn","User");
            }
        }

        //Create new Room
        //Get
        [HttpGet]
        public ActionResult AddRoom()
        {
            if (Session["Admin_Name"] != null)
            {
                return View();
            }
            return RedirectToAction("LogIn", "User");
        }

        //Post
        [HttpPost]
        public ActionResult AddRoom(Hotel_Info hotel)
        {
            if (ModelState.IsValid)
            {
                UserClass user = new UserClass();
                user.AddRoom(hotel);

                return RedirectToAction("LogIn","User");
            }
            else
            {
                return View(hotel);
            }
        }

        //Get Action method, Editing Room
        [HttpGet]
        public ActionResult EditRoom(int? id)
        {
            if (Session["Admin_Name"] != null)
            {
                Hotel_Info hotel = db.hotel_Info.Single(x => x.Room_No == id);
                return View(hotel);
            }
            return RedirectToAction("LogIn","User");
        }

        //Post action method, Editing Room
        [HttpPost]
        public ActionResult EditRoom(Hotel_Info hotel)
        {
            if (ModelState.IsValid)
            {
                Hotel_Info h = db.hotel_Info.Where(x => x.Room_No == hotel.Room_No).FirstOrDefault();
                if (h == null)
                {
                    UserClass user = new UserClass();
                    user.EditRoom(hotel);
                }
                else
                {
                    ModelState.AddModelError("", "Room is already exist.");
                    return View(hotel);
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(hotel);
            }
        }

        //Http Get for Delete
        public ActionResult DeleteRoom(int id)
        {
            if (Session["Admin_Name"] != null)
            {
                Guest_Info guest = db.guest_Info.Where(x => x.Room_No == id).FirstOrDefault();
                if (guest == null)
                {
                    Hotel_Info hotel = db.hotel_Info.Find(id);
                    db.hotel_Info.Remove(hotel);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Are you sure to delete this record?";
                    return RedirectToAction("Index");
                }
                TempData["AlertMessage"] = "You can't delete this data";
                return RedirectToAction("Index");
            }
            return RedirectToAction("LogIn","User");
        }

        //http Get for profile
        [HttpGet]
        public ActionResult profile()
        {
            if (Session["Admin_ID"] != null)
            {
                //string User_ID = Session["User_ID"].ToString();
                //int ID = int.Parse(User_ID);
                //HotelModelClass admin = db.hotelModelClass.Where(x => x.Admin_ID == ID).FirstOrDefault();
                //return View(admin);
                string email = Session["Admin_Email"].ToString();
                HotelModelClass admin = db.hotelModelClass.Where(x => x.Email == email).FirstOrDefault();
                return View(admin);
            }
            else
            {
                return RedirectToAction("LogIn","User");
            }
        }

        //http post for profile
        [HttpPost]
        public ActionResult profile(HotelModelClass admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "your profile is updated";
                return RedirectToAction("profile");
            }
            else
            {
                return View(admin);
            }
        }

        //Http Get 
        [HttpGet]
        public ActionResult AllInfo()
        {
            if (Session["Admin_Name"] != null)
            {
                List<AllGuestInfo> alreadyIn = db.allGuestInfo.Where(x => x.IsBooked == true).ToList();
                List<AllGuestInfo> leave = db.allGuestInfo.Where(x => x.IsBooked == false).ToList();
                List<AllGuestInfo> totalGuest = db.allGuestInfo.ToList();
                AllGuestInfo context = new AllGuestInfo();
                ViewBag.alreadyIn = alreadyIn.Count();
                ViewBag.leave = leave.Count();
                ViewBag.TotalGuest = alreadyIn.Count() + leave.Count();

                List<Hotel_Info> booked = db.hotel_Info.Where(x => x.IsBooked == true).ToList();
                List<Hotel_Info> notbooked = db.hotel_Info.Where(x => x.IsBooked == false).ToList();
                ViewBag.Booked = booked.Count();
                ViewBag.notBooked = notbooked.Count();

                List<AuthenticateUser> admin = db.authenticateUser.Where(x => x.UserLevel == "Level1").ToList();
                List<AuthenticateUser> user = db.authenticateUser.Where(x => x.UserLevel == "Level2").ToList();
                ViewBag.Admin = admin.Count();
                ViewBag.User = user.Count();

                return View();
            }
            return RedirectToAction("LogIn","User");
        }


        //Guest List For Admin
        public ActionResult GuestList(string SearchByName, string SearchByCity)
        {
            if (Session["Admin_Name"] != null)
            {
                if (string.IsNullOrEmpty(SearchByName) == true && string.IsNullOrEmpty(SearchByCity) == true)
                {
                    return View(db.allGuestInfo.ToList());
                }
                else if (string.IsNullOrEmpty(SearchByName) == false && string.IsNullOrEmpty(SearchByCity) == false)
                {
                    IEnumerable<AllGuestInfo> guestinfo = db.allGuestInfo.Where(x => x.Guest_Name.ToLower() == SearchByName.ToLower() && x.City.ToLower() == SearchByCity.ToLower());
                    return View(guestinfo);
                }
                else
                {
                    if (string.IsNullOrEmpty(SearchByName) == false)
                    {
                        IEnumerable<AllGuestInfo> guestinfo = db.allGuestInfo.Where(x => x.Guest_Name.ToLower() == SearchByName.ToLower());
                        return View(guestinfo);
                    }
                    else
                    {
                        IEnumerable<AllGuestInfo> guestinfo = db.allGuestInfo.Where(x => x.City.ToLower() == SearchByCity.ToLower());
                        return View(guestinfo);
                    }
                }
            }
            return RedirectToAction("LogIn","User");
        }

        //Details of a guest
        public ActionResult GuestDetails(int id)
        {
            if (Session["Admin_Name"] != null)
            {
                AllGuestInfo guest = new AllGuestInfo();
                guest = db.allGuestInfo.Single(x => x.ID == id);
                return View(guest);
            }
            return RedirectToAction("LogIn", "User");
        }

        //Add another Admin
        [HttpGet]
        public ActionResult AddNewAdmin()
        {
            if (Session["Admin_Name"] != null)
            {
                return View();
            }
            return RedirectToAction("LogIn","User");
        }

        [HttpPost]
        public ActionResult AddNewAdmin(HotelModelClass Admin)
        {
            if (Session["Admin_Name"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.hotelModelClass.Add(Admin);
                    db.SaveChanges();
                    HotelModelClass adminID = db.hotelModelClass.Where(x => x.Name == Admin.Name && x.Email == Admin.Email && x.password == Admin.password && x.phone == Admin.phone).FirstOrDefault();
                    UserClass userclass = new UserClass();
                    userclass.LevelOneAuthenticateUser(Admin.Name, Admin.password, Admin.Email, adminID.Admin_ID);
                    return RedirectToAction("RemoveAdmin");
                }
                return View(Admin);
            }
            else
            {
                return RedirectToAction("LogIn", "User");
            }
        }

        //Remove an Admin
        [HttpGet]
        public ActionResult RemoveAdmin()
        {
            if (Session["Admin_Name"] != null)
            {
                List<HotelModelClass> adminList = db.hotelModelClass.ToList();
                return View(adminList);
            }
            else
            {
                return RedirectToAction("LogIn", "User");
            }
        }

        [HttpPost]
        public ActionResult RemoveAdmin(int? Admin_ID)
        {
            if (Session["Admin_Name"] != null)
            {
                AuthenticateUser authenticateUser = db.authenticateUser.Where(x => x.Admin_ID == Admin_ID).FirstOrDefault();
                db.authenticateUser.Remove(authenticateUser);
                db.SaveChanges();
                HotelModelClass admin = db.hotelModelClass.Find(Admin_ID);
                db.hotelModelClass.Remove(admin);
                db.SaveChanges();
                return RedirectToAction("RemoveAdmin", "Admin");
            }
            else
            {
                return RedirectToAction("LogIn", "User");
            }
        }
    }
}