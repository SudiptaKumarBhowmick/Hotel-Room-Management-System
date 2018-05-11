using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Mvc;

namespace HotelRoomManagementSystem.Models
{
    [Table("AllGuestInfo")]
    public class AllGuestInfo
    {
        public int ID { get; set; }
        [Display(Name = "Name")]
        public string Guest_Name { get; set; }
        [Display(Name = "Address")]
        public string Guest_addr { get; set; }
        [Display(Name = "Phone")]
        public string Guest_phone { get; set; }
        [Display(Name = "Country/state")]
        public string Country { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "age")]
        public int age { get; set; }
        [Display(Name = "Room")]
        public string Room_type { get; set; }
        public int Guest_Id { get; set; }
        [Display(Name = "Room No")]
        public int Room_No { get; set; }
        public Boolean IsBooked { get; set; }
    }
}