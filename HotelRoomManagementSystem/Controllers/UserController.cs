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
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelRoomManagementSystem.Controllers
{
    public class UserController : Controller
    {
        SampleDBContext db = new SampleDBContext();
        int[] RoomNo = new int[100];

        UserClass userclassobject = new UserClass();

        //LogIn Get
        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        //LogIn Post
        [HttpPost]
        public ActionResult LogIn(AuthenticateUser userInfo)
        {
            AuthenticateUser User = db.authenticateUser.Where(x => x.Email == userInfo.Email && x.password == userInfo.password).FirstOrDefault();

            if (User != null)
            {
                if (User.UserLevel == "Level1")
                {
                    Session["Admin_Name"] = User.UserName.ToString();
                    Session["Admin_ID"] = User.ID.ToString();
                    Session["Admin_Email"] = User.Email.ToString();
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.UserLevel == "Level2")
                {
                    Session["User_Name"] = User.UserName.ToString();
                    Session["User_ID"] = User.ID.ToString();
                    Session["User_Email"] = User.Email.ToString();
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return RedirectToAction("LogIn", "User");
                }
            }
            ModelState.AddModelError("", "LogIn failed. Please Input valid Email or password.");
            return View();
        }

        // GET: User
        public ActionResult Index(string SearchBy, string SearchTerm, int? page)
        {
            if (Session["User_Name"] != null)
            {
                var hotel = db.hotel_Info.AsQueryable();
                var guestInfo = db.guest_Info.AsQueryable(); 
                //var guest = db.guest_Info.AsQueryable();
                int pageNumber = page ?? 1;

                if (SearchBy == "Room_No")
                {
                    int check = 0;
                    //bool result = int.TryParse(SearchTerm, out check);
                    if (int.TryParse(SearchTerm, out check))
                    {
                        var ST = int.Parse(SearchTerm);
                        hotel = hotel.Where(x => x.Room_No == ST || SearchTerm == null);
                        int pageSize = hotel.Count();
                        return View(hotel.OrderBy(x => x.Room_No).ToPagedList(pageNumber,pageSize));
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
                return RedirectToAction("LogIn");
            }

        }

        //Get single Booking
        [HttpGet]
        public ActionResult singleBooking(int id)
        {
            if (Session["User_Name"] != null)
            {
                ViewBag.RoomNo = id;
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //Post single Booking
        [HttpPost]
        public ActionResult singleBooking(Guest_Info guest)
        {
            string paramID = RouteData.Values["id"].ToString();
            if (paramID == guest.Room_No.ToString())
            {
                if (ModelState.IsValid)
                {
                    db.guest_Info.Add(guest);
                    Hotel_Info hotel = db.hotel_Info.Where(x => x.Room_No == guest.Room_No).FirstOrDefault();
                    hotel.IsBooked = true;
                    db.SaveChanges();
                    int RoomNo;
                    if (guest.Room_No.HasValue)
                    {
                        RoomNo = (int)guest.Room_No;
                        userclassobject.addAllGuestInfo(RoomNo);
                    }
                    return RedirectToAction("Index");
                }

            }
            ModelState.AddModelError("", "Room No should not be changed. Input '"+paramID+"'");
            return View(guest);
        }

        //Get multiple Booking
        [HttpGet]
        public ActionResult multipleBooking()
        {
            if (Session["User_Name"] != null)
            {
                //int count = 0;
                //foreach (var item in Room_No)
                //{
                //    RoomNo[count] = item;
                //    count++;
                //}
                List<Hotel_Info> hotelInfoList = db.hotel_Info.Where(x => x.IsBooked == false).ToList();
                ViewBag.HotelList = hotelInfoList;
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }

        }

        //post multiple Booking 
        [HttpPost]
        public ActionResult multipleBooking(FormCollection collection)
        {
            if (Session["User_Name"] != null)
            {
                userclassobject.MultipleBookingUser(collection);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("LogIn");            
            }
        }

        //Http Leave
        [HttpGet]
        public ActionResult Leave(int? id)
        {
            if (Session["User_Name"] != null)
            {
                Guest_Info guest = db.guest_Info.Where(x => x.Room_No == id).FirstOrDefault();
                List<Guest_Info> multipleguest = db.guest_Info.Where(x => x.Guest_name == guest.Guest_name && x.Guest_addr == guest.Guest_addr && x.Guest_phone == guest.Guest_phone && x.Country == guest.Country && x.City == guest.City && x.age == guest.age).ToList();
                if (multipleguest.Count() > 1)
                {
                    ViewBag.multipleRoomLeave = multipleguest.Count();
                }
                return View(guest);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //Http Leave
        [HttpPost]
        [SubmitButtonSelector(Name = "SingleRoomLeave")]
        public ActionResult SingleRoomLeave(Guest_Info guest)
        {
            if (Session["User_Name"] != null)
            {
                UserClass user = new UserClass();
                int id = guest.Guest_Id;
                if (user.removeUser(id))
                {
                    Hotel_Info hotel = db.hotel_Info.Where(x => x.Room_No == guest.Room_No).FirstOrDefault();
                    AllGuestInfo guestInfo = db.allGuestInfo.Where(x => x.Guest_Id == guest.Guest_Id).FirstOrDefault();
                    hotel.IsBooked = false;
                    guestInfo.IsBooked = false;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(guest);
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //post multiple Leave
        [HttpPost]
        [SubmitButtonSelector(Name = "multipleRoomLeave")]
        public ActionResult multipleRoomLeave(Guest_Info Guest)
        {
            if (Session["User_Name"] != null)
            {
                if (userclassobject.removeMultipleUser(Guest) == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(Guest);
                }
            }
            else
            {
                return RedirectToAction("LogIn");
            }
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

        ////LogIn Get
        //[HttpGet]
        //public ActionResult LogIn()
        //{
        //    return View();
        //}

        ////LogIn Post
        //[HttpPost]
        //public ActionResult LogIn(User userModel)
        //{
        //    var user = db.user.Where(x => x.User_Email == userModel.User_Email && x.password == userModel.password).FirstOrDefault();
        //    if (user != null)
        //    {
        //        Session["User_Name"] = user.User_Name.ToString();
        //        Session["User_ID"] = user.User_ID.ToString();
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Email or password is invalid");
        //    }
        //    return View();
        //}

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
                    User userID = db.user.Where(x => x.User_Name == user.User_Name && x.User_Email == user.User_Email && x.password == user.password && x.phone == user.phone && x.age == user.age).FirstOrDefault();
                    userclassobject.LevelTwoAuthenticateUser(user.User_Name, user.password, user.User_Email, userID.User_ID);
                    return RedirectToAction("LogIn");
                }
                else
                {
                    ModelState.AddModelError("", "Email is already exist");
                }
            }
            ModelState.Clear();
            return View();
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
            if (Session["User_Email"] != null)
            {
                //string User_ID = Session["User_ID"].ToString();
                //int ID = int.Parse(User_ID);
                //Models.User user = db.user.Where(x => x.User_ID == ID).FirstOrDefault();
                //return View(user);
                string email = Session["User_Email"].ToString();
                Models.User user = db.user.Where(x => x.User_Email == email).FirstOrDefault();
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
            if (Session["User_Email"] != null)
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
            else
            {
                return RedirectToAction("LogIn");
            }
        }

        //Get for GuestList
        public ActionResult GuestList(string SearchByName, string SearchByCity)
        {
            if (Session["User_Email"] != null)
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
            return RedirectToAction("LogIn");
        }


    }
}