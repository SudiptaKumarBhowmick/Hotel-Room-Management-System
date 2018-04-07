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
    [Table("Guest_Info")]
    public class Guest_Info
    {
        [Key]
        public int Guest_Id { get; set; }
        [Display(Name = "Guest Name")]
        [Required(ErrorMessage = "Guest Name is required.")]
        public string Guest_name { get; set; }
        [Display(Name = "Guest Address")]
        [Required(ErrorMessage = "Guest Address is required.")]
        public string Guest_addr { get; set; }
        [Display(Name = "Guest Phone")]
        [Required(ErrorMessage = "Guest phone is required.")]
        public string Guest_phone { get; set; }
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }
        [Display(Name = "Age")]
        [Required(ErrorMessage = "age is required.")]
        public int age { get; set; }
        [Display(Name = "Room No")]
        [Required(ErrorMessage = "Room No is required.")]
        public Int32? Room_No { get; set; }
    }
}