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
    [Table("Admin")]
    public class HotelModelClass
    {
        [Key]
        public int Admin_ID { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Admin Name is required.")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Admin Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required.")]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }
    }
    
}