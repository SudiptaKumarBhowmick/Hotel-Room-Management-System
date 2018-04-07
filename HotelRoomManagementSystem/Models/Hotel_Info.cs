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
    [Table("Hotel_Info")]
    public class Hotel_Info
    {
        [Key]
        [Display(Name = "Room No")]
        [Required(ErrorMessage = "Room No is required.")]
        public int Room_No { get; set; }
        public string Room_type { get; set; }
        public Boolean IsBooked { get; set; }
    }
}