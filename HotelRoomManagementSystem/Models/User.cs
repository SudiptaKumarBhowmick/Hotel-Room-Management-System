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
    [Table("User")]
    public class User
    {
        [Key]
        public int User_ID { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "User Name is required.")]
        public string User_Name { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "User Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string User_Email { get; set; }
        [Display(Name = "password")]
        [Required(ErrorMessage = "password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "phone is required.")]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
        [Display(Name = "Age")]
        [Required(ErrorMessage = "Age is required.")]
        public int age { get; set; }
    }
}