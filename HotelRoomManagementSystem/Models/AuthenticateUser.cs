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
    [Table("AuthenticateUser")]
    public class AuthenticateUser
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        [Display(Name ="password")]
        [Required(ErrorMessage ="password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name ="Email")]
        [Required(ErrorMessage ="Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string UserLevel { get; set; }
        public Int32? Admin_ID { get; set; }
        public Int32? User_ID { get; set; }
    }
}